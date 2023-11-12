using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlpha : MonoBehaviour
{
    public Renderer targetRenderer;  // カスタムマテリアルを持つオブジェクトのRenderer
    [SerializeField] public float alphaValue = 0;  // 設定したいAlpha値

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))  // 任意のキーをトリガーにする
        {
            ChangeMaterialAlpha();
        }
    }

    private void ChangeMaterialAlpha()
    {
        Material material = targetRenderer.material;
        Color color = material.color;
        color.a = alphaValue;  // Alpha値を変更
        material.color = color;
    }
}
