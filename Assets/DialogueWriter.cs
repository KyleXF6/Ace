
using Ace;
using System;

public class DialogueWriter
{
    private GameManager gameManager;

    public bool IsDone { get; set; }
    public DialogueWriter(GameManager gameManager, string actorName, string line)
    {
        this.gameManager = gameManager;
        gameManager.dialogueManager.BecameIdle += HandleDialogueBecameIdle;
        gameManager.dialogueManager.ShowDialogue(line, actorName);
    }

    private void HandleDialogueBecameIdle()
    {
        if (gameManager != null)
        {
            gameManager.Clicked += HandleClick;
        }
    }

    public void Dispose()
    {
        gameManager.dialogueManager.HideDialogue();
        gameManager.Clicked -= HandleClick;
        gameManager.dialogueManager.BecameIdle -= HandleDialogueBecameIdle;
        gameManager = null;
    }

    private void HandleClick()
    {
        IsDone = true;
    }
}