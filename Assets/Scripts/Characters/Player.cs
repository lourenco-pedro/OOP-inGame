using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player main;

    protected override void Awake()
    {
        base.Awake();

        if (main != null)
        {
            main = this;
        }
        main = this;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (Base == null)
            return;

        if (Input.GetKey(KeyCode.D))
        {
            Walk(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Walk(-Vector2.right);
        }
        else if(State != CharacterState.BackAndForth && State != CharacterState.Attack)
        {
            Stop();
        }
    }
}
