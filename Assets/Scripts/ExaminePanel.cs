using Ace.StoryParts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExaminePanel : MonoBehaviour
{
    private bool isVisible;
    public Button ExamineButton0;
    //public Button ExamineOption2;
    //public Button ExamineOption3;
    private List<Button> ExamineButtons = new List<Button>();
    private List<Button> ExamineButtonClones = new List<Button>();
    public delegate void ExamineOptionDelegate(int option);
    public event ExamineOptionDelegate Examined;
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
        ExamineButtons.Clear();
        ExamineButtons.Add(ExamineButton0);
        ExamineButtonClones.Clear();
        ExamineButton0.onClick.AddListener(() => HandleClick(0));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleClick(int choice)
    {
        Examined?.Invoke(choice);
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
    public void Show(IEnumerable<string> landmarks)
    {
        if (landmarks != null && landmarks.Any())
        {
            gameObject.SetActive(true);
            int i = 0;
            foreach (var landmark in landmarks)
            {
                Button ExamineButtonCur = null;

                if (i == 0)
                {
                    ExamineButtonCur = ExamineButton0;
                }
                else
                {
                    var ExamineButtonCloneObj = Instantiate(ExamineButton0.gameObject, ExamineButton0.transform.parent);
                    var ExamineButtonClone = ExamineButtonCloneObj.GetComponent<Button>();
                    ExamineButtonClone.name = "ExamineButton" + (i);
                    ExamineButtonClone.transform.localPosition = new Vector3(0, 120 - 40 * (i), 0);
                    ExamineButtons.Add(ExamineButtonClone);
                    ExamineButtonClones.Add(ExamineButtonClone);
                    var index = i;
                    ExamineButtonClone.onClick.AddListener(() => HandleClick(index));
                    ExamineButtonCur = ExamineButtonClone;
                }

                TMP_Text buttonText = ExamineButtonCur.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = landmark;
                i++;
                isActive = true;
            }
        }

    }
    public void Hide()
    {
        foreach (Button b in ExamineButtonClones)
        {
            Destroy(b);
        }
        ExamineButtonClones.Clear();
        ExamineButtons.Clear();
        ExamineButtons.Add(ExamineButton0);
        gameObject.SetActive(false);
        isActive = false;
    }
}
