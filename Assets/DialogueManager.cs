using UnityEngine;
using UnityEngine.UI;
using AceConsole; 

public class DialogueManager : MonoBehaviour
{
    // public Button nextButton;

    private Dialogue currentDialogue;
    public DialoguePanel dialoguePanel;
    public void StartDialogue(Dialogue dialogue)
    {

        currentDialogue = dialogue;
        currentDialogue.CurrentLineIndex = 0;
        ShowCurrentLine();
    }

    public void ShowCurrentLine()
    {
        if (currentDialogue == null || currentDialogue.IsDone)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.CurrentLine;
        dialoguePanel.ActorText = line.Actor.Name + $" ({line.Pose})";
        dialoguePanel.DialogueText = line.Line;
    }

    public void OnNextClicked()
    {
        currentDialogue.Advance();
        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.ActorText = "";
        dialoguePanel.DialogueText = "End of dialogue.";
        // nextButton.interactable = false;
    }
}
