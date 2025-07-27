using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class MovePanel : MonoBehaviour
{
    public Button moveButton0;
    private List<Button> moveButtons = new List<Button>();
    private List<Button> moveButtonClones = new List<Button>();
    public delegate void MoveOptionDelegate(int option);
    public event MoveOptionDelegate Moved;
    public bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveButtons.Clear();
        moveButtons.Add(moveButton0);
        moveButtonClones.Clear();
        moveButton0.onClick.AddListener(() => HandleClick(0));
    }

    private void HandleClick(int option)
    {
        Moved?.Invoke(option);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(IEnumerable<string> locationNames)
    {
        gameObject.SetActive(true);
        int i = 0;
        foreach (var locationName in locationNames)
        {
            Button moveButtonCur = null;

            if (i == 0)
            {
                moveButtonCur = moveButton0;
            }
            else
            {
                var moveButtonCloneObj = Instantiate(moveButton0.gameObject, moveButton0.transform.parent);
                var moveButtonClone = moveButtonCloneObj.GetComponent<Button>();
                moveButtonClone.name = "MoveButton" + (i);
                moveButtonClone.transform.localPosition = new Vector3(0, 120 - 40 * (i), 0);
                moveButtons.Add(moveButtonClone);
                moveButtonClones.Add(moveButtonClone);
                var index = i;
                moveButtonClone.onClick.AddListener(() => HandleClick(index));
                moveButtonCur = moveButtonClone;
            }

            TMP_Text buttonText = moveButtonCur.GetComponentInChildren<TMP_Text>(true);
            buttonText.text = locationName;
            i++;
            isActive = true;
        }

    }

    public void Hide()
    {
        foreach(Button b in moveButtonClones)
        {
            Destroy(b);
        }
        moveButtonClones.Clear();
        moveButtons.Clear();
        moveButtons.Add(moveButton0);
        gameObject.SetActive(false);
        isActive = false;
    }

}
