using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using Ace.StoryParts;

// Source story file location: https://drive.google.com/file/d/1fduLL5k78fxAtbs3fESe8X2KghrMGC-p/view?usp=sharing

namespace Ace.Importers
{
    public class DrawioImporter
    {
        private class Dialogue
        {
            public List<DialogueLine> Lines { get; set; } = new List<DialogueLine>();
        }

        private class Diagram
        {
            public string Name { get; }
            public CellNode RootNode { get; }

            public Diagram(string name, CellNode rootNode)
            {
                Name = name;
                RootNode = rootNode;
            }
        }

        private class CellNode
        {
            public CellNode Parent { get; set; }
            public CellNode Source { get; set; }
            public CellNode Target { get; set; }
            public List<CellNode> Children { get; } = new List<CellNode>();
            public XElement Element { get; }

            public CellNode(XElement element)
            {
                Element = element;
            }
        }

        private static CellNode BuildNodeTree(XContainer root)
        {
            CellNode rootNode = null;

            var cellidMap = new Dictionary<string, CellNode>();
            foreach (var elem in root.Descendants("mxCell"))
            {
                XAttribute idAttr = elem.Attribute("id");
                var node = new CellNode(elem);

                if (idAttr != null)
                {
                    cellidMap[idAttr.Value] = node;
                }

                if (elem.Attribute("parent") == null)
                {
                    if (rootNode != null)
                    {
                        throw new FileLoadException("Multiple roots found");
                    }
                    rootNode = node;
                }
            }

            foreach (var elem in root.Descendants("mxCell"))
            {
                XAttribute idAttr = elem.Attribute("id");
                XAttribute parentAttr = elem.Attribute("parent");
                if (idAttr != null && parentAttr != null)
                {
                    cellidMap[idAttr.Value].Parent = cellidMap[parentAttr.Value];
                    cellidMap[parentAttr.Value].Children.Add(cellidMap[idAttr.Value]);
                }
            }

            foreach (var elem in root.Descendants("mxCell"))
            {
                XAttribute idAttr = elem.Attribute("id");
                XAttribute sourceAttr = elem.Attribute("source");
                XAttribute targetAttr = elem.Attribute("target");
                if (idAttr != null && sourceAttr != null && targetAttr != null)
                {
                    cellidMap[idAttr.Value].Source = cellidMap[sourceAttr.Value];
                    cellidMap[idAttr.Value].Target = cellidMap[targetAttr.Value];
                }
            }

            if (rootNode == null)
            {
                throw new FileLoadException("No root node found");
            }

            return rootNode;
        }

        private static List<Diagram> BuildDiagrams(XContainer doc)
        {
            var diagrams = new List<Diagram>();
            foreach (var elem in doc.Descendants("diagram"))
            {
                string name = elem.Attribute("name")?.Value ?? "";
                CellNode rootNode = BuildNodeTree(elem);
                diagrams.Add(new Diagram(name, rootNode));
            }

            return diagrams;
        }

        private static string GetCellNodeAttributeValue(CellNode node, string attribName, bool preserveLineBreaks = false)
        {
            var result = node?.Element?.Attribute(attribName)?.Value ?? string.Empty;
            result = result.Replace("&nbsp;", " ");
            if (preserveLineBreaks)
            {
                result = result.Replace("<div>", "\n");
                result = result.Replace("</div>", " ");
                result = result.Replace("<br>", "\n");
                result = Regex.Replace(result, "<p.*?>", "\n");
                result = result.Replace("</p>", " ");
            }
            else
            {
                result = result.Replace("<div>", " ");
                result = result.Replace("</div>", " ");
                result = result.Replace("<br>", " ");
                result = Regex.Replace(result, "<p.*?>", " ");
                result = result.Replace("</p>", " ");
            }
            result = Regex.Replace(result, "<.*?>", String.Empty);
            result = result.Replace("  ", " ");
            return result.Trim();
        }

        private static string GetCellNodeId(CellNode node)
        {
            string idStr = GetCellNodeAttributeValue(node, "value");
            string id = null;
            var parts = idStr.Split(":", 2);
            if (parts == null || parts.Length != 2)
            {
                id = idStr;
            }
            else
            {
                id = parts[0].Trim();
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                // No ID from title, use the ID assigned by draw.io
                id = GetCellNodeAttributeValue(node, "id");
            }
            return id;
        }

