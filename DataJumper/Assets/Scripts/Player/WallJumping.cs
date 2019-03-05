using UnityEngine;

public class WallJumping : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerMovement movementRef;

    // help
    private Vector3 moveVector;
    private Vector3 lastMove;
    private float speed = 8f;
    private float jumpForce = 8f;
    private float verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        movementRef = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //movementRef.playerVelocity = Vector3.zero;
        //movementRef.playerVelocity.y = 0;
        //movementRef.playerVelocity.Normalize();
        //movementRef.playerVelocity *= speed;
        //movementRef.playerVelocity.y = verticalVelocity;

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_controller.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.magenta, 1.25f);
                movementRef.playerVelocity = hit.normal *10f;
                movementRef.playerVelocity.y = speed;
            }
        }
    }
}
