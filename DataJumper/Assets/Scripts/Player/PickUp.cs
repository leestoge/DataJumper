using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float throwForce;
    private Vector3 objectPos;
    private float distance;

    public GameObject item;
    public GameObject tempParent;
    public bool isHolding;
    private bool toggle;

    private bool lookingAtObject;
    private Outline outline;

    private bool audioPlayed;

    void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
    }
    void Update()
    {
        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);

        if (lookingAtObject)
        {
            outline.OutlineWidth = 10;
        }
        else
        {
            outline.OutlineWidth = 0;
        }       

        if (distance >= 6f)
        {
            isHolding = false;
            lookingAtObject = false;
        }

        // check if holding
        if (isHolding)
        {           
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (!toggle)
            {
                audioPlayed = false;
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
        if (distance <= 6f)
        {
            // outline/glow
            lookingAtObject = true;
            // e to pick up, e again to throw
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayAudio();
                toggle = !toggle;
            }
        }

        if (toggle) // picked up
        {
            audioPlayed = true;
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
        toggle = false;
    }

    private void PlayAudio()
    {
        if (!audioPlayed)
        {
            FindObjectOfType<CubeTalk>().PlayPickUpAudio();
        }
    }
}