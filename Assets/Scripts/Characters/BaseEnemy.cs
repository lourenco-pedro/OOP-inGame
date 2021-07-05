using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Character
{

    protected enum EnemyState { Chase, Attack }
    [SerializeField]
    protected EnemyState _currentState;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            _currentState = EnemyState.Chase;
        }
    }
}
