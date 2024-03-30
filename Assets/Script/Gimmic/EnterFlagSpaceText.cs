using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterFlagSpaceText : FlagSwitch
{
    [SerializeField] Text anounceText;
    [SerializeField] GameObject anounceTextPanel;
    [SerializeField] string anounceTextMessage;

    public override void Start()
    {
        base.Start();
        if (anounceTextPanel != null)
        {
            anounceTextPanel.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();
        if (firstTime == false)
        {
            if (flagCaller.isOn)
            {
                if (anounceTextPanel != null && anounceText != null)
                {
                    OpenText();
                    anounceText.text = anounceTextMessage;
                    Time.timeScale = 0;
                }
                firstTime = true;
            }
        }

        if (firstTime)
        {
            if (Input.GetButtonDown("Dash"))
            {
                if (anounceTextPanel != null && anounceText != null)
                {
                    CloseText();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            flagCaller.SetTrueFlag(flagCaller.flagType);
        }
    }

    private void OpenText()
    {
        if (anounceTextPanel.activeSelf == false)
        {
            Time.timeScale = 0;

            anounceTextPanel.SetActive(true);

            _characterController.enabled = false;
            _playerControl.enabled = false;
            _cameraFollow.enabled = false;

        }
    }

    private void CloseText()
    {
        if (anounceTextPanel.activeSelf == true)
        {
            Time.timeScale = 1;

            anounceTextPanel.SetActive(false);

            _characterController.enabled = true;
            _playerControl.enabled = true;
            _cameraFollow.enabled = true;

        }
    }
}
