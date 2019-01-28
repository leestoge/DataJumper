using UnityEngine;

public class PickUp : MonoBehaviour
{
    private float throwForce = 600f;
    private Vector3 objectPos;
    private float distance;

    public GameObject item;
    public GameObject tempParent;
    public bool isHolding;
    private bool toggle;

    private bool lookingAtObject;
    public GameObject Outline;

    void Update()
    {
        if (lookingAtObject)
        {
            Outline.SetActive(true);
        }
        else
        {
            Outline.SetActive(false);
        }

        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);

        if (distance >= 3f)
        {
            isHolding = false;
            Outline.SetActive(false);
        }

        // check if holding
        if (isHolding)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (!toggle)
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
        if (distance <= 3f)
        {
            // outline/glow
            lookingAtObject = true;
            // e to pick up
            if (Input.GetKeyDown(KeyCode.E))
            {
                toggle = !toggle;
            }
        }

        if (toggle) // picked up
        {
            lookingAtObject = false;
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    void OnMouseExit()
    {
        lookingAtObject = false;
        isHolding = false;
        Outline.SetActive(false);
        toggle = false;
    }
}