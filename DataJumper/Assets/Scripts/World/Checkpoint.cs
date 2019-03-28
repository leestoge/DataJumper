using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public OutOfBounds SpawnManager;
    public GameObject hologram; // Hologram-like area of button -  WHAT WE ARE CHANGING THE COLOUR OF
    public Material activeMaterial; // Active material
    public Material nonactiveMaterial; // Non-active material
    private AudioSource checkpointNoise;
    private bool audioPlayed;

    void Awake()
    {
        SpawnManager = FindObjectOfType<OutOfBounds>();
        checkpointNoise = hologram.GetComponent<AudioSource>();
    }

    public void CheckpointOn()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (Checkpoint cp in checkpoints)
        {
            cp.CheckpointOff();
        }

        hologram.gameObject.GetComponent<Renderer>().material = activeMaterial; // Change hologram-like area of button to the active material (green)
        audioPlayed = true;
    }

    public void CheckpointOff()
    {
        hologram.gameObject.GetComponent<Renderer>().material = nonactiveMaterial; // Change hologram-like area of button to the non-active material (red)
        audioPlayed = false;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger achieved!");
            PlayAudio();
            SpawnManager.SetSpawnPoint(transform.position);
            CheckpointOn();  
        }
    }

    private void PlayAudio()
    {
        if (!audioPlayed)
        {
            checkpointNoise.Play();
        }
    }
}
