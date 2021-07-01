using UnityEngine;
using System;
using System.Collections;

public class Character : MonoBehaviour
{

    public enum CharacterState { Idle, Walk, Attack, Hurt }
    public CharacterState State 
    {
        get { return _state; }
        set 
        {
            _state = value;
            OnStateChanged?.Invoke(_state);
        }
    }

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
        StartCoroutine(EndHurt());
    }

    private IEnumerator EndHurt()
    {
        yield return new WaitForSeconds(.5f);
        State = CharacterState.Idle;
    }
}
