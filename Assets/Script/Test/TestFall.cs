using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFall : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public CharacterController characterController;

    public string groundTag = "ground";
    [SerializeField] float raycastDistance = 0.5f;

    FadeController fadeController;
    [SerializeField] GameObject fadePanel;
    float WaitTime = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = Player.GetComponent<CharacterController>();
        fadeController = fadePanel.GetComponent<FadeController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {


            // if (hit.gameObject.tag == "Hole")
            // {
            Debug.Log("unko");

            GetRespawnObjectPosition();
            // characterController.enabled = true;

            fadeController.isFadeOut = true;//フェードアウト


            //同じフレームでフェードのOut/Inを行うと止まるのでコルーチンで時間をずらす。
            StartCoroutine(WarpFadeIn());
            // }
        }
    }

    public Transform player; // プレイヤーのTransform
    public int numRays = 10; // レイの本数
    public float radius = 20f; // 円の半径
    public LayerMask layerMask; // レイヤーマスク

    // 関数として呼び出すためのメソッド
    public Vector3 GetRespawnObjectPosition()
    {
        // プレイヤーの位置を中心として円状にレイを飛ばす
        for (int i = 0; i < numRays; i++)
        {
            // 角度を計算
            float angle = i * 360f / numRays;
            // 極座標からワールド座標へ変換
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            // レイを飛ばす
            RaycastHit hit;
            // if (Physics.Raycast(Player.transform.position, direction, out hit, radius))
            if (Physics.Raycast(Player.transform.position, direction, out hit, radius, layerMask))

            {
                Debug.DrawLine(Player.transform.position, hit.point, Color.red); // レイをデバッグ表示

                // レイが何かに当たった場合の処理
                // 当たったオブジェクトがRespawnタグを持つ場合の処理
                if (hit.collider.CompareTag("Respawn"))
                {
                    Debug.Log(hit.collider.transform.position + "ffff");
                    characterController.enabled = false;
                    // RespawnPlayer();
                    Player.transform.position = hit.collider.transform.position + new Vector3(0, 30, 0);

                    characterController.enabled = true;
                }
            }
            else
            {
                // レイが何にも当たらなかった場合は円の外周をデバッグ表示
                Vector3 endPoint = Player.transform.position + direction * radius;
                Debug.DrawLine(Player.transform.position, endPoint, Color.green);
            }
        }

        // レイが当たるRespawnタグを持つオブジェクトが見つからない場合は、Vector3.zero を返す
        return Vector3.zero;
    }

    private IEnumerator WarpFadeIn()
    {
        yield return new WaitForSeconds(WaitTime);
        // transform.position = WarpOutPosi; //プレイヤーの座標変更
        fadeController.isFadeIn = true;//フェードイン
        characterController.enabled = true;//プレイヤーが操作可能に
    }
}
