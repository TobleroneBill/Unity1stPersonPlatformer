using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Player Transforms & Animation")]
    [SerializeField] Transform playerCam = null;
    [SerializeField] CharacterController charController = null;
    [SerializeField] Animator animator;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip[] sounds;

    [Header("Mouse Movement")]
    [SerializeField] float mouseSens = 3.5f;
    [SerializeField] bool lockState = true;
    bool canlook = true;

    [Header("Gravity & Smooth Move")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float walkspeed = 10f;
    //[SerializeField] float runspeed = 20f;
    [SerializeField] float jumpHeight = 4f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [Header("Respawn Point")]
    public bool respawn;
    public Transform spawnPoint;

    [Header("Dash")]
    bool dashing;
    public int dashCount = 1;
    public int currentDashCount;
    public int dashTime;
    private int dashtimer;
    public float dashDistance;

    [Header("Grapple")]
    public int noOfGrapples = 1;
    bool grappling;
    public float grappleDistance = 0f;
    public int grappleTime;
    public float raycastDist;
    int grappleTimer;
    Vector3 grapplePos;
    RaycastHit hit;

    [Header("ThrustPad")]
    public float thrustPower = 1;

    [Header("Gravity")]
    float CamPitch = 0.5f;
    public float velocityY = 0.0f;
    public int jumpCounter = 2;

    public Vector3 velocityPub;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVel = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVel = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        dashtimer = dashTime;
        grappleTimer = grappleTime;
        charController = GetComponent<CharacterController>();
        if (lockState) { 
        Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            charController.enabled = false;
            transform.position = spawnPoint.position;
            respawn = false;
            charController.enabled = true;
        }
        else {
            UpdateMovement();
        }

        if (charController.isGrounded)
        {
            animator.SetBool("InAir", false);
        }

        if (canlook)
        {
            UpdateMouseLook();
        }

    }

    void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVel, mouseSmoothTime);

        CamPitch -= currentMouseDelta.y * mouseSens;
        CamPitch = Mathf.Clamp(CamPitch, -90f, 90f);

        playerCam.localEulerAngles = Vector3.right * CamPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSens);
    }

    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVel, moveSmoothTime);      //smooths movement

        //velocity variable changed in next if statement

        if (Input.GetButtonDown("Fire1") && noOfGrapples > 0)
        {
            audioSource.PlayOneShot(sounds[0]);
            Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDist);
            if (hit.collider.tag == "Grapple")
            {
                jumpCounter = 1;
                currentDashCount = 1;
                Debug.Log(hit.collider);
                grappling = true;
            }
            else { 
                
            }
            //Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward, Color.red,100f);
            
        }

        if (Input.GetButtonDown("Jump") && jumpCounter > 0)
        {
            audioSource.PlayOneShot(sounds[2]);
            Jump();
            // Debug.Log("pressed jump");
        }

        if (Input.GetButtonDown("Fire2") && !dashing && currentDashCount > 0)
        {         // set dashing
            audioSource.PlayOneShot(sounds[1]);
            dashing = true;
            currentDashCount -= 1;
            animator.SetBool("Dashing", true);
        }
        else {
            animator.SetBool("Dashing", false);
        }

        //makes player execute a grapple
        if (grappling)
        {
            Grapple();
            animator.SetBool("Grappling", true);
        }
        else {
            animator.SetBool("Grappling", false);
        }

        //if statement to execute a dash if button is pressed. if dashing is false, it sets gravity to normal and applies normal movement.
        if (dashing)
        {
            Dash();
        }
        
        if (!dashing && !grappling){
            velocityY += gravity * Time.deltaTime;

            velocityPub = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkspeed + Vector3.up * velocityY;
            charController.Move(velocityPub * thrustPower * Time.deltaTime); //move the character controller by the velocity (created earlier)
        }

    }

    private void LateUpdate()
    {


        if (charController.isGrounded)
        {
            velocityY = -2f;
            jumpCounter = 2;
            noOfGrapples = 1;
            currentDashCount = dashCount;
            animator.SetBool("InAir", false);

        }
        else {
            animator.SetBool("InAir", true);
        }
 
    }


    void Dash() {
        if (dashing && dashtimer != 0)      // if dashTimer isnt 0 and dashing = true, dash, otherwise set dash to false and reset the dashtimer
        {
            velocityY = 0;
            Vector3 currentPos = (transform.forward * currentDir.y + transform.right * currentDir.x);
            Debug.Log(currentPos);
            charController.Move(currentPos * dashDistance );
            dashtimer -= 1;
        }
        else {
            dashing = false;
            dashtimer = dashTime;
        }
    }

    void Grapple() {
        if (grappling && grappleTimer != 0)      // if grappleTimer isnt 0 and grappling = true, grapple, otherwise set grapple to false and reset the grappletimer
        {
            canlook = false;
            noOfGrapples -= 1;
            grapplePos = hit.point - transform.position;
            
            Debug.Log("Move by: " + grapplePos * grappleDistance * 0.5f);
            charController.Move(grapplePos * grappleDistance * 0.5f);
            grappleTimer -= 1;
        }
        else
        {
            velocityY = 0;
            grappling = false;
            canlook = true;
            grappleTimer = grappleTime;
        }
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = hit.point;
        }

    void Jump() {
        velocityY = -2f;
        velocityY += Mathf.Sqrt(jumpHeight * -2f * gravity);              // old jump
        jumpCounter -= 1;
    }
}