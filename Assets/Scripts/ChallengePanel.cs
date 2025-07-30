using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengePanel : MonoBehaviour
{
    public SpriteRenderer promptBox;
    public TMP_Text promptText;
    public bool isActive = false;
    private bool isVisible;
    public Button ChallengeButton0;
    //public Button ChallengeOption2;
    //public Button ChallengeOption3;
    private List<Button> ChallengeButtons = new List<Button>();
    private List<Button> ChallengeButtonClones = new List<Button>();
    public delegate void ChallengeOptionDelegate(int option);
    public event ChallengeOptionDelegate Challenged;

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
        ChallengeButtons.Clear();
        ChallengeButtons.Add(ChallengeButton0);
        ChallengeButtonClones.Clear();
        ChallengeButton0.onClick.AddListener(() => HandleClick(0));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void HandleClick(int choice)
    {
        Challenged?.Invoke(choice);
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
    public void Show(IEnumerable<string> items, string promptText)
    {
        this.promptText.text = promptText;
        if (items != null && items.Any())
        {
            gameObject.SetActive(true);
            int i = 0;
            foreach (var item in items)
            {
                Button ChallengeButtonCur = null;

                if (i == 0)
                {
                    ChallengeButtonCur = ChallengeButton0;
                }
                else
                {
                    var ChallengeButtonCloneObj = Instantiate(ChallengeButton0.gameObject, ChallengeButton0.transform.parent);
                    var ChallengeButtonClone = ChallengeButtonCloneObj.GetComponent<Button>();
                    ChallengeButtonClone.name = "ChallengeButton" + (i);
                    ChallengeButtonClone.transform.localPosition = new Vector3(0, 80 - 40 * (i), 0);
                    ChallengeButtons.Add(ChallengeButtonClone);
                    ChallengeButtonClones.Add(ChallengeButtonClone);
                    var index = i;
                    ChallengeButtonClone.onClick.AddListener(() => HandleClick(index));
                    ChallengeButtonCur = ChallengeButtonClone;
                }
                TMP_Text buttonText = ChallengeButtonCur.GetComponentInChildren<TMP_Text>(true);
                buttonText.text = item;
                i++;
                isActive = true;
            }
        }

    }
    public void Hide()
    {
        foreach (Button b in ChallengeButtonClones)
        {
            Destroy(b.gameObject);
        }
        ChallengeButtonClones.Clear();
        ChallengeButtons.Clear();
        ChallengeButtons.Add(ChallengeButton0);
        gameObject.SetActive(false);
        isActive = false;
    }
}
