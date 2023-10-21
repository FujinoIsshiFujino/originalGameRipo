using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialog : MonoBehaviour
{
    [SerializeField] private int buttonNumber = 15;
    [SerializeField] private ItemButton itemButton;

    private ItemButton[] _itemButtons;

    void Start()
    {
        gameObject.SetActive(false);

        //アイテム欄をbuttonNumberの分だけ必要な文だけ生成
        for (var i = 0; i < buttonNumber - 1; i++)
        {
            Instantiate(itemButton, transform);
        }

        _itemButtons = GetComponentsInChildren<ItemButton>();
    }

    public void Toggle()
    {
        //自分自身のアクティブをボタンが押されたときに切り替える
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {

            // ダイアログ内のアイテムボタンの数（buttonNumber）よりもアイテムデータの数が少ない場合、残りのアイテムボタンは空白として表示
            for (var i = 0; i < buttonNumber; i++)
            {
                //ItemButtonのインスタンス作成して、その中のOwnedItemをいじくっているので、setの内容がはしる
                _itemButtons[i].OwnedItem = OwnedItemsData.Instance.OwnedItems.Length > i ?
                    OwnedItemsData.Instance.OwnedItems[i] : null;
            }
        }
    }


}
