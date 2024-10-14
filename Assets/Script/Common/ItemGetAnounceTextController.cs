using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemGetAnounceTextController : MonoBehaviour
{
    private Text itemGetAnounceText;
    private Image panelBackground;

    private void Start()
    {
        // Textコンポーネントを取得して、最初は非表示にする
        itemGetAnounceText = GetComponentInChildren<Text>();
        itemGetAnounceText.enabled = false;

        panelBackground = GetComponentInParent<Image>();

        if (panelBackground != null)
        {
            // 背景色を透明に設定
            panelBackground.color = new Color(1, 1, 1, 0); // 完全な透明
        }
    }

    // アイテム取得時に呼び出されるメソッド
    public void ShowItemInfo(string collectedItemsInfo)
    {
        // テキストを更新し、表示する
        itemGetAnounceText.text = collectedItemsInfo;
        itemGetAnounceText.enabled = true;

        if (panelBackground != null)
        {
            panelBackground.color = new Color(1, 1, 1, 0.1f); // 白色、透明度0.3
        }

        // 3秒後にテキストを非表示にするコルーチンを呼び出す
        StartCoroutine(HideTextAfterDelay(3f));
    }

    // 指定時間後にTextを非表示にするコルーチン
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        itemGetAnounceText.enabled = false;

        if (panelBackground != null)
        {
            // 背景色を透明に設定
            panelBackground.color = new Color(1, 1, 1, 0); // 完全な透明
        }
    }
}