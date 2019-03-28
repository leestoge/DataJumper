using System.Collections;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    //~player
    private GameObject player; 
    public ParticleSystem respawnParticle;
    private bool isRespawning;
    private Vector3 respawnPoint;
    private PlayerMovement movementRef;
    public float respawnLength;
    private ToggleTrail trailToggler;

    //~cube
    private GameObject cube;
    private Vector3 cubeRedeploy;
    private Quaternion cubeRotation;
    private ParticleSystem cuberespawnParticle;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movementRef = player.GetComponent<PlayerMovement>();
        trailToggler = player.GetComponent<ToggleTrail>();
        respawnPoint = player.transform.position;

        cube = GameObject.FindGameObjectWithTag("FriendCube");

        if (cube == null)
        {
            Debug.Log("FriendCube not found, is there not a FriendCube in this level?");
        }
        else
        {
            cubeRotation = cube.transform.rotation;
            cubeRedeploy = cube.transform.position;
            cuberespawnParticle = cube.GetComponent<ParticleSystem>();
        }
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
            cube.transform.position = cubeRedeploy;
            cube.transform.rotation = cubeRotation;
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

    public void SetSpawnPoint(Vector3 newPos) // pass in new spawn location
    {
        respawnPoint = newPos; // old respawn point is set to the new spawn location
    }
}
