using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    public ParticleSystem winnerParticle;
    public TimeManager timeManager;
    public GameObject statsCam;
    public GameObject gameplayUI;
    private readonly WaitForSeconds _delay1 = new WaitForSeconds(5f);
    private readonly WaitForSeconds _delay2 = new WaitForSeconds(10f);

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

            TimerManager.Minute_Count = 0;
            TimerManager.Second_Count = 0;
            TimerManager.Millisecond_Count = 0;

            // start platform particles
            winnerParticle.Play();
            FindObjectOfType<AudioManager>().Play("LevelFinish");
            timeManager.InitiateSlowMotion(); // slow mo
            Debug.Log("<color=cyan>You win!</color>"); // Replace with UI element?

            StartCoroutine(WaitForStats(other));
        }
    }

    private IEnumerator WaitForStats(Collider player)
    {
        yield return _delay1;

        player.gameObject.SetActive(false);
        gameplayUI.SetActive(false);
        statsCam.SetActive(true);

        yield return _delay2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
