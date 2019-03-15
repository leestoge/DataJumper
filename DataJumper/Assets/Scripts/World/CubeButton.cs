using UnityEngine;

public class CubeButton : MonoBehaviour
{
    public GameObject snapper; // where to place replica
    public GameObject item; // Friend Cube
    public GameObject replica; // replica non-interact-able Friend Cube
    public GameObject hologram; // Hologram-like area of button
    public Material activeMaterial; // Active material

    void OnTriggerEnter(Collider other)
    {
        PickUp PickUpHook = item.GetComponent<PickUp>(); // hook to PickUp.cs script
        if (item.CompareTag("FriendCube") && PickUpHook.isHolding && item != null) // check if FriendCube, the player is holding it, and the FriendCube isn't somehow null.
        {            
            Debug.Log("Friend cube found!"); // Replace with UI element?
            // voiceline or beep noise
            Destroy(item); // Destroy original Friend Cube
            Instantiate(replica, snapper.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(30, -40), 0))); // Create replica (non-interact-able) friend cube above button with a random rotation on Y axis between 30 - 140 degrees.
            hologram.gameObject.GetComponent<Renderer>().material = activeMaterial; // Change hologram-like area of button to the active material (green)
            Destroy(gameObject); // Destroy the trigger detection box collider
        }
        else
        {
            Debug.Log("You need a friend cube!"); // Replace with UI element?
        }
    }
}
