using UnityEngine;

public class PayloadDelivery : MonoBehaviour
{
    public ParticleSystem winnerParticle;
    public TimeManager timeManager;

    void OnTriggerEnter(Collider other)
    {
        // start platform particles
        winnerParticle.Play();
        // sound?

        // slow mo?
        timeManager.InitiateSlowMotion();

        Debug.Log("You win.");  // Replace with UI element?
    }
}
