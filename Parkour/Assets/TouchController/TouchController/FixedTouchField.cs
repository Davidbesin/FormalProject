using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    void Update()
    {
        if (Pressed)
        {
            if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            {
                var touch = Touchscreen.current.touches[0];
                if (touch.press.isPressed)
                {
                    Vector2 currentPos = touch.position.ReadValue();
                    TouchDist = currentPos - PointerOld;
                    PointerOld = currentPos;
                }
            }
            else if (Mouse.current != null)
            {
                Vector2 currentPos = Mouse.current.position.ReadValue();
                TouchDist = currentPos - PointerOld;
                PointerOld = currentPos;
            }
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}