        private static Type GetCellNodeTypeName(CellNode node)
        {
            var idStr = GetCellNodeAttributeValue(node, "value");
            var parts = idStr?.Split(":", 2);
            if (parts == null || parts.Length != 2)
            {
                return null;
            }

            var typeName = parts[1].Trim();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.Name == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        private static void SetInlineDialogueProperties(Dialogue dialogue, CellNode dialogueNode)
        {
            var id = GetCellNodeId(dialogueNode);

            var dialogueLines = new List<DialogueLine>();
            int lineNumber = 0;
            foreach (var lineNode in dialogueNode.Children)
            {
                var propStr = GetCellNodeAttributeValue(lineNode, "value", preserveLineBreaks: true);
                var lines = propStr?.Split("\n");
                if (lines != null)
                {
                    foreach (var lineStr in lines)
                    {
                        if (string.IsNullOrWhiteSpace(lineStr))
                        {
                            continue;
                        }

                        lineNumber++;

                        var parts = lineStr?.Split(":", 2);
                        if (parts == null || parts.Length != 2)
                        {
                            throw new FileLoadException($"While loading Dialogue '{id}': Invalid line specification '{lineStr}'");
                        }
                        var actorId = parts[0].Trim();
                        var line = parts[1].Trim();
                        var pose = "default";

                        var actorParts = actorId.Split("/");
                        actorId = actorParts[0];
                        if (actorParts.Length == 2)
                        {
                            pose = actorParts[1].Trim();
                        }

                        var dialogueLine = new DialogueLine();
                        if (lineNumber == 1)
                        {
                            dialogueLine.Id = id;
                        }
                        else
                        {
                            dialogueLine.Id = $"{id}-line{lineNumber}";
                        }
                        dialogueLine.Refs[nameof(dialogueLine.Actor)] = actorId;
                        dialogueLine.Line = line;
                        dialogueLine.Pose = pose;
                        dialogueLines.Add(dialogueLine);
                    }
                }
            }

            if (dialogueLines.Count == 0)
            {
                throw new FileLoadException($"While loading Dialogue '{id}': Empty dialogues are not allowed");
            }

            for (int i = 1; i < dialogueLines.Count; i++)
            {
                dialogueLines[i - 1].NextPart = dialogueLines[i];
            }

            dialogue.Lines = dialogueLines;
        }

        private static void SetEntityProperties(Entity obj, CellNode objNode, Dictionary<CellNode, object> objMap)
        {
            var objType = obj.GetType();
            var id = GetCellNodeId(objNode);
            if (id != null)
            {
                obj.Id = id;
            }

            foreach (var propNode in objNode.Children)
            {
                var propStr = GetCellNodeAttributeValue(propNode, "value");
                var parts = propStr?.Split(":", 2);
                string propName = null;
                string propVal = null;
                if (parts != null && parts.Length > 1)
                {
                    propName = parts[0].Trim();
                    propVal = parts[1].Trim();
                }
                else
                {
                    propName = propStr;
                }

                propName ??= "";

                var prop = objType.GetProperty(propName);
                if (prop == null)
                {
                    throw new FileLoadException($"While loading {objType.Name} '{id}': Invalid property '{propName}'");
                }
                if (prop.PropertyType == typeof(string))
                {
                    if (propVal == null)
                    {
                        throw new FileLoadException($"While loading {objType.Name} '{id}': Missing inline string value for '{propName}'");
                    }
                    prop.SetValue(obj, propVal);
                }
                else if (prop.PropertyType == typeof(int))
                {
                    if (propVal == null)
                    {
                        throw new FileLoadException($"While loading {objType.Name} '{id}': Missing inline int value for '{propName}'");
                    }
                    prop.SetValue(obj, int.Parse(propVal));
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    if (propVal == null)
                    {
                        throw new FileLoadException($"While loading {objType.Name} '{id}': Missing inline bool value for '{propName}'");
                    }
                    prop.SetValue(obj, propVal.ToUpper() == "TRUE");
                }
                else if ((prop.PropertyType.Name == "IReadOnlyList`1" || prop.PropertyType.Name == "List`1") && 
                    prop.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(Entity)))
                {
                    if (propVal != null)
                    {
                        throw new FileLoadException($"While loading {objType.Name} '{id}': Expected embedded list value for '{propName}'");
                    }

                    // Create a list of type T
                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(prop.PropertyType.GenericTypeArguments[0]);
                    var list = Activator.CreateInstance(constructedListType);
                    foreach (var childNode in propNode.Children)
                    {
                        var item = CreateObjectFromNode(childNode, objMap, prop.PropertyType.GenericTypeArguments[0]);
                        constructedListType.GetMethod("Add")?.Invoke(list, new[] { item });
                    }
                    prop.SetValue(obj, list);
                }
                else
                {
                    if (propVal == null)
                    {
                        // Use inline object property
                        if (propNode.Children.Count != 1)
                        {
                            throw new FileLoadException($"While loading {objType.Name} '{id}': Expected exactly one embedded value for '{propName}'");
                        }

                        var objVal = CreateObjectFromNode(propNode.Children[0], objMap, prop.PropertyType);
                        prop.SetValue(obj, objVal);
                    }
                    else if (propVal == "null")
                    {
                        // Set literal null object
                        prop.SetValue(obj, null);
                    }
                    else
                    {
                        // Assume reference property (as string) and defer assignment
                        obj.Refs[propName] = propVal;
                    }
                }
            }
        }

