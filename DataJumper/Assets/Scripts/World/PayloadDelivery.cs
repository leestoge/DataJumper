using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PayloadDelivery : MonoBehaviour
{
    public ParticleSystem winnerParticle;
    public TimeManager timeManager;

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    void OnTriggerEnter(Collider other)
    {
        // start platform particles
        winnerParticle.Play();
        // sound?

        // slow mo?
        timeManager.InitiateSlowMotion();

        Debug.Log("You win.");  // Replace with UI element?

        StartCoroutine(WaitForSceneLoad());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
