using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private static UI Instance;

    [SerializeField]
    private int _currentSlide;

    [SerializeField]
    private RectTransform[] _slides;

    [SerializeField]
    private DamageLabel _labelPrefab;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentSlide++;
            if (_currentSlide >= _slides.Length)
            {
                _currentSlide = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            _currentSlide--;
            if (_currentSlide < 0)
            {
                _currentSlide = _slides.Length - 1;
            }
        }

        for (int i = 0; i < _slides.Length; i++)
        {
            _slides[i].gameObject.SetActive(i == _currentSlide);
        }
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

    public static Vector2 ToWorldPosition(Vector2 screenPosition) 
    {
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
