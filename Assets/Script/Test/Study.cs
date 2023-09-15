using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    // enum 文字型リテラル敵なのの型をつくる。
    // list　は後から追加できる配列　ロックオンの時の敵の取得とか動的に変わりうるものにたいしてGOOD

    Vector3 positoion = new Vector3(4, 4, 4);
    // 数字だけでは型がベクターではないので、当然不可能
    // new　Vector3　で初期化すひつようがあるぽい・

    // int[] arry = new int[] { 1, 2, 3 };

    //foreach (int i in arry)
    //{
    //    Debug.Log("I is :"+i);
    //}




    //// Mathf.Clampの自作
    //public static float Clamp(float value, float min, float max)
    //{
    //    // valueが範囲内に収まるように制限する
    //    if (value < min)
    //    {
    //        return min;

    //    }
    //    else if (value > max)
    //    {
    //        return max;
    //    }
    //    else
    //    {
    //        return value;
    //    }







    //}
    void Start()
    {
        // transform.position = new Vector3(4, 4, 4);
        // 位置かえる


    }

    // Update is called once per frame
    void Update()
    {
        // .transform.foroward  transformっていうクラスの中の.forowardにアクセスしてる時点で向き

        // 右にうごく。.positionにアクセスしてるので。
        // this.gameObject.transform.position += (Vector3.right)/60;

        transform.Translate(0, 0, Time.deltaTime);
    }
}




// v=u+atの等加速度運動をつかったジャンプの式
//public class TestJump : MonoBehaviour
//{

//    public float jumpForce = 10f; // ジャンプ力
//    public float gravity = 9.81f; // 重力加速度
//    public bool canJump = true; // ジャンプを許可するかどうかのフラグ
//    private float verticalVelocity = 0f;

//    private void Update()
//    {
//        if (canJump && Input.GetKeyDown(KeyCode.Space))
//        {
//            Jump();
//        }

//        ApplyGravity();
//    }

//    private void Jump()
//    {
//        verticalVelocity = Mathf.Sqrt(2f * jumpForce * gravity);
//    }

//    private void ApplyGravity()
//    {
//        verticalVelocity -= gravity * Time.deltaTime;

//        // 地面に接触した場合の処理
//        if (transform.position.y <= 0f && verticalVelocity < 0f)
//        {
//            verticalVelocity = 0f;
//        }

//        Vector3 displacement = new Vector3(0f, verticalVelocity * Time.deltaTime, 0f);
//        transform.position += displacement;
//    }
//}







//// 等加速つかたジャンプ

//// 加速度[m/s^2]
//// ※ただし、この加速度は常に一定であるという前提
//[SerializeField] private float jumpForce;

//// 初速度[m/s]
//[SerializeField] private Vector3 _initialVelocity;

//// 初期位置[m]
//private Vector3 _startPosition;

//// 時刻[s]
//private float _time;

//private bool isJump;

//private int jumpCount;


//private void Update()
//{

//    if (Input.GetKeyDown((KeyCode.Space)) && jumpCount < 1)
//    {
//        isJump = true;
//        _startPosition = transform.position;    // 2段 防がなければ
//        jumpCount++;

//    }

//    if (isJump)
//    {

//        // 時刻更新
//        _time += Time.deltaTime;

//        // 現在位置を計算して反映する
//        transform.position =
//            _startPosition +
//            _initialVelocity * _time +
//            0.5f * Physics.gravity * _time * _time;

//        // こっちのほうが着実
//        //float yposi = _startPosition.y + jumpForce * _time + 0.5f * Physics.gravity.y * _time * _time;
//        //transform.position = new Vector3(transform.position.x, yposi, transform.position.z);



//    }

//    Debug.Log(_time);

//}

//private void OnCollisionEnter(Collision collision)
//{
//    Debug.Log("着地");
//    isJump = false;
//    _time = 0;
//    jumpCount = 0;
//}

//重力加速度は９．８で一定だが、速度が一定というわけではない。
//そもそも加速度っていうのは単位時間あたりに変化する。
//つまり重力加速度は１秒後は９．８だけど、２秒後には１９．６で、変化の割合が一定なだけ
//なので加速度を計算するときにvelocity * Time.deltaTimeみたいにやるとそれはその１フレームの一瞬だけの速度を計算しているだけ。
//通常の速度計算で速度が一定ならそれでいいかもしれないけど、加速度というのは時間とともに大きくなっていく。
//なので、加速度は足し合わせて大きくしていくイメージ
//velocity.y += Physics.gravity.y * Time.deltaTime;
//つまりこんな感じ



