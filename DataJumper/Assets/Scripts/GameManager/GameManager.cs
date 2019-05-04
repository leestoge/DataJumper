using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 146;
        TimerManager.Minute_Count = 0;
        TimerManager.Second_Count = 0;
        TimerManager.Millisecond_Count = 0;
    }
}
