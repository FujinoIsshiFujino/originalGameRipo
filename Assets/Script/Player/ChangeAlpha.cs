using UnityEngine;

public class ChangeAlpha : MonoBehaviour
{
    public Renderer targetRenderer;  // カスタムマテリアルを持つオブジェクトのRenderer
    [SerializeField] public float alphaValue = 0.5f;  // 設定したいAlpha値
    [SerializeField] public float redValie = 1;
    [SerializeField] public float buleValue = 1;
    Material originalMaterial;
    BridgeMove bridgeMove;
    CubeMove cubeMove;
    ObjMove objMove;

    private void Start()
    {
        // オリジナルのマテリアルを保存
        originalMaterial = new Material(targetRenderer.material);

        // Material はクラスであり、C# ではクラスのインスタンスを変数に代入した場合、その変数はそのクラスのインスタンスへの参照となります。
        // そのため、originalMaterial = targetRenderer.material;（クラスのインスタンス）とstart関数内で定義しても逐次的に変更されてしまう、なのでnewを使ってインスタンス生成。

    }

    private void Update()
    {
        ChangeMaterialAlpha();
    }

    private void ChangeMaterialAlpha()
    {
        objMove = GetComponent<ObjMove>();

        if (targetRenderer != null)
        {
            if (objMove.isSetable)
            {
                Material material = targetRenderer.material;
                Color color = new Color(0, 0, buleValue, alphaValue);
                material.color = color;

                targetRenderer.material = material; // オブジェクトに新しいマテリアルを設定

                if (Input.GetButtonDown("Dash"))
                {
                    targetRenderer.material = originalMaterial;
                    Destroy(this);
                }
            }
            else
            {
                Material material = targetRenderer.material;
                Color color = new Color(redValie, 0, 0, alphaValue);
                material.color = color;

                targetRenderer.material = material; // オブジェクトに新しいマテリアルを設定
            }
        }
    }
}
