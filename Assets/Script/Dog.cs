using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal, IMovable
{
    void Start()
    {
        Speak();
        Move();
    }

    public void Move()
    {
        Debug.Log("Running fast!");
    }

    public override void Speak()
    {
        base.Speak();
        Debug.Log("Woof");
    }
}