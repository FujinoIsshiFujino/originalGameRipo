using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagSwitch : MonoBehaviour
{
    public FlagCaller flagCaller;
    public bool firstTime;
    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject Camera;
    public PlayerControl _playerControl;
    public CharacterController _characterController;
    public CameraFollow _cameraFollow;

    public virtual void Start()
    {
        firstTime = false;

        _playerControl = Player.GetComponent<PlayerControl>();
        _characterController = Player.GetComponent<CharacterController>();
        _cameraFollow = Camera.GetComponent<CameraFollow>();
    }
    // Update is called once per frame
    public virtual void Update()
    {
        flagCaller = GetComponent<FlagCaller>();
    }
}


