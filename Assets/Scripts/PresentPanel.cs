using Ace.StoryParts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresentPanel : MonoBehaviour
{
    private bool isVisible;
    public Button PresentButton0;
    //public Button PresentOption2;
    //public Button PresentOption3;
    private List<Button> presentButtons = new List<Button>();
    private List<Button> presentButtonClones = new List<Button>();
    public delegate void PresentOptionDelegate(int option);
    public event PresentOptionDelegate Presented;
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
        presentButtons.Clear();
        presentButtons.Add(PresentButton0);
        presentButtonClones.Clear();
        PresentButton0.onClick.AddListener(() => HandleClick(0));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleClick(int choice)
    {
        Presented?.Invoke(choice);
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
    public void Show(IEnumerable<string> items)
    {
        if (items != null && items.Any())
        {
            gameObject.SetActive(true);
            int i = 0;
            foreach (var item in items)
            {
                Button PresentButtonCur = null;

                if (i == 0)
                {
                    PresentButtonCur = PresentButton0;
                }
                else
                {
                    var PresentButtonCloneObj = Instantiate(PresentButton0.gameObject, PresentButton0.transform.parent);
                    var PresentButtonClone = PresentButtonCloneObj.GetComponent<Button>();
                    PresentButtonClone.name = "PresentButton" + (i);
                    PresentButtonClone.transform.localPosition = new Vector3(0, 80 - 40 * (i), 0);
                    presentButtons.Add(PresentButtonClone);
                    presentButtonClones.Add(PresentButtonClone);
                    var index = i;
                    PresentButtonClone.onClick.AddListener(() => HandleClick(index));
                    PresentButtonCur = PresentButtonClone;
                }

                TMP_Text buttonText = PresentButtonCur.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = item;
                i++;
                isActive = true;
            }
        }

    }
    public void Hide()
    {
        foreach (Button b in presentButtonClones)
        {
            Destroy(b.gameObject);
        }
        presentButtonClones.Clear();
        presentButtons.Clear();
        presentButtons.Add(PresentButton0);
        gameObject.SetActive(false);
        isActive = false;
    }
}
