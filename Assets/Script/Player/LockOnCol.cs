using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCol : MonoBehaviour
{
    public string enemyTag = "Enemy"; // Enemyタグを指定

    public List<GameObject> enemyListResult = new List<GameObject>(); // エネミーのリスト

    //カメラ内の時の処理
    // public List<GameObject> enemyList = new List<GameObject>(); // エネミーのリスト
    // public List<GameObject> enemyListTrigger = new List<GameObject>(); // エネミーのリスト
    //  Vector3 viewportPosition;
    // [SerializeField] GameObject Player;

    //     //カメラ内の時の処理
    // void Start()
    // {
    //     // enemyList = new List<GameObject>();  // リストを初期化
    // }

    void Update()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Cameraが見つかりません。");
            return;
        }


        //カメラ内にうつっている条件も加えるとき

        // // enemyタグのオブジェクトの取得
        // GameObject[] visibleEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // // これはシーン上の敵を全部読みこんでしまうので、かなりメモリを食う？
        // // しかし、そうじゃないとWorldToViewportPointでビューポイントで変換する対象が見つからない

        // enemyList.Clear(); //クリアーをしないとlistに増え続ける

        // foreach (GameObject enemyObject in visibleEnemies)
        // {
        //     // 対象（エネミータグ）のカメラのビューポート座標を取得
        //     viewportPosition = mainCamera.WorldToViewportPoint(enemyObject.transform.position);

        //     // カメラの視錘台内にいるかどうかをチェック
        //     if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
        //         viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
        //         viewportPosition.z >= 0)
        //     {

        //         enemyList.Add(enemyObject);
        //     }
        //     else
        //     {
        //         // カメラに映っていない場合、enemyListから取り除く
        //         enemyList.Remove(enemyObject);
        //     }
        // }

        // // 更新時に一致する要素を更新
        // UpdateEnemyListResult();

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            //カメラ内の時の処理
            // if (!enemyListTrigger.Contains(other.gameObject))
            // {

            //      enemyListTrigger.Add(other.gameObject);  // リストに追加
            // }

            //違うターゲットがコリジョン内に存在すれば配列に追加
            if (!enemyListResult.Contains(other.gameObject))
            {
                enemyListResult.Add(other.gameObject);
            }

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //カメラ内の時の処理
            // enemyListTrigger.Remove(other.gameObject);  // リストから削除
            enemyListResult.Remove(other.gameObject);
        }
    }

    //カメラ内の時の処理
    // enemyListとenemyListTriggerの一致する要素をenemyListResultに格納
    // private void UpdateEnemyListResult()
    // {
    //     enemyListResult.Clear();

    //     foreach (GameObject enemy in enemyList)
    //     {
    //         if (enemyListTrigger.Contains(enemy))
    //         {
    //             enemyListResult.Add(enemy);
    //         }
    //     }
    // }



}
