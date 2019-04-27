using UnityEngine;

public class TimerComplete : MonoBehaviour
{
    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MillisecondDisplay;

    void OnTriggerEnter()
    {
        if (TimerManager.Second_Count <= 9)
        {
            SecondDisplay.GetComponent<TextMesh>().text = "0" + TimerManager.Second_Count + ".";
        }
        else
        {
            SecondDisplay.GetComponent<TextMesh>().text = "" + TimerManager.Second_Count + ".";
        }

        if (TimerManager.Minute_Count <= 9)
        {
            MinuteDisplay.GetComponent<TextMesh>().text = "0" + TimerManager.Minute_Count + ":";
        }
        else
        {
            MinuteDisplay.GetComponent<TextMesh>().text = "" + TimerManager.Minute_Count + ":";
        }

        MillisecondDisplay.GetComponent<TextMesh>().text = "" + TimerManager.Millisecond_Count;
    }
}
