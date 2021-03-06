﻿using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

// Contains the command the user wishes upon the character
struct Cmd
{
    public float forwardMove;
    public float rightMove;
}

public class PlayerMovement : MonoBehaviour
{
    public Transform playerView; // Camera
    public float playerViewYOffset = 1.9f; // The height at which the camera is bound to
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;

    /*Frame occuring factors*/
    public float gravity = 20.0f;

    public float friction = 6; //Ground friction

    /* Movement stuff */
    public float moveSpeed = 7.0f; // Ground move speed
    public float runAcceleration = 14.0f; // Ground accel
    public float runDeacceleration = 10.0f; // Deacceleration that occurs when running on the ground
    public float airAcceleration = 2.0f; // Air accel
    public float airDecceleration = 2.0f; // Deacceleration experienced when opposite strafing
    public float airControl = 0.3f; // How precise air control is
    public float sideStrafeAcceleration = 50.0f; // How fast acceleration occurs to get up to sideStrafeSpeed when
    public float sideStrafeSpeed = 1.0f; // What the max speed to generate when side strafing
    public float jumpSpeed = 8.0f; // The speed at which the character's up axis gains when hitting jump

    public bool
        holdJumpToBhop; // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.

    /*print() style */
    public GUIStyle style;

    /*FPS Stuff */
    public float fpsDisplayRate = 4.0f; // 4 updates per sec

    private int frameCount;
    private float dt;
    private float fps;

    private CharacterController _controller;

    // Camera rotations
    private float rotX;
    private float rotY;

    private Vector3 moveDirectionNorm = Vector3.zero;
    [HideInInspector]
    public Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity;

    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump;

    // Used to display real time fricton values
    private float playerFriction;

    // Player commands, stores wish commands that the player asks for (Forward, back, jump, etc)
    private Cmd _cmd;

    // Sliding stuff
    private bool isSliding;
    public ParticleSystem slideSparks1;
    public ParticleSystem slideSparks2;

    // Crouching stuff
    private bool isCrouching;

    // UI stuff
    [HideInInspector]
    public float currentSpeed;
    [HideInInspector]
    public float maxSpeed;

    public Text HUD_element_speed; // variable to link to the ui element
    public TextMesh HUD_element_Maxspeed; // variable to link to the ui element

