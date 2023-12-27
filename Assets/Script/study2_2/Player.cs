using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigidbody;
    public GameObject bomb;
    public Transform target;

    float speed = 10.0f;
    float runSpeed = 30.0f;
    float detectionRadius = 10.0f;
    int nowHasBombIndex;

    [HideInInspector] public bool hasBomb = false;
    bool isNearbyPlayer;
    public bool isPassBomb = false;

    void Awake() 
    {
        rigidbody = GetComponent<Rigidbody>();
        if(gameObject.transform.childCount != 0) bomb = gameObject.transform.Find("Bomb").gameObject;
    }

    void Update()
    {
        MoveAutomatically();
        Run(GetTarget());
        CheckNearbyPlayer();
        Away();
    }

    void MoveAutomatically()
    {
        Vector3 force = Vector3.zero;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player != this.gameObject && player.GetComponent<Player>().hasBomb)
            {
                Vector3 directionToAvoid = transform.position - player.transform.position;

                force = directionToAvoid.normalized * speed;
                rigidbody.AddForce(force, ForceMode.VelocityChange);

            }
        }


        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        force = randomDirection.normalized * speed;

        rigidbody.velocity = force;
    }

    Transform GetTarget()
    {
        if(hasBomb == false) return null;
        if(target != null) return target;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i< players.Length; i++)
        {
            if(players[i].GetComponent<Player>().hasBomb == true)
            {
                nowHasBombIndex = i;
            }
        }
        for(int i = nowHasBombIndex; i < players.Length -1; i++)
        {
            players[i] = players[i+1];
        }

        Array.Resize(ref players, players.Length -1);

        target = players[UnityEngine.Random.Range(0,players.Length)].transform;

        return target;
    }
    
    void CheckNearbyPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        
        List<Collider> validColliders = colliders.ToList();

        validColliders.RemoveAll(collider => collider.gameObject.name == this.gameObject.name);

        isNearbyPlayer = validColliders.Any(collider => collider.CompareTag("Player"));
    }

    void Run(Transform target)
    {
        if(hasBomb == true && target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            Vector3 force = directionToTarget.normalized * runSpeed;
            rigidbody.velocity = force;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Away()
    {
        if(hasBomb) return;
        if (isNearbyPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Vector3 awayDirection = Vector3.zero;
            foreach (var player in players)
            {
                if (player != this.gameObject)
                {
                    Vector3 directionToPlayer = transform.position - player.transform.position;
                    awayDirection += directionToPlayer.normalized * detectionRadius;
                }
            }
            rigidbody.velocity = awayDirection.normalized * runSpeed;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Player") && hasBomb == true && isPassBomb == true)
        {
            Player otherPlayer = other.gameObject.GetComponent<Player>();

            bomb.transform.parent = other.transform;

            bomb.transform.localPosition = new Vector3(0,2,0);

            hasBomb = false;

            otherPlayer.hasBomb = true;
            otherPlayer.bomb = other.gameObject.transform.Find("Bomb").gameObject;

            bomb = null;

            target = null;

            isPassBomb = false;

            otherPlayer.bomb.GetComponent<Bomb>().ChangeHasBombPlayer();
            
            StartCoroutine(DelayTime(otherPlayer));
        }
    }

    IEnumerator DelayTime(Player player)
    {
        yield return new WaitForSeconds(3.0f);
        player.target = player.GetTarget();
        player.isPassBomb = true;
    }
}