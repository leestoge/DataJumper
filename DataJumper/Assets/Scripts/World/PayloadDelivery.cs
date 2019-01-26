using UnityEngine;

public class PayloadDelivery : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // start platform particles
        // sound?
        // slow mo?
        Debug.Log("You win.");
    }
}
