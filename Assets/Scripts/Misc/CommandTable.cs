using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTable : MonoBehaviour
{

    public CharacterTemplate PlayerTemplate;
    public CharacterTemplate Enemy1Template;
    public CharacterTemplate Enemy2Template;

    private void Update()
    {
        AddCharacter();
    }

    private void AddCharacter() 
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 spawnPos = UI.ToWorldPosition(Input.mousePosition);
            spawnPos.y = -4.95f;

            if (Input.GetKey(KeyCode.Alpha0))
            {
                Character.Spawn(PlayerTemplate, spawnPos);
            }
            else if (Input.GetKey(KeyCode.Alpha1))
            {
                Character.Spawn(Enemy1Template, spawnPos);
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                Character.Spawn(Enemy2Template, spawnPos);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C)) 
        {
            Character.ClearAll();
        }
    }
}
