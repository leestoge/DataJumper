using UnityEngine;

public class pickupRocketLauncher : MonoBehaviour
{
    private GameObject RLonFloor;
    public GameObject actualRL;

    void Awake()
    {
        RLonFloor = gameObject.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            FindObjectOfType<AudioManager>().Play("PickUp");
            actualRL.SetActive(true);
            Destroy(RLonFloor);
        }
    }
}
