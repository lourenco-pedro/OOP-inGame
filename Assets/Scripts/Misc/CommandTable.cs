using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTable : MonoBehaviour
{

    public static CommandTable main;

    public CharacterTemplate PlayerTemplate;
    public CharacterTemplate Enemy1Template;
    public CharacterTemplate Enemy2Template;

    private void Awake()
    {
        main = this;
    }

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
                Character.Spawn<Player>(PlayerTemplate, spawnPos);
            }
            else if (Input.GetKey(KeyCode.Alpha1))
            {
                Enemy1 _spawnedEnemy = Character.Spawn<Enemy1>(Enemy1Template, spawnPos);
                _spawnedEnemy.ActAsAEnemy = true;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                Enemy2 _spawnedEnemy = Character.Spawn<Enemy2>(Enemy2Template, spawnPos);
                _spawnedEnemy.ActAsAEnemy = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Character.ClearAll();
        }
        else if (Input.GetKeyDown(KeyCode.O)) 
        {
            Character.Spawn<Player>(PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width + Screen.width / 2, 0)));
        }
    }
}
