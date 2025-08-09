
using Ace;
using System;

public class DialogueWriter
{
    private GameManager gameManager;

    public bool IsDone { get; set; }

    private bool DialogueIsIdle { get; set; }
    public DialogueWriter(GameManager gameManager, Actor actor, string pose, string line)
    {
        this.gameManager = gameManager;
        gameManager.dialogueManager.ShowDialogue(line, actor, pose);
        gameManager.Clicked += HandleClick;
    }

    public void Dispose()
    {
        gameManager.dialogueManager.HideDialogue();
        gameManager.Clicked -= HandleClick;
        gameManager = null;
    }

    private void HandleClick()
    {
        if (gameManager.dialogueManager.IsIdle)
        {
            IsDone = true;
        } else
        {
            gameManager.dialogueManager.SkipToEnd();
        }
    }
}