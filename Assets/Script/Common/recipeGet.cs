using UnityEngine;

public class recipeGet : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform content;
    [SerializeField] private Recipe recipe;

    // このスクリプトがアタッチされるレシピが何なのかを示す
    [SerializeField] private MakeButtoon.makeItemType recipeType;

    /// <summary>
    /// レシピを取得したときの処理
    /// </summary>
    public void GetRecipe()
    {
        // すでに取得済みなら何もしない
        if (RecipeData.IsUnlocked(recipeType))
        {
            return;
        }

        //以下レシピを新規取得した時の処理
        // レシピを取得済みにする
        RecipeData.Unlock(recipeType);

        // ButtonをContentに追加
        AddPrefabToScrollView();

        // レシピオブジェクトを消す
        Destroy(gameObject);
    }

    /// <summary>
    /// Content内にレシピButtonを追加する
    /// </summary>
    private void AddPrefabToScrollView()
    {
        if (prefab == null || content == null)
        {
            Debug.LogWarning("Prefab または Content が設定されていません。");
            return;
        }

        GameObject newItem = Instantiate(prefab, content);

        // RecipeのmenuButtonsを更新
        if (recipe != null)
        {
            recipe.UpdateMenuButtons();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤーがレシピに接触しました！");
            GetRecipe();
        }
    }
}