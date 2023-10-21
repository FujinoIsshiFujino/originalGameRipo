using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System;
using UnityEngine.Video;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    public OwnedItemsData.OwnedItem OwnedItem
    {
        get { return _ownedItem; }
        set
        {
            //、渡された新しい value（プレイヤーのアイテムデータ）を _ownedItem に設定し、アイテムボタンをアップデート
            _ownedItem = value;

            //アイテムが所有されていない場合、アイテムボタンのイメージと数量のテキストが非表示になり、ボタンが非アクティブに設定されます。
            //アイテムが所有されている場合、アイテムボタンのイメージと数量のテキストが表示され、ボタンがアクティブに設定されます。
            //また、アイテムボタンのイメージは、対応するアイテムタイプに関連づけられたスプライトで設定されます
            var isEmpty = null == _ownedItem;
            image.gameObject.SetActive(!isEmpty);
            number.gameObject.SetActive(!isEmpty);
            _button.interactable = !isEmpty;//ユーザーがボタンをクリックできるかどうか
            if (!isEmpty)
            {
                image.sprite = itemSprites.First(x => x.itemType == _ownedItem.Type).sprite;
                number.text = "" + _ownedItem.Number;
            }
        }
    }

    [SerializeField] private ItemTypeSpriteMap[] itemSprites;
    [SerializeField] private Image image;
    [SerializeField] private Text number;

    private Button _button;
    private OwnedItemsData.OwnedItem _ownedItem;


    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {

    }

    [Serializable]
    public class ItemTypeSpriteMap
    {
        public Item.ItemType itemType;
        public Sprite sprite;
    }
}
