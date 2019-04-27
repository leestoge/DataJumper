using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public static int Minute_Count;
    public static int Second_Count;
    public static float Millisecond_Count;
    public static string MillisecondDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MillisecondBox;

    void Update()
    {
        Millisecond_Count += Time.deltaTime * 10;
        MillisecondDisplay = Millisecond_Count.ToString("F0");

        MillisecondBox.GetComponent<Text>().text = "" + MillisecondDisplay;

        if (Millisecond_Count >= 10)
        {
            Millisecond_Count = 0;
            Second_Count += 1;
        }

        if (Second_Count <= 9)
        {
            SecondBox.GetComponent<Text>().text = "0" + Second_Count + ".";
        }
        else
        {
            SecondBox.GetComponent<Text>().text = "" + Second_Count + ".";
        }

        if (Second_Count >= 60)
        {
            Second_Count = 0;
            Minute_Count += 1;
        }

        if (Minute_Count <= 9)
        {
           MinuteBox.GetComponent<Text>().text = "0" + Minute_Count + ":";
        }
        else
        {
            MinuteBox.GetComponent<Text>().text = "" + Minute_Count + ":";
        }
    }
}
