using System.Collections.Generic;

public static class RecipeData
{
    // HashSet→重複したオブジェクトを追加できないリストクラス
    private static HashSet<MakeButtoon.makeItemType> unlockedRecipes
        = new HashSet<MakeButtoon.makeItemType>();

    public static void Unlock(MakeButtoon.makeItemType type)
    {
        unlockedRecipes.Add(type);
    }

    //Containsメソッドで指定した要素があるかを確認する
    public static bool IsUnlocked(MakeButtoon.makeItemType type)
    {
        return unlockedRecipes.Contains(type);
    }
}