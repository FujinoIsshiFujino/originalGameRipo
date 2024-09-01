using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MakeMchineUI : MonoBehaviour
{
    [SerializeField] private GameObject backPanel;

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

    void Start()
    {
        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();

        currentFaze = faze.none;
    }

    public void CloseMenu(GameObject menu)
    {
        Time.timeScale = 1;

        menu.SetActive(false);

        _characterController.enabled = true;
        _playerControl.enabled = true;

        backPanel.SetActive(true);
    }

    public void OpenMenu(GameObject menu)
    {
        Time.timeScale = 0;

        menu.SetActive(true);

        _characterController.enabled = false;
        _playerControl.enabled = false;

        backPanel.SetActive(false);
    }
}
