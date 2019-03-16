using System.Collections;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cube;
    [SerializeField] private Transform cubeRedeploy;
    public ParticleSystem respawnParticle;
    public ParticleSystem cuberespawnParticle;
    private bool isRespawning;
    private Vector3 respawnPoint;
    private PlayerMovement movementRef;
    public float respawnLength;
    private ToggleTrail trailToggler;

    void Awake()
    {
        movementRef = player.GetComponent<PlayerMovement>();
        trailToggler = player.GetComponent<ToggleTrail>();
        respawnPoint = player.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            RespawnPlayer();
        }
        else if (other.CompareTag("FriendCube"))
        {
            FindObjectOfType<AudioManager>().RandomizePitch("Respawn");
            FindObjectOfType<AudioManager>().Play("Respawn");
            cuberespawnParticle.Play();
            cube.transform.position = cubeRedeploy.transform.position;
            cube.transform.rotation = cubeRedeploy.transform.rotation;
        }
    }

    public void RespawnPlayer()
    {      
        if (!isRespawning)
        {
            StartCoroutine("RespawnCoroutine");
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        isRespawning = true;
        // player.gameObject.SetActive(false);
        // instantiate gore particle at player last position?

        yield return new WaitForSeconds(respawnLength);

        isRespawning = false;

        // player.gameObject.SetActive(true);    
        movementRef.playerVelocity = Vector3.zero; // cancel out all speed the player has gained
        player.transform.position = respawnPoint; // respawn them where they started
        FindObjectOfType<AudioManager>().RandomizePitch("Respawn");
        FindObjectOfType<AudioManager>().Play("Respawn"); // play respawn noise
        respawnParticle.Play(); // play respawn particle
        trailToggler.ClearTrail(); // reset player trail
    }
}
