using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreasureController : FlagSwitch
{
    [SerializeField] GameObject OpenTreasure;
    [SerializeField] private UnityEvent onTreasureOpened = new UnityEvent();

    private void OnTriggerStay(Collider other)
    {
        if (flagCaller.isOn == false)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (Input.GetButtonDown("Attack"))
                {
                    flagCaller.SetTrueFlag(flagCaller.flagType);

                    // オブジェクトの入れ替え
                    this.gameObject.SetActive(false);
                    OpenTreasure.SetActive(true);

                    // 設定されているイベントがあれば実行する
                    onTreasureOpened?.Invoke();
                }
            }
        }
    }
}
