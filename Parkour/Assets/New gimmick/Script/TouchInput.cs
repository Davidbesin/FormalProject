using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public FixedTouchField touchField; // drag your FixedTouchField object here in Inspector

    void Update()
    {
        // Check if there's swipe input
        if (touchField.Pressed)
        {
            Vector2 swipe = touchField.TouchDist;

            // Detect swipe direction
            if (swipe.magnitude > 50f) // threshold to avoid tiny movements
            {
                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                        SwipeRight();
                    else
                        SwipeLeft();
                }
                else
                {
                    if (swipe.y > 0)
                        SwipeUp();
                    else
                        SwipeDown();
                }

                // Reset after detecting swipe
                touchField.TouchDist = Vector2.zero;
            }
        }
    }

    void SwipeLeft()
    {
        Debug.Log("Swipe Left → Move player left");
        // Add lane change or movement logic here
    }

    void SwipeRight()
    {
        Debug.Log("Swipe Right → Move player right");
    }

    void SwipeUp()
    {
        Debug.Log("Swipe Up → Jump");
    }

    void SwipeDown()
    {
        Debug.Log("Swipe Down → Roll/Slide");
    }
}