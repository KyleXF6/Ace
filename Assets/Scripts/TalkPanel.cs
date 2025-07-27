using Ace.StoryParts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    private bool isVisible;
    public Button talkButton0;
    //public Button talkOption2;
    //public Button talkOption3;
    private List<Button> talkButtons = new List<Button>();
    private List<Button> talkButtonClones = new List<Button>();
    public delegate void TalkOptionDelegate(int option);
    public event TalkOptionDelegate Talked;
    public bool isActive;
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
        talkButtons.Clear();
        talkButtons.Add(talkButton0);
        talkButtonClones.Clear();
        talkButton0.onClick.AddListener(() => HandleClick(0));
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
        if(topics != null && topics.Any())
        {
            gameObject.SetActive(true);
            int i = 0;
            foreach (var topic in topics)
            {
                Button talkButtonCur = null;

                if (i == 0)
                {
                    talkButtonCur = talkButton0;
                }
                else
                {
                    var talkButtonCloneObj = Instantiate(talkButton0.gameObject, talkButton0.transform.parent);
                    var talkButtonClone = talkButtonCloneObj.GetComponent<Button>();
                    talkButtonClone.name = "TalkButton" + (i);
                    talkButtonClone.transform.localPosition = new Vector3(0, 120 - 40 * (i), 0);
                    talkButtons.Add(talkButtonClone);
                    talkButtonClones.Add(talkButtonClone);
                    var index = i;
                    talkButtonClone.onClick.AddListener(() => HandleClick(index));
                    talkButtonCur = talkButtonClone;
                }

                TMP_Text buttonText = talkButtonCur.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = topic;
                i++;
                isActive = true;
            }
        }
        
    }
    public void Hide()
    {
        foreach (Button b in talkButtonClones)
        {
            Destroy(b);
        }
        talkButtonClones.Clear();
        talkButtons.Clear();
        talkButtons.Add(talkButton0);
        gameObject.SetActive(false);
        isActive = false;
    }
}
