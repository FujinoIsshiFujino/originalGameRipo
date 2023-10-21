using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button itemButton;
    [SerializeField] private Button recipeButton;

    [SerializeField] private ItemDialog itemsDialog;


    void Start()
    {
        pausePanel.SetActive(false);

        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        itemButton.onClick.AddListener(ToggleItemDialog);
        recipeButton.onClick.AddListener(ToggleRecipeDialog);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void ToggleItemDialog()
    {
        itemsDialog.Toggle();
    }

    private void ToggleRecipeDialog()
    {

    }

}
