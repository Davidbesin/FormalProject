using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] FixedTouchField touchfield;
    [SerializeField] DynamicJoystick LeftJoystick;

    [HideInInspector] public bool front;
    [HideInInspector] public bool left;
    [HideInInspector] public bool right;
    [HideInInspector] public bool back;
    [HideInInspector] public float touchLeftRight;
    [HideInInspector] public float touchUpDown;
    [HideInInspector] public float leftRight;
    [HideInInspector] public float frontBack;
    [HideInInspector] public bool slower;
    [HideInInspector] public bool faster;

    //[HideInInspector]
    public bool jumpButtonPressed;

    void FixedUpdate()
    {
        touchLeftRight = touchfield.TouchDist.x;
        touchUpDown = touchfield.TouchDist.y;

        left = LeftJoystick.Horizontal > 0;
        right = LeftJoystick.Horizontal < 0;
        front = LeftJoystick.Vertical > 0;
        back = LeftJoystick.Vertical < 0;

        leftRight = LeftJoystick.Horizontal;
        frontBack = LeftJoystick.Vertical;

        Joystick.InputRange range = LeftJoystick.GetInputRange();
        slower = range == Joystick.InputRange.Lesser;
        faster = range == Joystick.InputRange.Greater;

        
    }

    public void JumpButtondown()
    {
        jumpButtonPressed = true;
    }
    public void JumpButtonup()
    {
        jumpButtonPressed = false;
    }
}
