using UnityEngine;
using UnityEngine.UI;
using Ace.StoryParts;
using Ace;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;
    public delegate void BecameIdleHandler();
    public event BecameIdleHandler BecameIdle;

    private string dialogueText;
    public Actor Actor { get; private set; }
    public string ActorPose { get; private set; }
    public bool IsShowing { get; private set; }
    public bool IsIdle { get; private set; }

    private float totalTimeInMs = 0f;
    public void ShowDialogue(string dialogueText, Actor actor, string pose)
    {
        this.Actor = actor;
        this.ActorPose = pose;
        this.IsShowing = true;
        this.dialogueText = dialogueText;
        dialoguePanel.Show(dialogueText, actor?.Name ?? "");
        IsIdle = false;
    }

    public void SkipToEnd()
    {
        dialoguePanel.RevealCount = dialogueText.Length;
    }

    public void HideDialogue()
    {
        this.IsShowing = false;
        dialoguePanel.Hide();
    }
    
    public void Update()
    {
        if (!IsShowing)
        {
            return;
        }

        const float RevealCharacterTimeInMs = 100;

        // Get the frame time in seconds
        float frameTime = Time.deltaTime;

        // Optionally, convert to milliseconds for easier interpretation
        float frameTimeInMs = frameTime * 1000;
        totalTimeInMs += frameTimeInMs;
        while (totalTimeInMs > RevealCharacterTimeInMs)
        {
            totalTimeInMs -= RevealCharacterTimeInMs;
            dialoguePanel.RevealCount++;
        }

        if (dialoguePanel.RevealCount >= dialogueText.Length)
        {
            IsIdle = true;
            BecameIdle?.Invoke();
        }
    }
}
