using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Shooting caracteristics :")]
    [SerializeField] public float fireRate;
    private float fireCountdown = 0;

    [Header("Movement caracteristics :")]
    [SerializeField] public float runSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float offset;

    [Header("Components :")]
    public Rigidbody rb;
    public Animator animator;
    public Camera camera;
    public Transform shootPoint;
    public GameObject projectile;

    private Vector3 movement;
    private Vector3 mousePos;
    private Vector3 pointToLook;

    [Header("Stats :")]
    public bool isMoving = false;
    public bool isRunning = false;
    public bool isShooting = false;

    private List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Input movements
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //Input shooting while mooving
        if (Input.GetMouseButton(0) && isMoving && !isRunning)
        {
            isShooting = true;
            if (fireCountdown <= 0)
            {
                ShootProjectile();
                fireCountdown = 1 / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

        //Input shooting while idle
        else if (Input.GetMouseButton(0) && !isMoving && !isRunning)
        {
            isShooting = true;
            if (fireCountdown <= 0)
            {
                ShootProjectile();
                fireCountdown = 1 / (fireRate * 2);
            }
            fireCountdown -= Time.deltaTime;
        }
        else
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
        if (!isRunning)
        {
            rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 newPosition = new Vector3(pointToLook.x, pointToLook.y + offset, pointToLook.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, runSpeed * Time.deltaTime);
        }
    }

    void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(projectile, shootPoint.transform.position, shootPoint.rotation);
        Destroy(projectileGO, 1.0f);
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(projectile.GetComponent<ParticleSystem>(), other, collisionEvents);

        foreach(ParticleCollisionEvent colission in collisionEvents)
        {
            Debug.Log(colission);
        }
    }
}

