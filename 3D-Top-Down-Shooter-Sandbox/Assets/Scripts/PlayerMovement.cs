using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement caracteristics :")]
    [SerializeField] public float runSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float fireRate;

    [Header("Components :")]
    public Rigidbody rb;
    public Animator animator;
    public Camera camera;

    private Vector3 movement;
    private Vector3 mousePos;

    [Header("Stats :")]
    public bool isMoving = false;
    public bool isRunning = false;
    public bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Input movements
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        //Input rotation
        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //Input shooting
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        //Movement detection
        if (movement.x != 0 || movement.z != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Animator management
        animator.SetFloat("movementAxisX", movement.x);
        animator.SetFloat("movementAxisZ", movement.z);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isShooting", isShooting);

        
    }

    void FixedUpdate()
    {
        //Movement
        if(!isRunning)
        {
            rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
        }

        
    }
}
