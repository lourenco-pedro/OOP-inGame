using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public static List<Character> AllCharacters = new List<Character>();

    public enum CharacterState { Idle, Walk, Attack, Hurt, Chase }

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

    protected virtual void Awake() 
    {
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update() 
    {
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

    public virtual void AddDamage() 
    {
        Graphics.SetColorMultiplier(Color.white);
        StopCoroutine(IEDamage());
        StartCoroutine(IEDamage());
    }

    private IEnumerator IEDamage() 
    {
        Graphics.SetColorMultiplier(Color.red);
        yield return new WaitForSeconds(.1f);
        Graphics.SetColorMultiplier(Color.white);
    }

    public static void Spawn(CharacterTemplate character, Vector2 position) 
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
            return;
        }
        else if (character == enemy2Template.Base) 
        {
            Character spawned2 = Instantiate(enemy2Template, position, Quaternion.identity);
            spawned2.Base = character;
            spawned2.Disabled = false;
            spawned2.Initialize();
            AllCharacters.Add(spawned2);
            return;
        }

        Character spawned = Instantiate(playerTemplate, position, Quaternion.identity);
        spawned.Base = character;
        spawned.Disabled = false;
        spawned.Initialize();
        AllCharacters.Add(spawned);
    }

    public static void ClearAll() 
    {
        foreach (Character character in AllCharacters) 
        {
            GameObject.Destroy(character.gameObject);
        }

        AllCharacters.Clear();
    }
}
