using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public ActionPanel actionPanel;
    public MovePanel movePanel;
    public TalkPanel talkPanel;
    private IEnumerable<string> locationNames;
    private IEnumerable<string> topics;
    public delegate void MoveOptionDelegate(int option);
    public event MoveOptionDelegate Moved;
    public delegate void TalkOptionDelegate(int option);
    public event TalkOptionDelegate Talked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionPanel.ActionTaken += HandleActionTaken;
        movePanel.Moved += HandleMoved;
        talkPanel.Talked += HandleTalked;
    }

    private void HandleMoved(int option)
    {
        movePanel.Hide();
        Moved?.Invoke(option);
    }

    private void HandleTalked(int option)
    {
        talkPanel.Hide();
        Talked?.Invoke(option);
    }

    private void HandleActionTaken(Action action)
    {
        switch (action)
        {
            case Action.Move:
                movePanel.Show(locationNames);
                break;
            case Action.Examine:
                break;
            case Action.Talk:
                talkPanel.Show(topics);
                break;
            case Action.Present:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(IEnumerable<string> locationNames, IEnumerable<string> topics)
    {
        this.locationNames = locationNames;
        this.topics = topics;
        actionPanel.IsVisible = true;
    }
    

    public void Hide()
    {
        actionPanel.IsVisible = false;

    }
}
