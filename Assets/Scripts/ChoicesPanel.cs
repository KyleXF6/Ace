using UnityEngine;
using UnityEngine.UI;
using Ace.StoryParts;
public class ChoicesPanel : MonoBehaviour
{
    private bool isVisible;
    public Button choiceOption1;
    public Button choiceOption2;
    public Button choiceOption3;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