// ジャンプについて　キャラコンを使う場合は、地面にいないときは重力に時間をかけたものに１フレーム（デルタタイム）をかける　重力 * time * Time.deltaTime
//ジャンプしたときには、初速に1フレームの進む距離を書ける　つまり　 jumpForce *Time.deltaTime;　　結局基本は速度はvo+atで出してる

//上のだと、jumpForceが最高点の高さと同じ意味になるはず。　ただ、これは重力加速度９．８に従った場合だけの話。ゲームなので、プレイヤーだけ物理挙動にしたがわなくても別にいいはず。
//つまり、上野に加えて、最高点に達するまでの時間も指定できればよりゲームっぽい。カービーみたいな緩いジャンプかメタルマリオみたいに超加速するかとかそういう話。

//jumpPower = apex/apexTime + 0.5*9.81*apexTime;で等加速の公式応用すれば初速は出せるが、これだと最高点にapexTimeでたどり着ける初速は算出してくれるが、最高点で止まってくれるとは限らない。
//上のがうまくいかない理由は、初速がどれだけ変わっても重力が一定なので、下向きの力がかわらないので、初速が早いほど最高点は高くなるから。
//なので、最高点とそれに達するまでの時間を任意で決められるとすると、重力と初速度が毎回変わる必要がある。
//重力の計算　h = 1/2at^2 を変形　（等加速運動の公式）　virtualGra= 2*apex / Mathf.Pow(apexTime, 2)*-1;　これを地面にいないときにvirtualGra * time*Time.deltaTime
// 初速度の計算　vo = at　jumpPower = virtualGra * apexTime*-1;　 これをジャンプ押した時に、jumpPower*Time.deltaTime


// Lerpは多分内分、t,1-tに分けてる。tの範囲を０～１にしてる。

// void Update()
// {
//     // 補間変数を更新
//     t += lerpSpeed * Time.deltaTime;
//     t = Mathf.Clamp01(t); // 0から1の範囲にクランプ

//     // 開始位置から終了位置への補間を計算
//     Vector3 lerpedPosition = startTransform.position * (1f - t) + endTransform.position * t;

//     // オブジェクトの位置を更新
//     transform.position = lerpedPosition;

//     // 補間が終了したかどうかを確認
//     if (t >= 1f)
//     {
//         Debug.Log("補間終了");
//         enabled = false; // 補間が終了したらこのスクリプトを無効化
//     }
// }

//AI説明
//  位置ベクトル: 通常、3D空間内で物体の位置は位置ベクトルとして表現されます。これらのベクトルは、通常、3つの成分（x、y、z座標）で構成されます。

// 初期位置と最終位置: 線形補間では、物体の初期位置（始点）と最終位置（終点）が与えられます。これらは2つのベクトルとして表現されます。

// 時間の経過: 補間の進行を制御するために、0から1の間の値で表される「時間」または「進行度」が使用されます。時間が0のとき、物体は初期位置にあり、時間が1のとき、物体は最終位置にあります。
//時間の値が0から1まで徐々に増加することで、物体は初期位置から最終位置に向かって移動します。

// 位置の計算: 時間が進行するにつれて、初期位置と最終位置の間の位置を計算します。これは簡単な線形計算で行われます。
//たとえば、時間が0.5の場合、物体は初期位置と最終位置の中間にあります。したがって、初期位置と最終位置を補間する際に、
//初期位置から最終位置に向かうベクトルを途中で止めることなく移動することになります。

// 補間の滑らかさ: タイムステップ（通常はフレームごとに進む時間の小さな単位）を使用して、滑らかな移動を実現します。
//時間が徐々に増加し、物体が初期位置から最終位置に移動するたびに、新しい位置が計算され、物体は滑らかに動きます。

//AIになぜ目的地手前で遅くなるのか聞いてみた。
// public class CustomLerpSimulation : MonoBehaviour
// {
//     public Transform target;  // 移動先の目標位置
//     public float speed = 2.0f;  // 移動速度

//     private Vector3 initialPosition;  // 開始位置
//     private float distanceToTarget;  // 目的地までの距離
//     private float journeyLength;  // 移動距離

//     private void Start()
//     {
//         initialPosition = transform.position;
//         distanceToTarget = Vector3.Distance(initialPosition, target.position);
//         journeyLength = distanceToTarget;
//     }

//     private void Update()
//     {
//         if (distanceToTarget > 0)
//         {
//             float distanceCovered = Mathf.Min(speed * Time.deltaTime, distanceToTarget);
//             float journeyFraction = distanceCovered / journeyLength;

