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
    public bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTextMesh(); 
    }
    public int RevealCount { get; set; }

    private void UpdateTextMesh()
    {
        dialogueText.ForceMeshUpdate();

        for (int i = 0; i < dialogueText.textInfo.characterCount; i++)
        {
            var charInfo = dialogueText.textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            int index = charInfo.vertexIndex;

            for (int j = 0; j < 4; ++j)
            {
                byte alpha = i < RevealCount ? (byte)0xff : (byte)0;

                dialogueText.textInfo.meshInfo[charInfo.materialReferenceIndex].colors32[index + j].a = alpha;
            }
        }

        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
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
        isActive = true;
        RevealCount = 0;
        UpdateTextMesh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    
}
