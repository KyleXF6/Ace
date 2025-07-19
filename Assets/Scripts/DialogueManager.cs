using UnityEngine;
using UnityEngine.UI;
using Ace.StoryParts; 

public class DialogueManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;
    public delegate void BecameIdleHandler();
    public event BecameIdleHandler BecameIdle;
    public void ShowDialogue(string dialogueText, string actorText)
    {
    
        dialoguePanel.Show(dialogueText, actorText);
        // Since we're not staging text yet, immediately declare that we are idle
        BecameIdle?.Invoke();
    }

    public void HideDialogue()
    {
        dialoguePanel.Hide();
    }
}
