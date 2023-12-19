using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class study3 : MonoBehaviour
{
    List<int> baseBall = new List<int>();

    List<int> inputBaseBall = new List<int>();

    int listMaxLength = 3;
    
    int attempts;

    void Start()
    {
        for(int i = 0; i < listMaxLength; i++)
        {
            baseBall.Add(Random.Range(0,10));
        }
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                inputBaseBall.Add(i);
            }
        }   

        if(Input.GetKeyDown(KeyCode.Return))
        {
            ChackBaseball();
            inputBaseBall.Clear();
        }
    }

    void ChackBaseball()
    {
        int ball = 0;
        int strike = 0;

        for(int i = 0; i < listMaxLength; i++)
        {
            if(baseBall[i] == inputBaseBall[i]) strike++;
            else if(baseBall.Contains(inputBaseBall[i])) ball++;
        }

        if(strike == listMaxLength) Debug.Log("승리!!");

        for(int i = 0; i < listMaxLength; i++)
        {
            Debug.Log($"입력한 숫자: {inputBaseBall[i]}");
        }

        Debug.Log($"Result: {strike} Strike, {ball} Ball");

        attempts++;

        Debug.Log($"시도횟수: {attempts}");

    }
}