using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[Serializable]
public class OwnedItemsData
{
    //PlayerPrefs を使用してデータをセーブ/ロードし、プレイヤーがゲーム内で所有するアイテムのリストを保持。
    private const string PlayerPrefsKey = "OWNED_ITEMS_DATA";

    //シングルトンパターン
    //シングルトンパターンは、特定のクラスがプログラム内で唯一のインスタンスを持つように設計 ゲーム内に複数存在してしまうと破綻するから
    //このスクリプトはアタッチしないので、アタッチによるインスタンスは作成されないので、インスタンスをここで作成している
    //  ItemsのOwnedItemsData.Instance.Add(type);でInstance プロパティを介して OwnedItemsData インスタンスを取得する際に呼ばれる処理。
    public static OwnedItemsData Instance
    {
        get
        {
            //if (null == _instance)：この行は、シングルトンインスタンス _instance がまだ作成されていないかどうかを確認しています。
            //初めて Instance プロパティにアクセスすると、_instance はまだ存在しないため、この条件が真になります
            if (null == _instance)
            {

                //HasKey メソッドは、指定されたキーが既に保存されているかどうかを確認 Saveメソッドで使用
                //trueの場合、PlayerPrefs から取得した JSON 形式のアイテムのデータを OwnedItemsData クラスに変換
                //JsonUtility.FromJson<OwnedItemsData>(...)：JsonUtility.FromJson メソッドは、JSON文字列を指定されたクラスのインスタンスに変換
                //layerPrefs からのデータが存在しない場合、新しい OwnedItemsData インスタンスが作成


                _instance = PlayerPrefs.HasKey(PlayerPrefsKey) ?
                    JsonUtility.FromJson<OwnedItemsData>(PlayerPrefs.GetString(PlayerPrefsKey)) : new OwnedItemsData();


            }
            return _instance;
        }
    }

    private static OwnedItemsData _instance;

    //プレイヤーが所有するアイテムの配列を返すプロパティ
    public OwnedItem[] OwnedItems
    {
        get { return ownedItems.ToArray(); }
    }



    //プレイヤーが所有するアイテムデータを格納するためのリスト
    [SerializeField] private List<OwnedItem> ownedItems = new List<OwnedItem>();

    private OwnedItemsData()
    {

    }

    //プレイヤーの所有アイテムデータを JSON 形式に変換し、ローカルのプレイヤープリファレンスに保存。
    public void Save()
    {
        var jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PlayerPrefsKey, jsonString);
        PlayerPrefs.Save();
    }

    public void Add(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item)
        {

            item = new OwnedItem(type);
            ownedItems.Add(item);


        }
        item.Add(number);
    }

    public void Use(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item || item.Number < number)
        {
            throw new Exception("アイテムが足りません");
        }
        item.Use(number);
    }

    public OwnedItem GetItem(Item.ItemType type)
    {
        return ownedItems.FirstOrDefault(x => x.Type == type);
    }


    //プレイヤーが所有する各アイテムのデータを格納するための内部クラスです。
    //タイプと数量の情報を保持し、Add メソッドと Use メソッドを介して数量を増減できます。
    [Serializable]
    public class OwnedItem
    {

        [SerializeField] private Item.ItemType type;
        [SerializeField] private int number;
        public Item.ItemType Type
        {
            get { return type; }
        }

        public int Number
        {
            get { return number; }
        }

        public OwnedItem(Item.ItemType type)
        {
            this.type = type;
        }

        public void Add(int number = 1)
        {
            this.number += number;
        }

        public void Use(int number)
        {
            this.number -= number;
        }
    }
}







//仮に以下の JSON 文字列が PlayerPrefs で保存されているとします：

// json

// {
//     "ownedItems": [
//         {
//             "type": "Wood",
//             "number": 10
//         },
//         {
//             "type": "Stone",
//             "number": 5
//         }
//     ]
// }
// この JSON 文字列は、OwnedItemsData クラスにデシリアライズされると(JsonUtility.FromJson<OwnedItemsData>の時)、
//次のようなデータ構造が得られます：


// OwnedItemsData.Instance.OwnedItems[0].Type // "Wood"
// OwnedItemsData.Instance.OwnedItems[0].Number // 10
// OwnedItemsData.Instance.OwnedItems[1].Type // "Stone"
// OwnedItemsData.Instance.OwnedItems[1].Number // 5
// 具体的に何が起こるかを説明すると、OwnedItemsData クラスは、ownedItems という名前のリストを持っており、
// 各要素は OwnedItem クラスのインスタンスです。JSON 文字列の中のデータは、この構造にマッピングされます。
// したがって、OwnedItemsData インスタンスの OwnedItems プロパティは、JSON 文字列内の ownedItems 配列に対応し、
// 各要素は OwnedItem インスタンスに対応します。