using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player main;

    protected override void Start()
    {
        base.Start();

        if (main == null) 
        {
            main = this;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.D))
        {
            Walk(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Walk(-Vector2.right);
        }
        else 
        {
            Stop();
        }
    }
}
