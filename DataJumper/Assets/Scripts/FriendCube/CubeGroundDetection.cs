using UnityEngine;

public class CubeGroundDetection : MonoBehaviour
{
    public bool isGrounded;

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            
            isGrounded = false;
            
        }
    }
}
