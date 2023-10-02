
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{


    [SerializeField] private LayerMask raycastLayerMask;
    // private RaycastHit[] raycasthit = new RaycastHit[10];
    private RaycastHit raycasthit;

    private NavMeshAgent _agent;
    private EnemyStatus _status;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _status = GetComponent<EnemyStatus>();

    }

    public void OnDetectObject(Collider collider)
    {
        // if (collider.gameObject.tag == "Player")
        // {
        //     //障害物がプレイヤーとの間にあれば止まる
        //     Vector3 diff = collider.transform.position - transform.position;
        //     var directinon = diff.normalized;
        //     var distance = diff.magnitude;
        //     var hitCount = Physics.RaycastNonAlloc(transform.position + new Vector3(0, 0.5f, 0), directinon, raycasthit, distance);

        //     Debug.Log("hitCount" + hitCount);

        //     foreach (RaycastHit hitobj in raycasthit)
        //     {
        //         Debug.Log("raycasthit" + hitobj.collider.gameObject);
        //         if (hitobj.collider.gameObject.tag == "Player")
        //         {

        //             _agent.isStopped = false;
        //             _agent.destination = collider.transform.position;

        //         }
        //         else
        //         {

        //             _agent.isStopped = true;
        //         }
        //     }



        //     Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), directinon, Color.red, 1, false);
        // }







        // if (collider.CompareTag("Player"))
        // {
        //     GameObject Target = collider.gameObject;

        //     var diff = Target.transform.position - transform.position;
        //     var distance = diff.magnitude;
        //     var direction = diff.normalized;

        //     if (Physics.Raycast(transform.position, direction, out raycasthit, distance))
        //     {
        //         Debug.Log("Target" + Target);
        //         Debug.Log("raycasthit" + raycasthit.transform.gameObject);
        //         Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), direction, Color.red, 1, false);
        //         if (raycasthit.collider.gameObject.tag == "Player")
        //         {
        //             Debug.Log("ターゲット");
        //             _agent.isStopped = false;
        //             _agent.destination = Target.transform.position;
        //         }
        //         else
        //         {
        //             _agent.isStopped = true;
        //         }

        //     }

        // }



        //レイが当たった一番近いものがプレイヤーでなかった場合（障害物の場合）、敵は動きをとめる。
        //現状の実装はアイテムとの角度計算するために、プレイヤーに球のコライダー使っている。なので、レイはそれにあたる
        //今回はレイヤーマスクでレイは障害物と球コライダーのみ当たるようにして、一番近いものがプレイヤーレイヤーだったら追うし、それ以外のデフォルトレイヤー（今後追加はありうる）だったら追わない
        //アイテムの球のコライダーをプレイヤーじゃなくて、アイテムにつけるときはプレイヤーにコライダーつければいい。
        //キャラコンはコライダーではないので、いちいちコライダーをつけたり、プレイヤーについているコライダーをいじったりしている



        if (collider.gameObject.tag == "Player")
        {
            if (!_status.IsMovable)
            {
                _agent.isStopped = true;
                return;
            }

            var diff = collider.gameObject.transform.position - transform.position;
            var distance = diff.magnitude;
            var direction = diff.normalized;

            if (Physics.Raycast(transform.position, direction, out raycasthit, distance, raycastLayerMask))
            {
                // Debug.Log("Target" + collider.gameObject);
                // Debug.Log("raycasthit" + raycasthit.transform.gameObject);
                // Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), direction, Color.red, 1, false);
                if (raycasthit.collider.gameObject.tag == "Player")
                {

                    _agent.isStopped = false;
                    _agent.destination = collider.gameObject.transform.position;
                }
                else
                {
                    _agent.isStopped = true;
                }

            }
        }

    }
}
