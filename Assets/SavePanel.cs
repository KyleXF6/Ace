using Ace;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Ace.GameFileManager;

public class SavePanel : MonoBehaviour
{
    public GameObject saveSlotProto;
    public Button closeButton;

    private List<Button> saveButtons = new List<Button>();
    private List<Button> loadButtons = new List<Button>();
    private List<TMP_Text> descriptionTexts = new List<TMP_Text>();
    private Game currentGame;
    private bool createdSlots;

    public delegate void GameLoadedHandler(Game newGame);
    public event GameLoadedHandler GameLoaded;
    public bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closeButton.onClick.AddListener(HandleCloseButtonClick);
    }

    private void HandleCloseButtonClick()
    {
        Hide();
    }

    private void CreateSlots()
    {
        if (createdSlots)
        {
            return;
        }

        saveSlotProto.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            var saveSlotClone = Instantiate(saveSlotProto, saveSlotProto.transform.parent);
            saveSlotClone.transform.localPosition = new Vector3(0,
                saveSlotProto.transform.localPosition.y -
                    ((RectTransform)saveSlotProto.transform).rect.height * i,
                0);
            saveSlotClone.gameObject.SetActive(true);
            saveSlotClone.name = $"SaveSlot{i}";

            var saveSlotIndex = i;

            var saveButtonObj = saveSlotClone.transform.Find("SaveButton");
            var saveButton = saveButtonObj.GetComponent<Button>();
            saveButton.onClick.AddListener(() => HandleSaveClick(saveSlotIndex));

            var loadButtonObj = saveSlotClone.transform.Find("LoadButton");
            var loadButton = loadButtonObj.GetComponent<Button>();
            loadButton.onClick.AddListener(() => HandleLoadClick(saveSlotIndex));

            var descriptionObj = saveSlotClone.transform.Find("Description");
            var descriptionText = descriptionObj.GetComponent<TMP_Text>();

            saveButtons.Add(saveButton);
            loadButtons.Add(loadButton);
            descriptionTexts.Add(descriptionText);
        }

        createdSlots = true;
    }

    private void UpdateUI()
    {
        CreateSlots();
        for (int i = 0; i < 3; i++)
        {
            if (GameFileManager.HasSaveGame(i + 1))
            {
                var gameFile = GameFileManager.LoadGameFile(i + 1);
                descriptionTexts[i].text = $"{gameFile.Time.ToString()}: {gameFile.LocationName}";
                loadButtons[i].enabled = true;
            }
            else
            {
                descriptionTexts[i].text = "Empty";
                loadButtons[i].enabled = false;
            }
        }
    }

    public void Show(Game game)
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        currentGame = game;
        UpdateUI();
        
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Hide()
    {
        currentGame = null;
        gameObject.SetActive(false);
        isActive = false;
    }

    private void HandleSaveClick(int saveSlotIndex)
    {
        GameFileManager.SaveGame(currentGame, saveSlotIndex + 1);
        UpdateUI();
    }

    private void HandleLoadClick(int saveSlotIndex)
    {
        if (!GameFileManager.HasSaveGame(saveSlotIndex + 1))
        {
            return;
        }

        var newGame = GameFileManager.LoadGame(saveSlotIndex + 1);
        GameLoaded?.Invoke(newGame);

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
