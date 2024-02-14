using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//検証１
//検証２
//検証3
//検証4
//検証5
//藤野６
//検証藤野７
//検証２ブランチの検証８
//検証９


// sortの降順をsort を使わないで表現。foreachで繰り返しで大小比べて、一番大きい値を元の配列から削除して・・を繰り返す

// using System.Collections.Generic;
// using UnityEngine;

// public class DescendingSort : MonoBehaviour
// {
//     void Start()
//     {
//         List<int> numbers = new List<int>() { 5, 2, 9, 1, 3 };
//         List<int> sortedList = new List<int>();

//         while (numbers.Count > 0)
//         {
//             int max = FindMaxValue(numbers);
//             sortedList.Add(max);
//             numbers.Remove(max);
//         }

//         foreach (int number in sortedList)
//         {
//             Debug.Log(number);
//         }
//     }

//     int FindMaxValue(List<int> list)
//     {
//         int max = list[0];
//         foreach (int num in list)
//         {
//             if (num > max)
//             {
//                 max = num;
//             }
//         }
//         return max;
//     }
// }





// カメラ追従２

// 頭いいベクトル逆方向型
//public Transform target; // 追尾対象のプレイヤーのTransform
//public float followDistance = 5f; // カメラの追尾距離

//void LateUpdate()
//{
//    if (target == null)
//    {
//        Debug.LogWarning("カメラの追尾対象が設定されていません。");
//        return;
//    }

//    // 追尾対象の後ろに追従する位置を計算
//    // target.forwardの逆をベクトル計算することで、常にカメラを後ろにすることに成功
//    Vector3 behindPosition = target.position - target.forward * followDistance;
//    behindPosition.y = 1;

//    // カメラの位置を更新
//    transform.position = behindPosition;
//   // transform.LookAt(target); // カメラがプレイヤーを常に見つめるようにする
//}




// 初期位置から距離を計算して、カメラの現在のポジションにその距離を足して一定の距離を守るタイプ

//public Transform target; // 追尾対象のプレイヤーのTransform
//public float distanceFromPlayer = 5f; // カメラの初期位置からの距離
//public float followSpeed = 5f; // カメラの追尾速度

//private Vector3 initialOffset; // カメラの初期位置とプレイヤーの位置のオフセット

//void Start()
//{


//    // カメラの初期位置とプレイヤーの位置のオフセットを計算
//    initialOffset = transform.position - target.position;
//}

//void LateUpdate()
//{


//    // カメラの追尾対象の位置を計算
//    Vector3 targetPosition = target.position + initialOffset.normalized * distanceFromPlayer;

//    // 追尾速度を考慮してカメラの位置を更新
//    Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
//    transform.position = newPosition;
//}







//ラムダ式:　匿名関数　リアクトのノリでアロー演算子使えはする。ただし、関数の時のみで、変数の場合は引数がいらないので（）の記述もなくなる。
//右辺を評価して左辺に格納するっていう根本思想は同じ

// Func<bool> IsMovableJudge = () => StateEnum.Normal == _state; は、ラムダ式を変数 IsMovableJudge に代入しています。
// これは、条件を評価するための関数を変数に格納する方法です。必要に応じて、IsMovableJudge 関数を呼び出して条件を評価できます。
// このアプローチは、条件を何度も再利用する必要がある場合に役立ちます。


// public bool IsMovable => StateEnum.Normal == _state; は、プロパティとして条件を定義しています。
// このプロパティは、外部から参照する際に通常のプロパティとしてアクセスできます。
// プロパティのゲッターとしてラムダ式を使用しているため、IsMovable プロパティの値は _state の値に基づいて動的に変化します。
// プロパティの主な目的は、外部コードから内部状態にアクセスする際にカプセル化と制御を提供することです。



// get setについて
// public class Player
// {
//     private string _name;

//     public string Name
//     {
//         get { return _name; }
//         set
//         {
//             _name = value; // value は新しい値を指す
//         }
//     }

//     public Player(string initialName)
//     {
//         _name = initialName;
//     }
// }


// Player player = new Player("Alice");
// Console.WriteLine(player.Name); // "Alice"
//コンストラクタてきな関数をつかっているので、そりゃそうなる。

// player.Name = "Bob"; // `set` アクセサ内で `value` を使用
// Console.WriteLine(player.Name); // "Bob"
//こっちはnewでコンストラクタつかわなくてもPlayer クラスのインスタンスを作成し、Name プロパティに値を設定すると変えることができる
//set アクセサを使うことで、新しい値に対して独自の検証ロジックや条件を設けることができます。
//たとえば、値が特定の条件を満たしている場合のみ設定を許可するなど、値を制御できます。
//例えば以下のコード

// private int _score;

// public int Score
// {
//     get { return _score; }
//     set
//     {
//         if (value >= 0 && value <= 100) // 0から100の間に制約を設ける
//         {
//             _score = value;
//         }
//         else
//         {
//             throw new ArgumentOutOfRangeException("Score must be between 0 and 100.");
//         }
//     }
// }




//ライフゲージの複数管理　没

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// // [RequireComponent(typeof(RectTransform))]
// public class LifeGaugeContainer : MonoBehaviour
// {
// public static LifeGaugeContainer Instance
// {
//     get { return _instance; }
// }

// private static LifeGaugeContainer _instance;

// [SerializeField] private Camera mainCamera;
// [SerializeField] private EnemyLifeGague enemyLifeGague;

// private RectTransform rectTransform;
// private readonly Dictionary<MobStatus, EnemyLifeGague> _statusLifeBarMap = new Dictionary<MobStatus, EnemyLifeGague>();
// //ステータスとライフゲージの結び付け

// private void Awake()
// {
//     if (null != _instance) throw new Exception("LifeBarContainer instance aldeady exists.");
//     _instance = this;
//     rectTransform = GetComponent<RectTransform>();
// }

// public void Add(MobStatus status)
// {
//     var lifeGauge = Instantiate(enemyLifeGague, transform);
//     lifeGauge.Initialize(rectTransform, mainCamera, status);
//     _statusLifeBarMap.Add(status, lifeGauge);
// }

// public void Remove(MobStatus status)
// {
//     Destroy(_statusLifeBarMap[status].gameObject);
//     _statusLifeBarMap.Remove(status);
// }


// }


