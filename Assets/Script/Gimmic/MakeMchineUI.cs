using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MakeMchineUI : MonoBehaviour
{
    [SerializeField] private GameObject backPanel;
    [SerializeField] private GameObject recipeDialog;

    enum faze
    {
        none,
        pause,
        item
    }
    faze currentFaze;


    [SerializeField] GameObject Player;
    PlayerControl _playerControl;
    CharacterController _characterController;

    bool recipeToggle;
    [SerializeField] private GameObject pausePanel;

    void Start()
    {
        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();

        currentFaze = faze.none;
        recipeToggle = false;
        recipeDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeSelf == true)
        {
            if (Input.GetButton("Dash"))
            {
                CloseRecipeDialog();
            }
        }
    }
    public void makeMchineUI()
    {
        pausePanel.SetActive(true);
        ToggleRecipeDialog();
    }

    public void CloseRecipeDialog()
    {
        ToggleRecipeDialog();
        pausePanel.SetActive(false);
    }

    private void ToggleRecipeDialog()
    {
        if (recipeDialog.activeSelf == true)
        {

            Time.timeScale = 1;

            recipeDialog.SetActive(false);//ここはもしかしたら itemsDialog.Toggle();みたいに専用のをつくったほうがいいかも

            _characterController.enabled = true;
            _playerControl.enabled = true;

            backPanel.SetActive(true);

            currentFaze = faze.item;
        }
        else
        {
            Time.timeScale = 0;

            recipeDialog.SetActive(true);//ここはもしかしたら itemsDialog.Toggle();みたいに専用のをつくったほうがいいかも

            _characterController.enabled = false;
            _playerControl.enabled = false;

            backPanel.SetActive(false);

            currentFaze = faze.item;
        }
    }
}
