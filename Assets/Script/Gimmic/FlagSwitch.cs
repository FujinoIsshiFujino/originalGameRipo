using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagSwitch : MonoBehaviour
{
    public FlagCaller flagCaller;
    public bool firstTime;
    [SerializeField] public GameObject Player;
    public PlayerControl _playerControl;
    public CharacterController _characterController;

    public virtual void Start()
    {
        firstTime = false;

        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();
    }
    // Update is called once per frame
    public virtual void Update()
    {
        flagCaller = GetComponent<FlagCaller>();
    }
}


