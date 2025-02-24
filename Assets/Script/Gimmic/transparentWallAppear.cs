using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWallAppear : MonoBehaviour
{
    [SerializeField] private GameObject walls;
    [SerializeField] private EnemyAllDeath enemyAllDeath;
    [SerializeField] private float delayTime = 1f;

    private bool isProcessing = false; // 一度だけ実行するためのフラグ

    private void Update()
    {
        if (enemyAllDeath.Exection && !isProcessing)
        {
            isProcessing = true; // 処理開始フラグ
            StartCoroutine(DisableCollidersWithDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            walls.SetActive(true);

            // walls の子オブジェクトにある全ての BoxCollider の isTrigger をオフにする
            BoxCollider[] childColliders = walls.GetComponentsInChildren<BoxCollider>();
            if (childColliders.Length > 0)
            {
                foreach (BoxCollider collider in childColliders)
                {
                    collider.isTrigger = false;
                }
            }
            else
            {
                Debug.LogError("⚠️ walls の子オブジェクトに BoxCollider が見つかりません！");
            }
        }
    }

    private IEnumerator DisableCollidersWithDelay()
    {
        yield return new WaitForSeconds(delayTime); // 指定した秒数待機
        walls.SetActive(false); // 子オブジェクトの BoxCollider を無効化
    }
}
