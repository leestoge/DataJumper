using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    public ParticleSystem pickupParticle;

    void OnTriggerEnter(Collider other)
    {
        // sound clip?
        pickupParticle.Play();
        player.transform.position = respawnPoint.transform.position;
    }
}
