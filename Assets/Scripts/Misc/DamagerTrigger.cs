using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null) 
        {
            return;
        }

        character.ChangeLife(-1);
    }
}