        private static void AddRootEntities<T>(List<T> entityList, CellNode groupNode) where T : Entity, new()
        {
            foreach (var entityNode in groupNode.Children.Where(c => c.Source == null && c.Target == null))
            {
                var entity = new T();
                SetEntityProperties(entity, entityNode, objMap: null);
                entityList.Add(entity);
            }
        }

        private static void AddLocations(StoryFile storyFile, CellNode locsNode)
        {
            var objMap = new Dictionary<CellNode, object>();
            foreach (var locNode in locsNode.Children.Where(c => c.Source == null && c.Target == null))
            {
                var loc = new Location();
                SetEntityProperties(loc, locNode, objMap);
                storyFile.Locations.Add(loc);
                objMap[locNode] = loc;
            }

            foreach (var locNode in locsNode.Children)
            {
                if (locNode.Source != null && locNode.Target != null)
                {
                    var sourceLoc = (Location)objMap[locNode.Source];
                    var targetLoc = (Location)objMap[locNode.Target];
                    sourceLoc.NearLocations.Add(targetLoc);
                    targetLoc.NearLocations.Add(sourceLoc);
                }
            }
        }

        private static object CreateObjectFromNode(CellNode node, Dictionary<CellNode, object> objMap, Type defaultType)
        {
            object obj = null;

            if (GetCellNodeAttributeValue(node, "style").Contains("fillColor=#ffe6cc"))
            {
                // Orange fill means dialogue shortcut, items are condensed in the list
                var dialogue = new Dialogue();
                SetInlineDialogueProperties(dialogue, node);
                obj = dialogue;
            }
            else if (GetCellNodeAttributeValue(node, "style").Contains("rhombus"))
            {
                // Diamond shape is choice shortcut, choice options are in arrows and handled later
                var choice = new Choice();
                choice.Prompt = GetCellNodeAttributeValue(node, "value");
                obj = choice;
            }
            else if (GetCellNodeAttributeValue(node, "style").Contains("cloud"))
            {
                // Cloud shape is Presentation, presentation options are in arrows and handled later
                var presentation = new Presentation();
                var unknownItemPartId = GetCellNodeAttributeValue(node, "value");
                if (!string.IsNullOrWhiteSpace(unknownItemPartId))
                {
                    presentation.Refs[nameof(presentation.UnknownItemPart)] = unknownItemPartId;
                }
                obj = presentation;
            }
            else
            {
                // Treat everything else as a generic story part and set properties generically
                var type = GetCellNodeTypeName(node);
                if (type == null)
                {
                    type = defaultType;
                }
                if (type == null)
                {
                    throw new FileLoadException("Couldn't find object type to activate");
                }
                obj = Activator.CreateInstance(type);
                if (obj is Entity entity)
                {
                    SetEntityProperties(entity, node, objMap);
                }
            }

            if (obj != null && objMap != null)
            {
                objMap[node] = obj;
            }
            return obj;
        }

        private static object TranslateSource(object source)
        {
            if (source is Dialogue dialogue)
            {
                return dialogue.Lines.Last();
            }
            return source;
        }

        private static object TranslateTarget(object source)
        {
            if (source is Dialogue dialogue)
            {
                return dialogue.Lines.First();
            }
            return source;
        }

