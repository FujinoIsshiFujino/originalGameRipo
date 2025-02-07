using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureController : FlagSwitch
{
    [SerializeField] GameObject OpenTreasure;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("Attack"))
            {
                flagCaller.SetTrueFlag(flagCaller.flagType);

                // オブジェクトの入れ替え
                this.gameObject.SetActive(false);
                OpenTreasure.SetActive(true);
            }
        }
    }
}
