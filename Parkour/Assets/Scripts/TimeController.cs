using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Method for Button 1
    public void SlowDownTime()
    {
        Time.timeScale = Mathf.Max(0f, Time.timeScale - 0.1f);
        Debug.Log("Time slowed: " + Time.timeScale);
    }

    // Method for Button 2
    public void ResetTime()
    {
        Time.timeScale = 1f;
        Debug.Log("Time reset to normal");
    }
}
