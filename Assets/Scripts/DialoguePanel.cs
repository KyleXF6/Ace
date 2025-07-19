using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    public SpriteRenderer nameBox;
    public SpriteRenderer dialogueBox;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        } else
        {
            nameText.gameObject.SetActive(true);
            nameBox.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
