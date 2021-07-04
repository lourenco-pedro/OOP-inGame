using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{

    public enum CharacterState { Idle, Walk, Attack, Hurt, Chase }
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
        Initialize();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update() 
    {
    }

    protected virtual void Initialize() 
    {

        name += $"_{Base.Name}";
        
        _graphics = Instantiate(Base.Graphics, transform);
        _graphics.transform.localPosition = Vector2.zero;
        OnStateChanged += _graphics.OnStateChanged;

        Stop();

        Life = Base.Life;
        Stregth = Base.Strength;
        WalkSpeed = Base.WalkSpeed;
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
}
