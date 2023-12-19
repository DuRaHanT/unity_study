using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class study2_1 : MonoBehaviour
{
    public GameObject[] players;
    public GameObject bomb;

    int currentBombCharacterIndex;

    int previousBombCharacterIndex;

    float bombTimer = 15;

    float duration;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        bomb = GameObject.FindWithTag("Bomb");
    }

    void Start() => StartCoroutine(BombTimerCoroutine());

    IEnumerator BombTimerCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        while (bombTimer > 0)
        {
            PassBomb();
            yield return new WaitForSeconds(duration);

            bombTimer -= Time.time;
        }
        Debug.Log($"Character {players[currentBombCharacterIndex]} 폭사!!!");

        Debug.Log($"Character {players[previousBombCharacterIndex]} 승리!!!");
    }

    void PassBomb()
    {
        while(currentBombCharacterIndex == previousBombCharacterIndex)
        {
            currentBombCharacterIndex = UnityEngine.Random.Range(0, players.Length);
        }       

        Vector3 targetPosition = players[currentBombCharacterIndex].transform.position;
        StartCoroutine(MoveBombCoroutine(targetPosition));

        Debug.Log($"Character {players[previousBombCharacterIndex]}에서 Character {players[currentBombCharacterIndex]}로 전달");

        previousBombCharacterIndex = currentBombCharacterIndex;
    }
    
    IEnumerator MoveBombCoroutine(Vector3 targetPosition)
    {
        float elapsedTime = 0.0f;
        duration = UnityEngine.Random.Range(0.1f, 2.0f);

        Vector3 startPosition = bomb.transform.position;
        targetPosition.y = bomb.transform.position.y;

        while (elapsedTime < duration)
        {
            bomb.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bomb.transform.position = targetPosition;
    }
}
