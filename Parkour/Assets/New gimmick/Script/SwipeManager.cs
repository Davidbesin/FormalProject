using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SwipeManager : MonoBehaviour
{
    public static event Action<Vector2> OnSwipe;
    private Vector2 _startPos;
    private const float Deadzone= 10f; // Ignore jitters smaller than 10px

    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
            _startPos = Pointer.current.position.ReadValue();

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            Vector2 endPos = Pointer.current.position.ReadValue();
            Vector2 diff = endPos - _startPos;

            if (diff.magnitude < Deadzone) return;
            OnSwipe?.Invoke(diff.normalized);
        }
    }
}
