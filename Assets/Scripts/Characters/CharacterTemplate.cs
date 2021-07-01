using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game/character", fileName = "character")]
public class CharacterTemplate : ScriptableObject
{
    public CharacterGraphics Graphics;
    public string Name;
    public float Life;
    public float Strength;
    public float WalkSpeed;
}
