using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public ActionPanel actionPanel;
    public MovePanel movePanel;
    public ExaminePanel examinePanel;
    public TalkPanel talkPanel;
    public PresentPanel presentPanel;
    private IEnumerable<string> locationNames;
    private IEnumerable<string> topics;
    private IEnumerable<string> items;
    private IEnumerable<string> landmarks;
    public delegate void MoveOptionDelegate(int option);
    public event MoveOptionDelegate Moved;
    public delegate void ExamineOptionDelegate(int option);
    public event ExamineOptionDelegate Examined;
    public delegate void TalkOptionDelegate(int option);
    public event TalkOptionDelegate Talked;
    public delegate void PresentOptionDelegate(int option);
    public event PresentOptionDelegate Presented;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionPanel.ActionTaken += HandleActionTaken;
        movePanel.Moved += HandleMoved;
        talkPanel.Talked += HandleTalked;
        presentPanel.Presented += HandlePresented;
        examinePanel.Examined += HandleExamined;

    }

    private void HandlePresented(int option)
    {
        presentPanel.Hide();
        Presented?.Invoke(option);
    }
    private void HandleExamined(int option)
    {
        examinePanel.Hide();
        Examined?.Invoke(option);
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
                if (!movePanel.isActive)
                {
                    movePanel.Show(locationNames);
                } else
                {
                    movePanel.Hide();
                }
                break;
            case Action.Examine:
                if (!examinePanel.isActive)
                {
                    examinePanel.Show(landmarks);
                }
                else
                {
                    examinePanel.Hide();
                }
                break;
            case Action.Talk:
                if(!talkPanel.isActive)
                {
                    talkPanel.Show(topics);
                } else
                {
                    talkPanel.Hide();
                }
                break;
            case Action.Present:
                if (!presentPanel.isActive)
                {
                    presentPanel.Show(items);
                }
                else
                {
                    presentPanel.Hide();
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(IEnumerable<string> locationNames, IEnumerable<string> topics, IEnumerable<string> items, IEnumerable<string> landmarks)
    {
        this.locationNames = locationNames;
        this.topics = topics;
        this.items = items;
        this.landmarks = landmarks;
        actionPanel.IsVisible = true;
        actionPanel.isActive = true;
    }
    

    public void Hide()
    {
        actionPanel.IsVisible = false;
        actionPanel.isActive = false;

    }
}
