using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockOnCol : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    public string enemyTag = "Enemy"; // Enemyタグを指定
    public List<GameObject> enemyListResult = new List<GameObject>(); // エネミーのリスト
    public List<GameObject> nearEnemyList = new List<GameObject>();
    public bool isLockOn;
    Vector3 playerPosition;
    public bool enemyDestroy = false;

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
        if (Camera == null)
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




        enemyListResult = new List<GameObject>(enemyListResult);
        playerPosition = Player.transform.position;

        //ロックオンをした時点でのenemyListResultを取得。そうしないと視点がどんどん入れ替わってしまうため
        if (Input.GetButtonDown("Lock"))
        {
            isLockOn = !isLockOn;
            nearEnemyList = new List<GameObject>(enemyListResult);
        }

        //体力があるかつ近い順にならべ変える
        if (enemyListResult.Count > 0)
        {
            MakeEnemyDistanceList(enemyListResult);

            for (int i = 0; i < nearEnemyList.Count; i++)
            {
                //敵の体力がなくなったら、コリジョン内のリストからも消去
                var enemyStatus = nearEnemyList[i].GetComponent<EnemyStatus>();
                if (enemyStatus.Life == 0)
                {
                    nearEnemyList.RemoveAt(i);
                    enemyDestroy = !enemyDestroy;
                }

                //エラーが出るのでコメントアアウト もしかすると必要な処理かもしれないので、のこしておく
                // //もしリスト内の敵がmissingになった時の処理
                // if (nearEnemyList[i] == null)
                // {
                //     nearEnemyList.RemoveAt(i);
                //     i--; // 要素を削除したのでインデックスをデクリメント
                // }
            }
        }

        if (!isLockOn)
        {
            nearEnemyList.Clear();
        }

        // listから敵がいなくなった場合（プレイヤーの離脱、敵の撃破）
        if (enemyListResult.Count == 0)
        {
            isLockOn = false;
            nearEnemyList.Clear();
        }
    }

    //近い順（同じ距離の場合は、プレイヤーとのなす角が小さいほうが優先）にリストを並べ替える処理
    List<GameObject> MakeEnemyDistanceList(List<GameObject> enemyList)
    {
        enemyListResult.Sort((a, b) =>
        {
            if (a == null || b == null)
            {
                // どちらかが null の場合、比較できないので 0 を返す
                return 0;
            }


            // 距離と角度をかけ合わせたものに基づいてエネミーをソート　同じ距離ならプレイヤー（厳密にはカメラ）との角度を小さい方を近い方とする


            // 単純に距離だけでソートする場合
            // float distanceA = getDistance(a.transform.position, Player.transform.position);
            // float distanceB = getDistance(b.transform.position, Player.transform.position);

            // return distanceA.CompareTo(distanceB);



            // プレイヤーとエネミーの位置ベクトルを取得
            Vector3 positionA = a.transform.position;
            Vector3 positionB = b.transform.position;

            // プレイヤーからエネミーへのベクトルを計算
            Vector3 vectorA = positionA - playerPosition;
            Vector3 vectorB = positionB - playerPosition;

            // 角度を計算
            float angleA = Vector3.Angle(vectorA, Camera.transform.forward);
            float angleB = Vector3.Angle(vectorB, Camera.transform.forward);
            // Debug.Log("angleA" + angleA);

            // 距離を計算
            float distanceA = getDistance(positionA, playerPosition);
            float distanceB = getDistance(positionB, playerPosition);

            // 角度と距離を掛け合わせて比較
            float scoreA = angleA * distanceA;
            float scoreB = angleB * distanceB;

            // 昇順でソートする場合（小さい順）
            return scoreA.CompareTo(scoreB);
        });

        // 配列からnull要素を削除
        enemyList.RemoveAll(item => item == null);

        return enemyListResult;
    }

    float getDistance(Vector3 a, Vector3 b)
    {
        Vector3 dv = a - b;
        return dv.magnitude;
    }



    private void OnTriggerStay(Collider other)
    {
        //もしリスト内の敵がmissingになった時の処理
        if (enemyListResult.Count > 0)
        {
            for (int i = 0; i < enemyListResult.Count; i++)
            {
                if (enemyListResult[i] == null)
                {
                    enemyListResult.RemoveAt(i);
                    i--; // 要素を削除したのでインデックスをデクリメント
                }
            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            //カメラ内の時の処理
            // if (!enemyListTrigger.Contains(other.gameObject))
            // {

            //      enemyListTrigger.Add(other.gameObject);  // リストに追加
            // }

            //リストにいないターゲットがコリジョン内にはいってくれば配列に追加
            if (!enemyListResult.Contains(other.gameObject))
            {
                enemyListResult.Add(other.gameObject);
            }

            //敵の体力がなくなったら、コリジョン内のリストからも消去
            var enemyStatus = other.gameObject.GetComponent<EnemyStatus>();
            if (enemyStatus.Life == 0)
            {
                enemyListResult.Remove(other.gameObject);
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


    //インスペクターでenemyListResult（やそれを参照しているものも？）を見ながら敵を撃破した時などに以下のようなエラーが出ることがある
    // InvalidOperationException: The operation is not possible when moved past all properties (Next returned false)
    // UnityEditor.SerializedProperty.get_objectReferenceInstanceIDValue () (at <53ddbed73faf4fe3b980a493ab4e6639>:0)
    // UnityEditor.EditorGUIUtility.ObjectContent (UnityEngine.Object obj, System.Type type, UnityEditor.SerializedProperty property, UnityEditor.EditorGUI+ObjectFieldValidator validator) (at <53ddbed73faf4fe3b980a493ab4e6639>:0)
    // UnityEditor.UIElements.ObjectField+ObjectFieldDisplay.Update () (at <53ddbed73faf4fe3b980a493ab4e6639>:0)
    // UnityEditor.UIElements.ObjectField.UpdateDisplay () (at <53ddbed73faf4fe3b980a493ab4e6639>:0)
    // UnityEngine.UIElements.VisualElement+SimpleScheduledItem.PerformTimerUpdate (UnityEngine.UIElements.TimerState state) (at <215a3d79f22d4716a26783e9b63e881e>:0)
    // UnityEngine.UIElements.TimerEventScheduler.UpdateScheduledEvents () (at <215a3d79f22d4716a26783e9b63e881e>:0)
    // UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.UpdateSchedulers () (at <215a3d79f22d4716a26783e9b63e881e>:0)
    // UnityEngine.UIElements.UIEventRegistration.UpdateSchedulers () (at <215a3d79f22d4716a26783e9b63e881e>:0)
    // UnityEditor.RetainedMode.UpdateSchedulers () (at <1087a632947b4cacb7629f01f1937a19>:0)
}