//             // 目的地に向かって移動
//             transform.position += (target.position - transform.position).normalized * distanceCovered;

//             distanceToTarget -= distanceCovered;
//         }
//     }
// }

// distanceToTargetは毎フレーム目的理までの残り距離が更新されていく。
// float distanceCovered = Mathf.Min(speed * Time.deltaTime, distanceToTarget);　で小さいほうがdistanceToTargetからひかれていくので。
//             // 目的地に向かって移動
//             transform.position += (target.position - transform.position).normalized * distanceCovered;
// ここの行で、現在のフレームの残りの距離のノーマライズしたもの（つまり方向のみの働き）を係数倍、ここではどんどん小さくなるdistanceCoveredを毎フレームかけているので、どんどん遅くなる。
// つまりlerpは内分の計算を用いて、毎フレームの軌道を計算（毎フレームの座標がその直線を表すのは媒介変数表示みたいな感覚。ちょっとちがうか。）して、
// どんどん小さくなる値を毎回かけているから、目的地手前でおそくなるということ。











//  Vector3.MoveTowardsの説明
//  割とそのまんまで、目的地から現在位置を引いて方向ベクトル取得→正規化
//  それにスピードをかけたものを毎フレーム足していく。元の方向ベクトルは毎フレーム正規化されるのそれに毎フレーム一定のスピードをかけているので、
//  だんだん早くなることはない。
//上のはAI出力だけど、下のコードでもやってる子と同じなのでいける。

// public Transform target; // 移動の目標位置
// public float speed = 5.0f; // 移動速度

// void Update()
// {
//     // 現在位置から目標位置へのベクトルを計算
//     Vector3 direction = target.position - transform.position;

//     // ベクトルの長さが1以上の場合に移動を行う
//     if (direction.magnitude > 1e-5)
//     {
//         // ベクトルを正規化して移動方向を取得
//         Vector3 moveDirection = direction.normalized;

//         // 移動方向に速度を掛けて新しい位置を計算
//         Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

//         // 新しい位置に移動
//         transform.position = newPosition;
//     }
// }



// public class TestEnemyMove : MonoBehaviour
// {
//     public Transform startPoint;  // 開始位置
//     public Transform endPoint;    // 終了位置
//     public float speed = 2.0f;    // 移動速度

//     private Vector3 nextPosition;


//     void Start()
//     {
//         transform.position = startPoint.position;
//         nextPosition = endPoint.position;
//     }

//     void Update()
//     {
//         float step = speed * Time.deltaTime;


//             Vector3 progressVec = (nextPosition-transform.position).normalized;

//             transform.position += progressVec*step;


//     }
// }







// Vector3.Distanceは２点間のベクトル成分を引き算して求めて、それを２乗（マグニチュード）で求めてるだけ




// たまたまみつけたカメラに映った特定タグのものを消す処理

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// public class TestCameraIsExited : MonoBehaviour
// {
//     public string enemyTag = "Enemy"; // Enemyタグを指定
//     private GameObject[] enemyObjects;

//     void Start()
//     {
//         enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);
//     }

//     void Update()
//     {
//         Camera mainCamera = Camera.main;

//         if (mainCamera == null)
//         {
//             Debug.LogError("Main Cameraが見つかりません。");
//             return;
//         }

//         foreach (GameObject enemyObject in enemyObjects)
//         {
//             // カメラのビューポート座標を取得
//             Vector3 viewportPosition = mainCamera.WorldToViewportPoint(enemyObject.transform.position);

//             // カメラの視錘台内にいない場合、enemyObjectsから削除
//             if (!(viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
//                   viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
//                   viewportPosition.z >= 0))
//             {
//                 // enemyObjectをenemyObjectsから削除
//                 RemoveEnemy(enemyObject);
//             }
//         }
//     }

//     // enemyObjectsからオブジェクトを削除する関数
//     void RemoveEnemy(GameObject enemyToRemove)
//     {
//         for (int i = 0; i < enemyObjects.Length; i++)
//         {
//             if (enemyObjects[i] == enemyToRemove)
//             {
//                 // オブジェクトを削除
//                 Destroy(enemyObjects[i]);

//                 // enemyObjectsからも削除
//                 enemyObjects[i] = null;
//                 enemyObjects = RemoveNullObjects(enemyObjects);
//                 break;
//             }
//         }
//     }

//     // null要素を削除した配列を返す関数
//     GameObject[] RemoveNullObjects(GameObject[] array)
//     {
//         return array.Where(x => x != null).ToArray();
//     }
// }






// ワールド座標をビューポート値に変換している
// Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);