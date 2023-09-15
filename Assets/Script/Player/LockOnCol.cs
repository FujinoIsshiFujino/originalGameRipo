using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCol : MonoBehaviour
{
    public bool isLockOn;

    //  public GameObject[] enemyList;  //エネミーの配列
    public List<GameObject> enemyList;  // エネミーのリスト
    [SerializeField] GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        isLockOn = false;
        enemyList = new List<GameObject>();  // リストを初期化
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isLockOn = true;
            // enemyList = other.gameObject;
            if (!enemyList.Contains(other.gameObject))
            {
                enemyList.Add(other.gameObject);  // リストに追加
            }

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isLockOn = false;
            enemyList.Remove(other.gameObject);  // リストから削除
        }
    }
}
