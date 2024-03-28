using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAllDeath : FlagSwitch
{
    [SerializeField] Text anounceText;
    [SerializeField] GameObject anounceTextPanel;
    [SerializeField] string anounceTextMessage;
    public bool Exection;
    public Vector3 boxsize;

    public override void Start()
    {
        base.Start();
        boxsize = GetComponent<BoxCollider>().size;
        anounceTextPanel.SetActive(false);
    }


    public override void Update()
    {
        base.Update();

        if (Physics.CheckBox(transform.position, boxsize * 0.52f, Quaternion.identity, LayerMask.GetMask("Enemy")))
        {
            Exection = false;
        }
        else
        {
            Exection = true;
            flagCaller.SetTrueFlag(flagCaller.flagType);
        }

        if (firstTime == false)
        {
            if (flagCaller.isOn)
            {
                OpenText();
                anounceText.text = anounceTextMessage;
                firstTime = true;
                Time.timeScale = 0;
            }
        }

        if (firstTime)
        {
            if (Input.GetButtonDown("Dash"))
            {
                CloseText();
            }
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

        }
    }
}
