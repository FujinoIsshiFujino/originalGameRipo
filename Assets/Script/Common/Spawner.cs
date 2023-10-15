using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PlayerStatus _playerStatus;
    [SerializeField] private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            var distanceVctor = new Vector3(10, 0);
            // distanceVctorをランダムにy軸中心に回転させている
            var SpawnPositionFromPlyer = Quaternion.Euler(0, Random.Range(0, 360), 0) * distanceVctor;
            var SpawnPosition = _playerStatus.transform.position + SpawnPositionFromPlyer;

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(SpawnPosition, out navMeshHit, 10, NavMesh.AllAreas))
            {
                Instantiate(enemyPrefab, navMeshHit.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(10);

            if (_playerStatus.Life <= 0)
            {
                break;
            }

        }
    }

}
