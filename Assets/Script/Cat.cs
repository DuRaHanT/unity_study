using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Animal
{
    void Start()
    {
        Speak();    
    }

    public override void Speak()
    {
        base.Speak();
        Debug.Log("Meow");
    }
}