using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Wood,
        Stone,
        ThroAxe
    }

    [SerializeField] private ItemType type;

    public void Initialize()
    {
        // アニメーションが終わるまでコライダーを無効化する
        var colliderCache = GetComponent<Collider>();
        colliderCache.enabled = false;

        // 出現アニメーション
        var transformCache = transform;
        var dropPosition = transform.localPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        transform.DOLocalMove(dropPosition, 0.5f); // 第二引数は秒数

        var defaultScale = transformCache.localScale;
        transformCache.localScale = Vector3.zero;
        transformCache.DOScale(defaultScale, 0.5f)
            .SetEase(Ease.OutBounce) // 振れ幅
            .OnComplete(() =>
            {
                // アニメーションが終わったらコライダーを有効化する
                colliderCache.enabled = true;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        // キャラクターそのものに接触しないと取得できない
        if (other.name != "unitychan_dynamic") return;

        // アイテムデータに追加
        OwnedItemsData.Instance.Add(type);
        OwnedItemsData.Instance.Save();

        // 所持アイテムの情報を収集
        string collectedItemsInfo = type + "を1個取得\n";

        // ItemGetAnounceTextControllerを見つけて、テキストを表示する
        ItemGetAnounceTextController textController = GameObject.Find("ItemGetAnounce").GetComponent<ItemGetAnounceTextController>();
        if (textController != null)
        {
            textController.ShowItemInfo(collectedItemsInfo);
        }
        else
        {
            Debug.LogError("ItemGetAnounceオブジェクトが見つかりませんでした。");
        }

        Destroy(gameObject);
    }
}
