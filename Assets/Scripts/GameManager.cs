using Ace;
using Ace.StoryParts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using Ace.Actions;
using Unity.Collections;
using System;
public class GameManager : MonoBehaviour
{
    private bool isMouseDown;
    private Game game;
    private StoryFile storyFile;
    public ActionManager actionManager;
    public DialogueManager dialogueManager;
    public LocationManager locationManager;
    public ChoiceManager choiceManager;
    public TMP_Text locationText;
    public TMP_Text courtRecord;
    public delegate void ClickedHandler();
    public event ClickedHandler Clicked;
    public SavePanel savePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = new Game();
        SetStoryPart(game.CurrentPart);
        actionManager.Moved += HandleMoved;
        actionManager.Talked += HandleTalked;
        actionManager.Presented += HandlePresented;
        actionManager.Presented += HandleExamined;
        locationText.text = game.CurrentLocation.Name;
        savePanel.GameLoaded += HandleGameLoaded;
    }

    private void HandleExamined(int option)
    {
        var landmarks = GetCurrentLandmarks();
        SetStoryPart(landmarks[option].InspectStoryPart);
    }

    private void HandlePresented(int option)
    {
        var items = GetCurrentItems();
        var item = items[option];
        var curPresentation = game.CurrentLocation.Presentation;
        if (curPresentation != null)
        {
            var presentationItem = curPresentation.Items.Where(i => i.Item == item).FirstOrDefault();
            if (presentationItem != null)
            {
                SetStoryPart(presentationItem.StoryPart);
            }
            else
            {
                SetStoryPart(curPresentation.UnknownItemPart);
            }
        }
    }

    private void HandleGameLoaded(Game newGame)
    {
        SetStoryPart(null);
        game = newGame;
        SetStoryPart(game.CurrentPart);
    }

    private void HandleMoved(int option)
    {
        var locations = GetNearLocations();
        game.CurrentLocation = locations[option];
        locationText.text = game.CurrentLocation.Name;
        
    }
    private void HandleTalked(int option)
    {
        var topics = GetLocationTopics();
        SetStoryPart(topics[option].FirstPart);
    }

    private void SetStoryPart(IStoryPart newPart)
    {
        var oldPart = game.CurrentPart;
        game.CurrentPart = newPart;
        oldPart?.Deactivate(game, this);
        newPart?.Activate(game, this);
        if (newPart != null && newPart.IsJournaled)
        {
            game.AddJournalEntry(GameJournalEvent.AdvanceStoryPart, newPart);
        }
    }
    private Location[] GetNearLocations()
    {
        var locations = game.CurrentLocation.NearLocations?.Where(l => l.CanMoveTo);
        if(locations == null)
        {
            return new Location[] { };
        }
        return locations.ToArray();
    }

    private Topic[] GetLocationTopics()
    {
        var topics = game.CurrentLocation.Conversation?.Topics;
        if (topics == null)
        {
            return new Topic[] { };
        }
        return topics.ToArray();
    }

    private Item[] GetCurrentItems()
    {
        var items = game.Items.Where(i => i.IsVisible);
        if (items == null)
        {
            return new Item[] { };
        }
        return items.ToArray();
    }

    private Landmark[] GetCurrentLandmarks()
    {
        var landmarks = game.Landmarks.Where(l => l.IsVisible);
        if (landmarks == null)
        {
            return new Landmark[] { };
        }
        return landmarks.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            savePanel.Show(game);
            actionManager.Hide();
            dialogueManager.dialoguePanel.Hide();
            actionManager.movePanel.Hide();
            actionManager.talkPanel.Hide();
            //choiceManager.choicesPanel.Hide();
            
        }
        if (Input.GetMouseButtonDown(0) && !isMouseDown)
        {
            isMouseDown = true;
        }
        else if (!Input.GetMouseButtonDown(0) && isMouseDown && !savePanel.isActiveAndEnabled)
        {
            isMouseDown = false;
            Clicked?.Invoke();
        }
        if (game.CurrentPart == null && game.CurrentLocation.AutoStoryPart != null)
        {
            SetStoryPart(game.CurrentLocation.AutoStoryPart);
            game.CurrentLocation.AutoStoryPart = null;
            game.AddJournalEntry(GameJournalEvent.ClearLocationAutoStoryPart, game.CurrentLocation);
        }
        while (game.CurrentPart != null && game.CurrentPart.CanAdvance(game, this))
        {
            SetStoryPart(game.CurrentPart?.NextPart);
        }
        if (game.CurrentPart == null)
        {
            actionManager.Show(GetNearLocations().Select(l => l.Name), GetLocationTopics().Select(l => l.Name), GetCurrentItems().Select(l => l.Name), GetCurrentLandmarks().Select(l => l.Name));
        }
        else
        {
            actionManager.Hide();
        }
        courtRecord.text = string.Join('\n', game.Items.Where(l => l.IsVisible).Select(l => l.Name));
    }


    

    
}
