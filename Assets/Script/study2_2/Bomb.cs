using System;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Player player;
    public TextMeshProUGUI timerView;
    public GameObject currentBombPlayer;
    public GameObject previousBombPlayer;


    void Awake()
    {
        player = gameObject.transform.parent.GetComponent<Player>();
    }

    void Start()
    {
        player.hasBomb = true;
        player.isPassBomb = true;
        currentBombPlayer = transform.parent.gameObject;
    }



    public void ChangeHasBombPlayer()
    {
        if(transform.parent != null)
        {
            previousBombPlayer = currentBombPlayer;
            currentBombPlayer = transform.parent.gameObject;
        }
    }

    void Winner()
    {
        Debug.Log($"{previousBombPlayer} is winner!");
    }

    void Loser()
    {
        Debug.Log($"{currentBombPlayer} is winner!");
    }
}