        private static void AddStoryParts(StoryFile storyFile, CellNode partsNode)
        {
            var objMap = new Dictionary<CellNode, object>();
            foreach (var partNode in partsNode.Children.Where(c => c.Source == null && c.Target == null))
            {
                IStoryPart part = CreateObjectFromNode(partNode, objMap, null) as IStoryPart;
                if (part != null)
                {
                    storyFile.StoryParts.Add(part);
                }
            }

            foreach (var partNode in partsNode.Children)
            {
                if (partNode.Source != null && partNode.Target != null)
                {
                    if (!objMap.TryGetValue(partNode.Source, out var source))
                    {
                        throw new FileLoadException($"Arrow '{GetCellNodeAttributeValue(partNode, "id")}' has invalid source");
                    }

                    if (!objMap.TryGetValue(partNode.Target, out var target))
                    {
                        throw new FileLoadException($"Arrow '{GetCellNodeAttributeValue(partNode, "id")}' has invalid target");
                    }

                    source = TranslateSource(source);
                    target = TranslateTarget(target);

                    string arrowText = partNode.Children.Count > 0 ? GetCellNodeAttributeValue(partNode.Children[0], "value") : null;

                    if (!string.IsNullOrWhiteSpace(arrowText))
                    {
                        // Named arrow
                        if (source is Choice choice)
                        {
                            // Arrow is connecting a choice to a story part
                            var option = new ChoiceOption();
                            option.Line = arrowText;
                            option.NextPart = (IStoryPart)target;
                            choice.Options.Add(option);
                        }
                        else if (source is Presentation presentation && target is IStoryPart presentationTarget)
                        {
                            // Arrow is connecting a presentation to a story part
                            var itemId = arrowText;
                            var item = storyFile.Items.First(i => i.Id == itemId);
                            var pItem = new PresentationItem()
                            {
                                Item = item,
                                StoryPart = presentationTarget
                            };
                            presentation.Items.Add(pItem);
                        }
                        else
                        {
                            // Arrow is linking items by property, attempt a direct assignment
                            var sourceType = source.GetType();
                            var prop = sourceType.GetProperty(arrowText);
                            if (prop == null)
                            {
                                throw new FileLoadException($"While loading {sourceType.Name}: Invalid property '{arrowText}'");
                            }
                            prop.SetValue(source, target);
                        }
                    }
                    else
                    {
                        // Unnamed arrow
                        if (source is Presentation presentation && target is IStoryPart presentationTarget)
                        {
                            // Arrow is connecting a choice to a story part
                            presentation.UnknownItemPart = presentationTarget;
                        }
                        else if (source is TestimonyLine sourceTestimony && target is TestimonyLine targetTestimony)
                        {
                            // Testimony has back-links
                            sourceTestimony.NextPart = targetTestimony;
                            targetTestimony.PrevPart = sourceTestimony;
                        }
                        else if (source is IStoryPart sourcePart && target is IStoryPart targetPart)
                        {
                            // Arrow is generically linking story parts
                            sourcePart.NextPart = targetPart;
                        }
                        else
                        {
                            // Arrow is linking invalid objects
                            throw new FileLoadException($"Arrow '{GetCellNodeAttributeValue(partNode, "id")}' is invalid (cannot link '{source.GetType().Name}' and '{target.GetType().Name}'");
                        }
                    }
                }
            }
        }

        private static CellNode FindNode(CellNode node, string value)
        {
            if (GetCellNodeAttributeValue(node, "value") == value)
            {
                return node;
            }

            foreach (var child in node.Children)
            {
                var foundNode = FindNode(child, value);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }

        private static CellNode GetRootEntityNode(CellNode node, string value)
        {
            foreach (var child in node.Children[0].Children)
            {
                if (GetCellNodeAttributeValue(child, "value") == value)
                {
                    return child;
                }
            }

            throw new FileLoadException($"Container '{value}' not found");
        }

        public static StoryFile Import(string filename)
        {
            var storyFile = new StoryFile();

            var doc = XDocument.Load(filename);
            var diagrams = BuildDiagrams(doc);
            var entityDiagram = diagrams.First(d => d.Name == "Entities");
            if (entityDiagram == null)
            {
                throw new FileLoadException("Container 'Entities' not found");
            }

            // Get all top-level groups
            var propsNode = GetRootEntityNode(entityDiagram.RootNode, "Properties");
            var actorsNode = GetRootEntityNode(entityDiagram.RootNode, "Actors");
            var itemsNode = GetRootEntityNode(entityDiagram.RootNode, "Items");
            var locsNode = GetRootEntityNode(entityDiagram.RootNode, "Locations");
            var convsNode = GetRootEntityNode(entityDiagram.RootNode, "Conversations");
            var landmarksNode = GetRootEntityNode(entityDiagram.RootNode, "Landmarks");
            var presentationsNode = GetRootEntityNode(entityDiagram.RootNode, "Presentations");

            var props = new GameProperties();
            SetEntityProperties(props, propsNode, objMap: null);
            storyFile.Properties = props;

            AddLocations(storyFile, locsNode);
            AddRootEntities<Actor>(storyFile.Actors, actorsNode);
            AddRootEntities<Item>(storyFile.Items, itemsNode);
            AddRootEntities<Conversation>(storyFile.Conversations, convsNode);
            AddRootEntities<Landmark>(storyFile.Landmarks, landmarksNode);
            AddRootEntities<Presentation>(storyFile.Presentations, presentationsNode);

            foreach (var diagram in diagrams)
            {
                if (diagram != entityDiagram)
                {
                    var partsNode = diagram?.RootNode?.Children?.First();
                    if (partsNode != null)
                    {
                        AddStoryParts(storyFile, partsNode);
                    }
                }
            }

            return storyFile;
        }
    }
}
