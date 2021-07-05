using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : BaseEnemy
{

    private DamagerTrigger _damager;

    protected override void Start()
    {
        _damager = GetComponentInChildren<DamagerTrigger>();
        base.Start();
        SetSpritesIndex(20);
    }

    protected override void Update()
    {
        base.Update();

        if (Base == null || Player.main == null || Player.main.Graphics == null || Disabled)
            return;

        UppdateState();
    }

    protected override void Walk(Vector2 direction)
    {
        if (State != CharacterState.BackAndForth)
        {
            transform.position = Vector2.MoveTowards(transform.position, direction, WalkSpeed * Time.deltaTime);
            Face();

            State = CharacterState.Walk;
        }
        else 
        {
            base.Walk(direction);
        }
    }

    private void UppdateState() 
    {
        if (!ActAsAEnemy)
            return;

        Vector2 dir = Vector2.right * (Player.main.Graphics.transform.localScale.x < 0 ? 1 : -1);
        Vector2 destination = (Vector2)Player.main.transform.position + dir * 2f;
        
        switch (_currentState) 
        {
            case EnemyState.Attack:
                {
                    State = CharacterState.Attack;
                    Face();

                    if(_damager != null)
                        _damager.CanDamage = true;

                    if (Vector2.Distance(destination, transform.position) > 2f)
                    {
                        _currentState = EnemyState.Chase;
                    }
                }
                break;
            case EnemyState.Chase:
                {   
                    Walk(destination);

                    if (_damager != null)
                        _damager.CanDamage = false;

                    if (Vector2.Distance(destination, transform.position) < .2f) 
                    {
                        _currentState = EnemyState.Attack;
                    }
                }
                break;
        }
    }

    private void Face() 
    {
        if (Player.main.transform.position.x >= transform.position.x)
        {
            Graphics.transform.localScale = Vector2.one * new Vector2(1, 1);
        }
        else
        {
            Graphics.transform.localScale = Vector2.one * new Vector2(-1, 1);
        }
    }
}
