using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class DragUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public UnityEvent OnRelease;

    private Button _button;
    private Vector2 _initialPos;
    [SerializeField]
    private bool _dragging;
    private bool _hover;
    private Vector2 _mouseOffset;

    void Start() 
    {
        _initialPos = transform.position;
    }

    void Update() 
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            _dragging = false;
            transform.position = _initialPos;
            OnRelease?.Invoke();
        }

        if (_dragging) 
        {
            transform.position = ((Vector2)Input.mousePosition);
        }
    }

    void OnValidate() 
    {
        if (_button == null) 
        {
            _button = GetComponent<Button>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hover = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _mouseOffset = Input.mousePosition;
        _dragging = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _dragging = false;
        transform.position = _initialPos;
        
    }
}
