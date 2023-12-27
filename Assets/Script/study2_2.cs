using UnityEngine;

public class study2_2 : MonoBehaviour
{
    GameObject[] players;
    GameObject bomb;

    float timer = 30.0f;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        bomb = GameObject.FindWithTag("Bomb");

        for(int i = 0; i <players.Length; i++)
        {
            players[i].transform.localPosition = new Vector3(Random.Range(-4.5f,4.5f), players[i].transform.localPosition.y, Random.Range(-4.5f,4.5f));
        }
    }

    void Start()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i].AddComponent<Player>();
        }

        bomb.AddComponent<Bomb>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
           // Stop();
        }
    }

    void Stop()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().enabled = false;
        }
    }
}