
using System.Collections.Generic;
using UnityEngine;

public class TestCameraIsExited : MonoBehaviour
{
    public string enemyTag = "Enemy"; // Enemyタグを指定
    public List<GameObject> enemyList = new List<GameObject>(); // エネミーのリスト
    Vector3 viewportPosition;

    void Update()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Cameraが見つかりません。");
            return;
        }

        // enemyタグのオブジェクトの取得
        GameObject[] visibleEnemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // これはシーン上の敵を全部読みこんでしまうので、かなりメモリを食う？
        // しかし、そうじゃないとWorldToViewportPointでビューポイントで変換する対象が見つからない

        enemyList.Clear(); //クリアーをしないとlistに増え続ける

        foreach (GameObject enemyObject in visibleEnemies)
        {
            // 対象（エネミータグ）のカメラのビューポート座標を取得
            viewportPosition = mainCamera.WorldToViewportPoint(enemyObject.transform.position);

            // カメラの視錘台内にいるかどうかをチェック
            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z >= 0)
            {
                enemyList.Add(enemyObject);
            }
            else
            {
                // カメラに映っていない場合、enemyListから取り除く
                enemyList.Remove(enemyObject);
            }
        }



    }

}
