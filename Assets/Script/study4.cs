using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class study4 : MonoBehaviour
{
    int baseNumber;

    List<int> inputNumber = new List<int>();

    void Start() => baseNumber = Random.Range(0, 101);

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                inputNumber.Add(i);
            }
        }  

        if(Input.GetKeyDown(KeyCode.Return))
        {
            CheckNumber();
            inputNumber.Clear();
        }
    }

    int SumNumber()
    {
        inputNumber.Reverse();
        int result = 0; 
        for(int i = 0; i < inputNumber.Count; i++)
        {
            result += inputNumber[i] * (int)Mathf.Pow(10, i);
        }

        Debug.Log(result);
        return result;
    }

    void CheckNumber()
    {
        int result = SumNumber();

        if(baseNumber == result) Debug.Log("승리!!!!");
        else if(baseNumber > result) Debug.Log("업!!!!");
        else Debug.Log("다운!!!!");
    }
}