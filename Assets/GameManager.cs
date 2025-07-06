using AceConsole;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StoryFile storyFile;
    public DialogueManager dialogueManager;
    public LocationManager locationManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storyFile = Loader.Load();
        dialogueManager.StartDialogue(storyFile.Dialogues[0]);
        locationManager.SetLocations(storyFile.Locations);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            dialogueManager.OnNextClicked();
        }
    }

    

    
}
