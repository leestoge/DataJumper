using UnityEngine;

public class CubeButton : MonoBehaviour
{
    public GameObject snapper;
    public GameObject item;
    public GameObject replica;

    void OnTriggerEnter(Collider other)
    {
        PickUp PickUpHook = item.GetComponent<PickUp>();
        if (item.CompareTag("FriendCube") && PickUpHook.isHolding && item != null)
        {            
            Debug.Log("Friend cube found!");
            // voiceline?
            Destroy(item);
            Instantiate(replica, snapper.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(30, 140), 0)));
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("You need a friend cube!");
        }
    }
}
