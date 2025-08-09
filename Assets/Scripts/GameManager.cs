using Ace;
using Ace.StoryParts;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEditor.Animations;
using System.Xml;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isMouseDown;
    private Game game;
    private StoryFile storyFile;
    public ActionManager actionManager;
    public DialogueManager dialogueManager;
    public TestimonyPanel testimonyPanel;
    public LocationManager locationManager;
    public ChoiceManager choiceManager;
    public ChallengePanel challengePanel;
    public TMP_Text courtRecord;
    public delegate void ClickedHandler();
    public event ClickedHandler Clicked;
    public SavePanel savePanel;
    public SpriteRenderer backgroundImage;
    public SpriteRenderer actor;
    public Button saveButton;
    public Button courtRecordButton;
    private Location lastLocation;
    private Actor lastActor;
    private string lastPose;
    private bool wasSpeaking;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = new Game();
        SetStoryPart(game.CurrentPart);
        actionManager.Moved += HandleMoved;
        actionManager.Talked += HandleTalked;
        actionManager.Presented += HandlePresented;
        actionManager.Examined += HandleExamined;
        savePanel.GameLoaded += HandleGameLoaded;
        saveButton.onClick.AddListener(() => HandleSaveButtonClicked());
        courtRecordButton.onClick.AddListener(() => HandleCourtRecordButtonClicked());
    }

    private void HandleCourtRecordButtonClicked()
    {
        throw new NotImplementedException();
    }

    public void ShowSavePanel()
    {
        savePanel.Show(game);
        // actionManager.Hide();
        // dialogueManager.dialoguePanel.Hide();
        // actionManager.movePanel.Hide();
        // actionManager.talkPanel.Hide();
        // choiceManager.choicesPanel.Hide();
    }
    private void HandleSaveButtonClicked()
    {
        ShowSavePanel();
    }

    private void SetBackground(string locationId)
    {
        var newSprite = Resources.Load<Sprite>($"Locations/{locationId}");
        backgroundImage.sprite = newSprite;
    }

    private AnimatorController LoadController(string actorId, string pose, bool isSpeaking)
    {
        AnimatorController controller = null;

        if (isSpeaking)
        {
            controller = Resources.Load<AnimatorController>($"Actors/{actorId}/{pose}/speaking");
            if (controller == null)
            {
                controller = Resources.Load<AnimatorController>($"Actors/{actorId}/default/speaking");
            }
        }

        if (controller == null)
        {
            controller = Resources.Load<AnimatorController>($"Actors/{actorId}/{pose}/default");
        }

        if (controller == null)
        {
            controller = Resources.Load<AnimatorController>($"Actors/{actorId}/default/default");
        }
        return controller;
    }

    private void SetActorVisual(string actorId, string pose, bool isSpeaking)
    {
        bool hasActor = !string.IsNullOrEmpty(actorId);
        bool hasPose = !string.IsNullOrEmpty(pose);

        actor.gameObject.SetActive(hasActor);
        if (hasActor)
        {
            if (!hasPose)
            {
                pose = "default";
            }

            var animator = actor.GetComponent<Animator>();
            actor.sprite = null;
            var controller = LoadController(actorId, pose, isSpeaking);
            animator.runtimeAnimatorController = controller;
            actor.sprite = null;
        }
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

    public Item[] GetCurrentItems()
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
        var landmarks = game.CurrentLocation.Landmarks.Where(l => l.IsVisible);
        if (landmarks == null)
        {
            return new Landmark[] { };
        }
        return landmarks.ToArray();
    }

    

    // Update is called once per frame
    void Update()
    {
        
        if (game?.CurrentLocation != lastLocation)
        {
            lastLocation = game?.CurrentLocation;
            if (lastLocation != null)
            {
                SetBackground(lastLocation.Id);
            }
        }

        var currentActor = dialogueManager.IsShowing ?
            dialogueManager.Actor : game?.CurrentLocation?.Actor;

        var currentPose = dialogueManager.IsShowing ?
            dialogueManager.ActorPose : game?.CurrentLocation?.ActorPose;

        var isSpeaking = !dialogueManager.IsIdle;

        if (currentActor?.Id == "pw")
        {
            currentActor = lastActor;
            isSpeaking = false;
        }

        if (currentActor != lastActor || currentPose != lastPose || wasSpeaking != isSpeaking)
        {
            lastActor = currentActor;
            lastPose = currentPose;
            wasSpeaking = isSpeaking;
            SetActorVisual(currentActor?.Id, currentPose, isSpeaking);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowSavePanel();
            
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
