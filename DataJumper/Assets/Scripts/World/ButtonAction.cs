using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public GameObject prefab;

    void OnTriggerEnter(Collider other)
    {
        if (prefab.CompareTag("FriendCube"))
        {
            Debug.Log("Trigger achieved! (Open door, or whatever)");
        }
    }
}
