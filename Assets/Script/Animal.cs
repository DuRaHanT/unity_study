using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{  
    string Name;
    int age;

    public virtual void Speak()
    {
        Debug.Log("I don't know what to say");
    }
}