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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = new Game();
        game.CurrentPart.Activate(game, this);
        actionManager.Moved += HandleMoved;
        actionManager.Talked += HandleTalked;
        locationText.text = game.CurrentLocation.Name;

        foreach (Item i in game.Items)
        {
            courtRecord.text = i.Name + " ";
        }
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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMouseDown)
        {
            isMouseDown = true;
        }
        else if (!Input.GetMouseButtonDown(0) && isMouseDown)
        {
            isMouseDown = false;
            Clicked?.Invoke();
        }
        if (game.CurrentPart == null && game.CurrentLocation.AutoStoryPart != null)
        {
            SetStoryPart(game.CurrentLocation.AutoStoryPart);
            game.CurrentLocation.AutoStoryPart = null;
        }
        while (game.CurrentPart != null && game.CurrentPart.CanAdvance(game, this))
        {
            SetStoryPart(game.CurrentPart?.NextPart);
        }
        if (game.CurrentPart == null)
        {
            actionManager.Show(GetNearLocations().Select(l => l.Name), GetLocationTopics().Select(l => l.Name));
        }
        else
        {
            actionManager.Hide();
        }
        courtRecord.text = string.Join('\n', game.Items.Where(l => l.IsVisible).Select(l => l.Name));
    }


    

    
}
