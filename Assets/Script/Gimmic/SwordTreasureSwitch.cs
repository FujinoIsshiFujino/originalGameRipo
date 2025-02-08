using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTreasureSwitch : MonoBehaviour
{
    [SerializeField] GameObject sword;

    public void SwordTreasureSwitchFunction()
    {
        sword.SetActive(true);
    }
}
