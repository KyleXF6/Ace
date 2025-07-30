using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestimonyPanel : MonoBehaviour
{
    public SpriteRenderer nameBox;
    public SpriteRenderer dialogueBox;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Button goNextButton;
    public Button goPrevButton;
    public Button PressButton;
    public Button PresentButton;
    public bool isActive = false;
    public delegate void PressOptionDelegate(int option);
    public event PressOptionDelegate Pressed;
    public delegate void PresentOptionDelegate(int option);
    public event PresentOptionDelegate Presented;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PressButton.onClick.AddListener(() => HandleClick(0));
        PresentButton.onClick.AddListener(() => HandleClick(1));
    }

    private void HandleClick(int option)
    {
        if(option == 0)
        {
            Pressed?.Invoke(option);
        } else
        {
            Presented?.Invoke(option);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show(string dialogueText, string actorText)
    {
        this.dialogueText.text = dialogueText;
        nameText.text = actorText;
        if (string.IsNullOrEmpty(actorText))
        {
            nameText.gameObject.SetActive(false);
            nameBox.gameObject.SetActive(false);
        }
        else
        {
            nameText.gameObject.SetActive(true);
            nameBox.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }
}