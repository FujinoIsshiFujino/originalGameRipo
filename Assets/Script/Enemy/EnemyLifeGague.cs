using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyLifeGague : MonoBehaviour
{
    EnemyStatus enemyStatus;
    [SerializeField] GameObject Enemy;

    void Update()
    {
        enemyStatus = Enemy.GetComponent<EnemyStatus>();

        if (enemyStatus.Life != 0)
        {
            //　カメラと同じ向きに設定
            transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}

// public class EnemyLifeGague : MonoBehaviour
// {
//     [SerializeField] Slider healthBar;
//     private RectTransform _parentRectTransform;
//     Camera _camera;
//     MobStatus _status;



//     void Start()
//     {
//         healthBar.maxValue = _status.LifeMax;
//         healthBar.value = _status.LifeMax;
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         Refresh();
//     }

//     public void Initialize(RectTransform parentRectTransform, Camera camera, MobStatus status)
//     {
//         _parentRectTransform = parentRectTransform;
//         _camera = camera;
//         _status = status;
//         Refresh();
//     }

//     void Refresh()
//     {
//         healthBar.value = _status.Life;

//         var screenPoint = _camera.WorldToScreenPoint(_status.transform.position);
//         Vector2 localPoint;

//         RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, screenPoint, null, out localPoint);
//         transform.localPosition = localPoint + new Vector2(0, 80);
//     }
// }
//シーンに配置されているオブジェクトとuGUIオブジェクトは座標系が異なる
//オブジェクトと見かけ上同じ位置にUIを表示するためには、オブジェクトのワールド座標→スクリーン座標→TransformRectのローカル座標の順に座標変換する必要がある

//スクリーン座標 左下を(0, 0)、右上を(画面の幅, 画面の高さ)とした座標系 １ピクセルを１（単位）とするため、解像度に依存する
// _camera.WorldToScreenPointで３D座標をスクリーン座標に変換

//RectTransformUtility.ScreenPointToLocalPointInRectangle
//これは、指定されたスクリーン座標を指定されたRectTransformオブジェクトのローカル座標に変換するメソッドです。RectTransformはcanvas上でのローカル位置
//第１引数には、変換先のRectTransformローカル座標の親を指定します。
// 第２引数には、変換元のスクリーン座標を指定します。
// 第３引数には、Canvasに関連するカメラを指定します。Canvasがオーバーレイモード [1] 場合はnullを指定しなければいけません。
// 第４引数には、RectTransformのローカル座標を受け取るための変数を指定します。



//壁抜けとカメラの前後判定どうするか。まあカメラのうしろでHP表示する意味はなくてリソースの無駄。非表示にしておこう。　壁抜けはしてもいい、　きょりせいげんをつける



