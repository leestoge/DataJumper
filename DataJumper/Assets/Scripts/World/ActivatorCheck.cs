using UnityEngine;

public class ActivatorCheck : MonoBehaviour
{
    public GameObject snapper; // where to place replica
    public GameObject FriendCube; // Friend Cube
    public GameObject replica; // replica non-interact-able Friend Cube

    public GameObject hologram1; // Hologram-like area of button -  WHAT WE ARE CHANGING THE COLOUR OF
    public GameObject hologram2;
    public Material activeMaterial; // Active material

    public GameObject LevelExit;
    public GameObject movingPlatform;

    void Awake()
    {
        if (movingPlatform == null)
        {
            Debug.Log("Moving Platform doesn't seem to be in this level.");
        }

        if (LevelExit == null)
        {
            Debug.Log("Level Exit doesn't seem to be in this level.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PickUp PickUpHook = FriendCube.GetComponent<PickUp>(); // hook to PickUp.cs script

        if (other.CompareTag("FriendCube") && PickUpHook.isHolding && FriendCube != null) // check if FriendCube, the player is holding it, and the FriendCube isn't somehow null.
        {            
            Debug.Log("Friend cube found!"); // Replace with UI element?
            // voiceline or beep noise

            if (LevelExit != null)
            {
                Debug.Log("Level Exit found");
                LevelExit.SetActive(true);
            }

            if (movingPlatform != null)
            {
                Debug.Log("Moving Platform found");
                movingPlatform.SetActive(true);     
            }
            
            Destroy(FriendCube); // Destroy original Friend Cube
            Instantiate(replica, snapper.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(30, -40), 0))); // Create replica (non-interact-able) friend cube above button with a random rotation on Y axis between 30 - 140 degrees.
            hologram1.gameObject.GetComponent<Renderer>().material = activeMaterial; // Change hologram-like area to the active material (green)
            hologram2.gameObject.GetComponent<Renderer>().material = activeMaterial; // Change hologram-like area to the active material (green)
            gameObject.SetActive(false); // set the trigger to not be active so it doesn't keep checking.
        }
        else
        {
            Debug.Log("You need a friend cube!"); // Replace with UI element?
        }
    }
}
