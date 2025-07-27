using UnityEngine;
using UnityEngine.UI;
using Ace.StoryParts;
using Unity.VisualScripting;
using System.Collections.Generic;

public enum Action
{
    Move,
    Examine,
    Talk,
    Present
}

public class ActionPanel : MonoBehaviour
{
    private bool isVisible;
    public Button moveButton;
    public Button presentButton;
    public Button examineButton;
    public Button talkButton;
    public delegate void ActionDelegate(Action action);
    public event ActionDelegate ActionTaken;
    public bool isActive;
    public bool IsVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            isVisible = value;
            gameObject.SetActive(value);
            //textGui.gameObject.SetActive(isVisible);
        }
    }

    void Start()
    {
        moveButton.onClick.AddListener(() => HandleClick(Action.Move));
        examineButton.onClick.AddListener(() => HandleClick(Action.Examine));
        talkButton.onClick.AddListener(() => HandleClick(Action.Talk));
        presentButton.onClick.AddListener(() => HandleClick(Action.Present));
        
    }

    private void HandleClick(Action action)
    {
        ActionTaken?.Invoke(action);
    }


    // Update is called once per frame
    void Update()
    {

    }

}
