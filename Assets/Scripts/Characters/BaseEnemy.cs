using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Character
{

    protected enum EnemyState { Chase, Attack }
    [SerializeField]
    protected EnemyState _currentState;

    public bool ActAsAEnemy;

    protected override void Update()
    {
        base.Update();
    }
}
