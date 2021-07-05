using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerTrigger : MonoBehaviour
{

    public bool CanDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player character = other.GetComponent<Player>();
        if (!CanDamage || character == null) 
        {
            return;
        }

        character.ChangeLife(-1);
    }
}
