using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGraphics : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    public void OnStateChanged(Character.CharacterState state)
    {
        for (int i = 0; i <= (int)Character.CharacterState.Hurt; i++) 
        {
            Character.CharacterState paramName = (Character.CharacterState)i;
            _animator.SetBool(paramName.ToString(), paramName == state);
        }

        if (state == Character.CharacterState.BackAndForth) 
        {
            _animator.SetBool("Walk", true);
        }
    }

    public void SetColorMultiplier(Color color) 
    {
        foreach (SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>()) 
        {
            render.color = color;
        }
    }
}