    private void Awake()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                playerView = mainCamera.gameObject.transform;
            }
        }

        // Put the camera inside the capsule collider
        if (playerView != null)
        {
            playerView.position.Set(transform.position.x, transform.position.y + playerViewYOffset,
                transform.position.z);
        }

        _controller = GetComponent<CharacterController>();

        if (HUD_element_Maxspeed == null || HUD_element_speed == null)
        {
            Debug.Log("Sort out the UI");
        }
    }

    private void Update()
    {
        // Do FPS calculation
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / fpsDisplayRate)
        {
            fps = Mathf.Round(frameCount / dt);
            frameCount = 0;
            dt -= 1.0f / fpsDisplayRate;
        }

        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        /* Camera rotation stuff, mouse controls this shit */
        rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

        // Clamp the X rotation
        if (rotX < -90)
        {
            rotX = -90;
        }
        else if (rotX > 90)
        {
            rotX = 90;
        }

        transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
        playerView.rotation = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera

        /* Movement, here's the important part */
        QueueJump();
        if (_controller.isGrounded)
        {
            GroundMove();
        }
        else if (!_controller.isGrounded)
        {
            AirMove();
        }

        var ups = _controller.velocity;
        ups.y = 0;
        currentSpeed = Mathf.Round(ups.magnitude * 100) / 100;
        maxSpeed = Mathf.Round(playerTopVelocity * 100) / 100;

        if (Input.GetKeyDown(KeyCode.LeftControl) && currentSpeed >= 7 && !isSliding) // if left ctrl pressed, player is grounded and are going more than 7ups and not currently sliding
        {           
            StartSliding(); // slide
            Invoke("StopSliding", 2.0f); // stop sliding after 2 seconds
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && currentSpeed >=0 && currentSpeed < 7 && !isCrouching) // if left ctrl pressed, player is grounded and is going below 7ups and not currently crouching
        {
            StartCrouching(); // crouch
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching) // if we release left ctrl and are currently crouching
        {
            StopCrouching(); // stop crouching
        }

        // Move the controller
        _controller.Move(playerVelocity * Time.deltaTime);

        /* Calculate top velocity */
        Vector3 udp = playerVelocity;
        udp.y = 0.0f;
        if (udp.magnitude > playerTopVelocity)
        {
            playerTopVelocity = udp.magnitude;
        }

        //Need to move the camera after the player has been moved because otherwise the camera will clip the player if going fast enough and will always be 1 frame behind.
        // Set the camera's position to the transform
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        // UI

        HUD_element_speed.text = currentSpeed.ToString(CultureInfo.CurrentCulture) + "ups";
        HUD_element_Maxspeed.text = "Top Speed: " + maxSpeed.ToString(CultureInfo.CurrentCulture) + "ups";


        HUD_element_speed.color = Color.Lerp(HUD_element_speed.color, currentSpeed >= 15 ? Color.yellow : Color.white, Time.deltaTime * 3f);
        HUD_element_speed.color = Color.Lerp(HUD_element_speed.color, currentSpeed >= 25 ? Color.green : Color.white, Time.deltaTime * 3f);
        HUD_element_speed.color = Color.Lerp(HUD_element_speed.color, currentSpeed >= 35 ? Color.cyan : Color.white, Time.deltaTime * 3f);
    }

    /*******************************************************************************************************\
   |* MOVEMENT
   \*******************************************************************************************************/

    /**
     * Sets the movement direction based on player input
     */
    private void SetMovementDir()
    {
        _cmd.forwardMove = Input.GetAxisRaw("Vertical");
        _cmd.rightMove = Input.GetAxisRaw("Horizontal");
    }

    /**
     * Queues the next jump just like in Q3
     */
    private void QueueJump()
    {
        if (holdJumpToBhop)
        {
            wishJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !wishJump)
        {
            wishJump = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            wishJump = false;
        }
    }

    /**
     * Execs when the player is in the air
    */
    private void AirMove()
    {
        Vector3 wishdir;
        float wishvel = airAcceleration;
        float accel;

        SetMovementDir();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        wishdir.Normalize();
        moveDirectionNorm = wishdir;

        // CPM: Aircontrol
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
        {
            accel = airDecceleration;
        }
        else
        {
            accel = airAcceleration;
        }

        // If the player is ONLY strafing left or right
        if (_cmd.forwardMove == 0 && _cmd.rightMove != 0)
        {
            if (wishspeed > sideStrafeSpeed)
            {
                wishspeed = sideStrafeSpeed;
                accel = sideStrafeAcceleration;
            }
        }

        Accelerate(wishdir, wishspeed, accel);
        if (airControl > 0)
        {
            AirControl(wishdir, wishspeed2);
        }
        // !CPM: Aircontrol

        // Apply gravity
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    /**
     * Air control occurs when the player is in the air, it allows
     * players to move side to side much faster rather than being
     * 'sluggish' when it comes to cornering.
     */
    private void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;

        // Can't control movement if not moving forward or backward
        if (Mathf.Abs(_cmd.forwardMove) < 0.001 || Mathf.Abs(wishspeed) < 0.001)
        {
            return;
        }

        zspeed = playerVelocity.y;
        playerVelocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, wishdir);
        k = 32;
        k *= airControl * dot * dot * Time.deltaTime;

        // Change direction while slowing down
        if (dot > 0)
        {
            playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
            playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
            playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;

            playerVelocity.Normalize();
            moveDirectionNorm = playerVelocity;
        }

        playerVelocity.x *= speed;
        playerVelocity.y = zspeed; // Note this line
        playerVelocity.z *= speed;
    }

    /**
     * Called every frame when the engine detects that the player is on the ground
     */
    private void GroundMove()
    {
        Vector3 wishdir;

        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0);
        }

        SetMovementDir();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);

        // Reset the gravity velocity
        playerVelocity.y = -gravity * Time.deltaTime;

        if (wishJump && !isSliding && !isCrouching)
        {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
        }

        if (playerVelocity.y >= jumpSpeed)
        {
            FindObjectOfType<AudioManager>().RandomizePitch("Jump");
            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }

    private void StartCrouching()
    {
        isCrouching = true;
        _controller.height /= 2f;
        moveSpeed /= 1.5f;
        _controller.center.Set(_controller.center.x, _controller.center.y / 2, _controller.center.z);
    }

    private void StopCrouching()
    {
        isCrouching = false;
        _controller.height *= 2f;
        moveSpeed *= 1.5f;
        _controller.center.Set(_controller.center.x, _controller.center.y * 2, _controller.center.z);
    }

    private void StartSliding()
    {
        isSliding = true;
        moveSpeed *= 2f;
        _controller.height /= 2f;
        _controller.center.Set(_controller.center.x, _controller.center.y / 2, _controller.center.z);
        FindObjectOfType<AudioManager>().Play("Slide");

        if (_controller.isGrounded)
        {
            slideSparks1.Play();
            slideSparks2.Play();
        }
    }

    private void StopSliding()
    {
        isSliding = false;
        moveSpeed /= 2f;
        _controller.height *= 2f;
        _controller.center.Set(_controller.center.x, _controller.center.y * 2, _controller.center.z);

        if (slideSparks1.isPlaying)
        {
            slideSparks1.Stop();
        }

        if (slideSparks2.isPlaying)
        {
            slideSparks2.Stop();
        }
    }

    /**
     * Applies friction to the player, called in both the air and on the ground
     */
    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity;
        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        /* Only if the player is on the ground then apply friction */
        if (_controller.isGrounded)
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newspeed = speed - drop;
        playerFriction = newspeed;

        if (newspeed < 0)
        {
            newspeed = 0;
        }

        if (speed > 0)
        {
            newspeed /= speed;
        }

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }

        accelspeed = accel * Time.deltaTime * wishspeed;

        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
    }

    //private void OnGUI()
    //{
    //    style.normal.textColor = Color.green;
    //    GUI.Label(new Rect(0, 0, 400, 100), "FPS: " + fps, style);
    //    var ups = _controller.velocity;
    //    ups.y = 0;
    //    GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups", style);
    //    GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "ups", style);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            FindObjectOfType<AudioManager>().RandomizePitch("Land");
            FindObjectOfType<AudioManager>().Play("Land");

            if (isSliding)
            {
                slideSparks1.Play();
                slideSparks2.Play();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (isSliding)
            {
                slideSparks1.Stop();
                slideSparks2.Stop();
            }
        }
    }
}
