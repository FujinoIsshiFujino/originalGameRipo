using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipeGet : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // 追加するプレハブ
    [SerializeField] private Transform content;
    [SerializeField] private Recipe recipe;

    /// <summary>
    /// Content 内にプレハブを追加する
    /// </summary>
    public void AddPrefabToScrollView()
    {
        if (prefab == null || content == null)
        {
            Debug.LogWarning("Prefab または Content が設定されていません。");
            return;
        }

        GameObject newItem = Instantiate(prefab, content);
        newItem.transform.localScale = Vector3.one; // スケールをリセット（UIのスケーリングを維持）

        if (recipe != null)
        {
            recipe.UpdateMenuButtons();
        }
    }
}
