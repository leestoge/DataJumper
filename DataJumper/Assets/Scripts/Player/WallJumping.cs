using UnityEngine;

public class WallJumping : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerMovement movementRef;
    private float upwardsThrust = 10f;
    private float speedGain = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        movementRef = GetComponent<PlayerMovement>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_controller.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {            
                Debug.DrawRay(hit.point, hit.normal, Color.magenta, 1.25f);
                movementRef.playerVelocity = hit.normal * speedGain;
                movementRef.playerVelocity.y = upwardsThrust;
                FindObjectOfType<AudioManager>().RandomizePitch("Jump");
                FindObjectOfType<AudioManager>().Play("Jump");
            }
        }
    }
}
