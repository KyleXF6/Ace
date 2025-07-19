
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections.Generic;

namespace Ace
{
    public class Entity
    {
        public string Id { get; set; }

        public Dictionary<string, object> Refs { get; set; } = new Dictionary<string, object>();


        public static List<WeakReference<Entity>> AllRefs = new List<WeakReference<Entity>>();

        public Entity()
        {
            AllRefs.Add(new WeakReference<Entity>(this));
        }


        public static void ClearRefs()
        {
            AllRefs = new List<WeakReference<Entity>>();
        }

        public static void ResolveRefs()
        {
            var idMap = new Dictionary<string, Entity>();

            foreach (var weakRef in AllRefs)
            {
                if (!weakRef.TryGetTarget(out Entity eRef))
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(eRef.Id))
                {
                    if (idMap.ContainsKey(eRef.Id))
                    {
                        throw new DuplicateNameException(eRef.Id);
                    }
                    idMap.Add(eRef.Id, eRef);
                }
            }

            foreach (var weakRef in AllRefs)
            {
                if (!weakRef.TryGetTarget(out Entity eRef))
                {
                    continue;
                }

                if (eRef.Refs != null)
                {
                    foreach (var propName in eRef.Refs.Keys)
                    {
                        object oVal = null;

                        if (eRef.Refs[propName] is string name)
                        {
                            if (!idMap.TryGetValue(name, out Entity val))
                            {
                                throw new KeyNotFoundException(name);
                            }

                            oVal = val;
                        }

                        var prop = eRef.GetType().GetProperty(propName);
                        if (prop == null)
                        {
                            throw new MissingFieldException(propName);
                        }

                        prop.SetValue(eRef, oVal);
                    }
                }
            }
        }
    }
}
