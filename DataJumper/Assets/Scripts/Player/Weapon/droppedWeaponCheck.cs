
using UnityEngine;

public class droppedWeaponCheck : MonoBehaviour
{
    private GameObject propWeapon;
    private AudioSource audio;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PropWeapon"))
        {
            propWeapon = other.gameObject;
            audio = propWeapon.GetComponent<AudioSource>();
            audio.Play();
            Destroy(propWeapon, 10f);
        }
    }
}
