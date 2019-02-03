using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform cubeRedeploy;
    public ParticleSystem respawnParticle;
    public ParticleSystem cuberespawnParticle;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // sound clip?
            respawnParticle.Play();
            player.transform.position = respawnPoint.transform.position;
        }
        else if (other.CompareTag("FriendCube"))
        {
            cuberespawnParticle.Play();
            cube.transform.position = cubeRedeploy.transform.position;
        }
    }
}
