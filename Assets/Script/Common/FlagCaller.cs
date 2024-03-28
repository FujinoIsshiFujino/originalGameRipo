using UnityEngine;
using System.Collections;
using System;
public class FlagCaller : MonoBehaviour
{
    private FlagManager flagManager;
    [SerializeField] GameObject flagManagerObj;
    [SerializeField] public FlagManager.Flag flagType;
    public bool isOn;

    void Start()
    {
        flagManager = flagManagerObj.GetComponent<FlagManager>();
        InitFlag(flagType);
    }

    void Update()
    {
        isOn = (bool)flagManager.flagDictionary[flagType];
    }

    public void InitFlag(FlagManager.Flag flagType)
    {
        flagManager.flagDictionary[flagType] = false;
    }

    public void SetTrueFlag(FlagManager.Flag flagType)
    {
        flagManager.flagDictionary[flagType] = true;
    }

    public void SetFalseFlag(FlagManager.Flag flagType)
    {
        flagManager.flagDictionary[flagType] = true;
    }
}