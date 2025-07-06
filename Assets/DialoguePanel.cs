using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    private string actorText;
    private string text;

    public TextMeshProUGUI dialogueText;
    public string ActorText
    {
        get
        {
            return actorText;
        }
        set
        {
            actorText = value;
        }
    }
    public string DialogueText
    {
        get
        {
            return text;
        }
        set
        {
            text = value;
            dialogueText.text = value;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
