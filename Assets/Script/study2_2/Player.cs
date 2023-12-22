using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public enum PlayerState {None = -1, Idle = 0, Move = 1, Run = 2, Pass = 3, Away = 4}

public class Player : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    PlayerState playerState = PlayerState.None;

    float passLimitRange = 5.0f;
    float targetRecognitionRange = 10.0f;
    float playersRecognitionRange = 20.0f;
    float speed = 10.0f;

    [HideInInspector] public bool isBomb = false;
    bool isPlayer;
    Transform target;

    void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();

    void Start()
    {
        GameObject test = GameObject.Find("Player 2");
        target = test.transform;

        ChangeState(PlayerState.Move);
    }

    void Update() => ShootRayInCircle();

    IEnumerator Idle()
    {
        float chageTime = Random.Range(0.1f, 0.5f);

        yield return new WaitForSeconds(chageTime);

        ChangeState(PlayerState.Move);
    }


    void ChangeState(PlayerState newstate)
    {
        if(playerState == newstate) return;

        StopCoroutine(newstate.ToString());

        playerState = newstate;

        StartCoroutine(newstate.ToString());
    }

    void ShootRayInCircle()
    {
        Vector3 rayOrigin = transform.position;

        float angle = Random.Range(0f, 360f);

        Vector3 rayDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));

        Ray ray = new Ray(rayOrigin, rayDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playersRecognitionRange * 1.5f))
        {
            isPlayer = true;

            if(isBomb == true)
            {
                if (hit.distance < passLimitRange)
                {
                    ChangeState(PlayerState.Pass);
                }
                else if (hit.distance < targetRecognitionRange)
                {
                    ChangeState(PlayerState.Run);
                }
            }
            if(hit.distance <= playersRecognitionRange) 
            {
                ChangeState(PlayerState.Away);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playersRecognitionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, passLimitRange);
    }

    IEnumerator Move()
    {
        if (!isBomb)
        {
            float currentTime = 0.0f;
            float maxTime = 10.0f;

            navMeshAgent.speed = speed;

            navMeshAgent.SetDestination(CalculateMovePosition());
            
            Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);            
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
            transform.rotation = Quaternion.LookRotation(to - from);

            while(currentTime < maxTime)
            {
                if(isPlayer == false)
                {
                    currentTime += Time.deltaTime;

                    if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
                    {
                        ChangeState(PlayerState.Idle);
                    }
                } 
                else if(isPlayer == true)
                {
                    currentTime = maxTime;
                    ChangeState(PlayerState.Idle);
                }
                yield return null;
            }
        }
    }

    Vector3 CalculateMovePosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderjitterMin = 0;
        int wanderjitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        wanderJitter = Random.Range(wanderjitterMin, wanderjitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    IEnumerator Run()
    {
        if(isBomb == true)
        {
            navMeshAgent.speed = speed * 1.5f;

            navMeshAgent.SetDestination(target.position);

            LookRotationToTarget();

            yield return null;
        }
    }

    void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 form = new Vector3(transform.position.x, 0, transform.position.z);

        transform.rotation = Quaternion.LookRotation(to - form);
    }

    Transform Target()
    {
        GameObject[] randomTarget = GameObject.FindGameObjectsWithTag("Player").Where(go => go != gameObject).ToArray();

        Transform tragetPosition = randomTarget[Random.Range(0,randomTarget.Length)].transform;

        target = tragetPosition;

        return target;
    }

    IEnumerator Pass()
    {
        navMeshAgent.ResetPath();

        isBomb = false;

        yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
    }

    IEnumerator Away()
    {
        float currentTime = 0.0f;
        float maxTime = 5.0f;

        navMeshAgent.speed = speed;

        navMeshAgent.SetDestination(CalculateMovePosition());
        
        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);            
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while(true)
        {
            currentTime += Time.deltaTime;

            if((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                ChangeState(PlayerState.Idle);
                isPlayer = false;
            }

            yield return null;
        }
    }
}
