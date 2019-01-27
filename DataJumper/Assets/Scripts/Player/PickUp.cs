using UnityEngine;

public class PickUp : MonoBehaviour
{
    private float throwForce = 600f;
    private Vector3 objectPos;
    private float distance;

    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;

    void Update()
    {
        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
        if (distance >= 3f)
        {
            isHolding = false;
        }

        // check if holding
        if (isHolding)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetMouseButton(1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }
        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (distance <= 3f)
            {
                isHolding = true;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().detectCollisions = true;
            }
        }
    }

    void OnMouseExit()
    {
        isHolding = false;
    }
}