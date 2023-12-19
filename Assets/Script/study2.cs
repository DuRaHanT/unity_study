using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class study2 : MonoBehaviour
{
    string[] characters = { "a", "b", "c", "d", "e" };

    int currentBombCharacterIndex;

    int previousBombCharacterIndex;

    float BombTimer = 5;


    void Start()
    {
        StartCoroutine(PassBomb());
    }

    void Update()
    {
        BombTimer -= Time.deltaTime;

        if(BombTimer <= 0)
        {
            Debug.Log($"Character {characters[currentBombCharacterIndex]} 폭사!!!");

            Debug.Log($"Character {characters[previousBombCharacterIndex]} 승리!!!");

            Time.timeScale = 0;
        }
    }

    void PassBombToRandomCharacter()
    {
        previousBombCharacterIndex = currentBombCharacterIndex;

        while(currentBombCharacterIndex == previousBombCharacterIndex)
        {
            currentBombCharacterIndex = Random.Range(0, characters.Length);
        }

        Debug.Log($"Character {characters[previousBombCharacterIndex]}에서 Character {characters[currentBombCharacterIndex]}로 전달");
    }

    IEnumerator PassBomb()
    {
        yield return new WaitForSeconds(0.3f);

        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));

            PassBombToRandomCharacter();

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ReStart()
    {
        BombTimer = 5;
        Time.timeScale = 1;
        StartCoroutine(PassBomb());
    }
}