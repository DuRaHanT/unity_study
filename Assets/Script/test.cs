using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // 시작하면서 랜덤한 위치로 이동
        StartCoroutine(MoveRandomly());
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // 랜덤한 위치 얻기
            Vector3 randomPoint = GetRandomPointOnNavMesh();
            
            // 랜덤한 위치로 이동
            navMeshAgent.SetDestination(randomPoint);

            // 이동 완료 대기
            yield return new WaitUntil(() => navMeshAgent.remainingDistance < 0.1f);

            // 다음 이동을 위해 잠시 대기
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 randomPoint;

        do
        {
            randomPoint = new Vector3(
                Random.Range(-10f, 10f),
                0f,
                Random.Range(-10f, 10f)
            );
        } while (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas));

        return hit.position;
    }
}