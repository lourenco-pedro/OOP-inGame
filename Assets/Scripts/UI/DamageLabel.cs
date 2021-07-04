using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageLabel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    public void Initialize(string text) 
    {
        _label.text = text;
    }

    public void Update()
    {
        transform.position += Vector3.up * 2;
        Destroy(gameObject, 1);
    }
}
