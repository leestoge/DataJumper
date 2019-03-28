using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    public ParticleSystem winnerParticle;
    public TimeManager timeManager;
    private readonly WaitForSeconds _delay = new WaitForSeconds(3f);

    private IEnumerator WaitForSceneLoad()
    {
        yield return _delay;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //FindObjectOfType<AudioManager>().SwitchMusic("Level" + SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter(Collider other)
    {
        // start platform particles
        winnerParticle.Play();
        // sound?

        // slow mo?
        timeManager.InitiateSlowMotion();
        Debug.Log("<color=cyan>You win!</color>");  // Replace with UI element?

        StartCoroutine(WaitForSceneLoad());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
