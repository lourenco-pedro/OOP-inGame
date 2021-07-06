using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public static List<Character> AllCharacters = new List<Character>();
    public static Player SpawnedPlayer;
    public static Enemy1 SpawnedEnemy1;
    public static Enemy2 SpawnedEnemy2;

    public enum CharacterState { Idle, Walk, Attack, Hurt, Chase, BackAndForth }

    public bool Disabled;
    
    public CharacterState State 
    {
        get { return _state; }
        set 
        {
            if (_state == value) 
            {
                return;
            }

            _state = value;
            OnStateChanged?.Invoke(_state);
        }
    }

    public CharacterGraphics Graphics => _graphics;
    public CharacterTemplate Base;
    public Action<CharacterState> OnStateChanged;

    public float Life;
    public float Stregth;
    public float WalkSpeed;

    [SerializeField]
    private CharacterState _state;
    private CharacterGraphics _graphics;

    private Vector2 _initialPos;
    private float _backAndForth_walkDuration = 1;
    private float _backAndForth_currentWalkDuration = 0;
    private bool _backAndForth_walkRight;

    protected virtual void Awake() 
    {
    }

    protected virtual void Start()
    {
        _initialPos = transform.position;
    }

    protected virtual void Update() 
    {
        if (_state == CharacterState.BackAndForth) 
        {
            UpdateBackAndForth();
        }
    }

    protected virtual void Initialize()
    {
        if (Base == null || Disabled)
        {
            return;
        }

        if (_graphics != null) 
        {
            GameObject.Destroy(_graphics.gameObject);
        }

        Debug.Log($"Initialize: {Base.Name}");

        _graphics = Instantiate(Base.Graphics, transform);
        _graphics.transform.localPosition = Vector2.zero;
        OnStateChanged += _graphics.OnStateChanged;

        Stop();

        Life = Base.Life;
        Stregth = Base.Strength;
        WalkSpeed = Base.WalkSpeed;

        if (Disabled)
        {
            WalkSpeed = 0;
        }
        else
        {
            name = $"Character_{Base.Name}";
        }
    }

    protected virtual void Walk(Vector2 direction) 
    {
        transform.Translate(direction * WalkSpeed * Time.deltaTime);

        if (direction == Vector2.left)
        {
            _graphics.transform.localScale = Vector2.one * new Vector2(-1, 1);
        }
        else 
        {
            _graphics.transform.localScale = Vector2.one;
        }

        if(_state != CharacterState.BackAndForth)
            State = CharacterState.Walk;
    }

    protected virtual void Stop() 
    {
        State = CharacterState.Idle;
    }

    public void ChangeLife(float value) 
    {
        State = CharacterState.Hurt;
        Life += value;
        if (value < 0) 
        {
            AddDamage();
            UI.AddDamageLabel(UI.ToScreenPosition((Vector2)transform.position + Vector2.up), value.ToString());
        }
        StartCoroutine(EndHurt());
    }
    public virtual void AddDamage() 
    {
        Graphics.SetColorMultiplier(Color.white);
        StopCoroutine(IEDamage());
        StartCoroutine(IEDamage());
    }

    protected void SetSpritesIndex(int offset)
    {
        SpriteRenderer[] renders = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer render in renders) 
        {
            render.sortingOrder += offset;
        }
    }

    private IEnumerator EndHurt()
    {
        yield return new WaitForSeconds(.5f);
        State = CharacterState.Idle;
    }

    private IEnumerator IEDamage() 
    {
        Graphics.SetColorMultiplier(Color.red);
        yield return new WaitForSeconds(.1f);
        Graphics.SetColorMultiplier(Color.white);
    }

    private void UpdateBackAndForth() 
    {
        if (_backAndForth_currentWalkDuration < _backAndForth_walkDuration)
        {
            _backAndForth_currentWalkDuration += 1 * Time.deltaTime;

            if (_backAndForth_walkRight)
            {
                Walk(Vector2.right);
            }
            else
            {
                Walk(Vector2.right * -1);
            }
        }
        else 
        {
            _backAndForth_walkRight = !_backAndForth_walkRight;
            _backAndForth_currentWalkDuration = 0;
        }
    }

    public static T Spawn<T>(CharacterTemplate character, Vector2 position)
        where T : Character
    {
        Player playerTemplate = GameObject.Find("Character_PlayerTemplate").GetComponent<Player>();
        Enemy1 enemy1Template = GameObject.Find("Character_Enemy1Template").GetComponent<Enemy1>();
        Enemy2 enemy2Template = GameObject.Find("Character_Enemy2Template").GetComponent<Enemy2>();

        if (character == enemy1Template.Base) 
        {
            Character spawned1 = Instantiate(enemy1Template, position, Quaternion.identity);
            spawned1.Disabled = false;
            spawned1.Initialize();
            spawned1.Base = character;
            AllCharacters.Add(spawned1);
            return spawned1 as T;
        }
        else if (character == enemy2Template.Base) 
        {
            Character spawned2 = Instantiate(enemy2Template, position, Quaternion.identity);
            spawned2.Base = character;
            spawned2.Disabled = false;
            spawned2.Initialize();
            AllCharacters.Add(spawned2);
            return spawned2 as T;
        }

        Character spawned = Instantiate(playerTemplate, position, Quaternion.identity);
        spawned.Base = character;
        spawned.Disabled = false;
        spawned.Initialize();
        AllCharacters.Add(spawned);
        return spawned as T;
    }

    public static void ClearAll() 
    {
        foreach (Character character in AllCharacters) 
        {
            GameObject.Destroy(character.gameObject);
        }

        AllCharacters.Clear();

        SpawnedPlayer = null;
        SpawnedEnemy1 = null;
        SpawnedEnemy2 = null;
    }

    public static void OnSlideChanged(int slide) 
    {

        ClearAll();
        
        if (slide == 12) 
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
        }
        if (slide == 13) 
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedPlayer.ChangeLife(-1);
        }
        if (slide == 14) 
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedPlayer.State = CharacterState.BackAndForth;
        }
        if (slide == 15)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedPlayer.State = CharacterState.Attack;
        }
        if (slide == 16)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
        }
        if (slide == 17)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));
        }
        if (slide == 18)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));

            SpawnedEnemy1.ChangeLife(-1);
            SpawnedEnemy2.ChangeLife(-1);
        }
        if (slide == 19)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));

            SpawnedEnemy1.State = CharacterState.BackAndForth;
            SpawnedEnemy2.State = CharacterState.BackAndForth;
        }
        if (slide == 20)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));

            SpawnedEnemy1.State = CharacterState.Attack;
            SpawnedEnemy2.State = CharacterState.Attack;
        }
        if (slide == 21)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));
        }
        if (slide == 25)
        {
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
        }
        if (slide == 26)
        {
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1.ActAsAEnemy = true;
        }
        if (slide == 28)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            //SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));
        }
        if (slide == 29)
        {
            SpawnedPlayer = Spawn<Player>(CommandTable.main.PlayerTemplate, UI.ToWorldPosition(new Vector2(Screen.width / 2, 0)));
            SpawnedEnemy1 = Spawn<Enemy1>(CommandTable.main.Enemy1Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 - (Screen.width / 4), 0)));
            SpawnedEnemy2 = Spawn<Enemy2>(CommandTable.main.Enemy2Template, UI.ToWorldPosition(new Vector2(Screen.width / 2 + (Screen.width / 4), 0)));
        }
    }
}
