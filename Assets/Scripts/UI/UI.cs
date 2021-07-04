using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    private static UI Instance;

    [SerializeField]
    private DamageLabel _labelPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void AddDamageInternal(Vector2 position, string lbl)
    {
        DamageLabel dmgLabel = Instantiate(_labelPrefab, transform);
        dmgLabel.Initialize(lbl);
        dmgLabel.transform.localScale = Vector3.one;
        dmgLabel.transform.position = position;
    }

    public static void AddDamageLabel(Vector2 position, string lbl) 
    {
        Instance.AddDamageInternal(position, lbl);
    }

    public static Vector2 ToScreenPosition(Vector2 worldPosition) 
    {
        return Camera.main.WorldToScreenPoint(worldPosition);
    }
}
