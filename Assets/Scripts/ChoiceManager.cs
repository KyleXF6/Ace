using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class ChoiceManager : MonoBehaviour
{
    public ChoicesPanel choicesPanel;

    public delegate void ChoiceOptionDelegate(int choice);
    public event ChoiceOptionDelegate ChoiceMade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        choicesPanel.choiceOption1.onClick.AddListener(() => HandleClick(0));
        choicesPanel.choiceOption2.onClick.AddListener(() => HandleClick(1));
        choicesPanel.choiceOption3.onClick.AddListener(() => HandleClick(2));
    }

    private void HandleClick(int choice)
    {
        ChoiceMade?.Invoke(choice);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void ShowChoices(IEnumerable<string> choices)
    {
        choicesPanel.IsVisible = true;
        Button[] choiceOptions = { choicesPanel.choiceOption1, choicesPanel.choiceOption2, choicesPanel.choiceOption3 };
        int i = 0;
        foreach (var choice in choices)
        {
            if (i < choiceOptions.Length)
            {
                SetButtonText(choiceOptions[i], choice);
                i++;
            }
        }
        while (i < choiceOptions.Length)
        {
            SetButtonText(choiceOptions[i], "");
            i++;
        }
    }

    public void HideChoices()
    {
        choicesPanel.IsVisible = false;
    }
}
