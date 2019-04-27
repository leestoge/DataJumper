using UnityEngine;

public class CubeTalk : MonoBehaviour
{
    private AudioSource _AudioSource;

    public AudioClip[] Greeting;
    public AudioClip[] PickedUp;
    public AudioClip[] EnterArea;
    readonly System.Random rnd = new System.Random();
    private int Chance;

    void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Chance = rnd.Next(1, 3);
    }

    private void PickUpClip()
    {
        _AudioSource.clip = PickedUp[Random.Range(0, PickedUp.Length)];
    }

    public void PlayGreetingAudio()
    {
        GreetingClip();
        if (!_AudioSource.isPlaying && Chance != 1)
        {
            _AudioSource.PlayOneShot(_AudioSource.clip);
        }
    }

    public void PlayPickUpAudio()
    {
        PickUpClip();
        if (!_AudioSource.isPlaying && Chance != 1)
        {
            _AudioSource.PlayOneShot(_AudioSource.clip);
        }
    }

    private void GreetingClip()
    {
        _AudioSource.clip = Greeting[Random.Range(0, Greeting.Length)];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CubeTalk>().PlayGreetingAudio();
        }
    }
}
