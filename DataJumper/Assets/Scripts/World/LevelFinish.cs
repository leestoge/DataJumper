using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    public ParticleSystem winnerParticle;
    public TimeManager timeManager;
    public GameObject statsCam;
    public GameObject gameplayUI;
    private readonly WaitForSeconds _delay = new WaitForSeconds(5f);

    // god..
    public GameObject MinuteDisplay;
    public GameObject SecondDisplay;
    public GameObject MillisecondDisplay;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

            MillisecondDisplay.GetComponent<TextMesh>().text = "" + TimerManager.Millisecond_Count.ToString("F0");

            // start platform particles
            winnerParticle.Play();
            // sound?       
            timeManager.InitiateSlowMotion(); // slow mo
            Debug.Log("<color=cyan>You win!</color>"); // Replace with UI element?

            StartCoroutine(WaitForStats(other));
        }
    }

    private IEnumerator WaitForStats(Collider player)
    {
        yield return _delay;

        player.gameObject.SetActive(false);
        gameplayUI.SetActive(false);
        statsCam.SetActive(true);

        // load next scene
    }
}
