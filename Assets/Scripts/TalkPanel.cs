using Ace.StoryParts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    private bool isVisible;
    public Button talkOption1;
    public Button talkOption2;
    public Button talkOption3;
    public delegate void TalkOptionDelegate(int option);
    public event TalkOptionDelegate Talked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        }
    }
    void Start()
    {
        talkOption1.onClick.AddListener(() => HandleClick(0));
        talkOption2.onClick.AddListener(() => HandleClick(1));
        talkOption3.onClick.AddListener(() => HandleClick(2));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleClick(int choice)
    {
        Talked?.Invoke(choice);
    }
    public string GetButtonText(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
        return buttonText.text;
    }
    private void SetButtonText(Button button, string text)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
        buttonText.text = text;
    }
    public void Show(IEnumerable<string> topics)
    {
        gameObject.SetActive(true);
        int i = 0;
        Button[] choices = {talkOption1, talkOption2, talkOption3};
        foreach (string topic in topics)
        {
            SetButtonText(choices[i], topic);
            i++;
        }
        while (i < choices.Length)
        {
            SetButtonText(choices[i], "");
            i++;
        }

    }
    public void Hide()
    {
        gameObject.SetActive(false);

    }
}
