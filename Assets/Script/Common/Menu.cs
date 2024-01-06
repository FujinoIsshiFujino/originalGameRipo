using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;

    [SerializeField] private GameObject backPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button recipeButton;

    [SerializeField] private ItemDialog itemsDialog;

    [SerializeField] GameObject Player;
    PlayerControl _playerControl;
    CharacterController _characterController;

    //backボタンがどの状態の時に表示されるかを指定するため
    //backボタンは汎用性が高く、状態によってどの画面にもどるかかかわると予想されるのでこの仕様に。
    enum faze
    {
        none,
        pause,
        item
    }
    faze currentFaze;

    void Start()
    {
        pausePanel.SetActive(false);

        pauseButton.onClick.AddListener(Pause);
        backButton.onClick.AddListener(Back);
        resumeButton.onClick.AddListener(Resume);
        itemButton.onClick.AddListener(ToggleItemDialog);
        recipeButton.onClick.AddListener(ToggleRecipeDialog);

        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();

        currentFaze = faze.none;
    }

    void Update()
    {
        Debug.Log("currentFaze " + currentFaze);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);

        _characterController.enabled = false;
        _playerControl.enabled = false;

        backPanel.SetActive(false);
        currentFaze = faze.pause;
    }

    private void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);

        _characterController.enabled = true;
        _playerControl.enabled = true;

        currentFaze = faze.none;
    }

    private void Back()
    {
        if (currentFaze == faze.item)
        {
            ToggleItemDialog();

            backPanel.SetActive(false);
            currentFaze = faze.pause;
        }
        else if (currentFaze == faze.pause)
        {
            // Resume();
        }
    }

    private void ToggleItemDialog()
    {
        Time.timeScale = 0;
        itemsDialog.Toggle();
        backPanel.SetActive(true);

        currentFaze = faze.item;
    }

    private void ToggleRecipeDialog()
    {

    }
}
