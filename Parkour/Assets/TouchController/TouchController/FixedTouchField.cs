using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Events fire once per swipe, like button presses
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;

    private Vector2 startPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - startPos;

        // Decide direction once, like pressing a D-pad button
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0) OnSwipeRight?.Invoke();
            else OnSwipeLeft?.Invoke();
        }
        else
        {
            if (delta.y > 0) OnSwipeUp?.Invoke();
            else OnSwipeDown?.Invoke();
        }
    }
}