using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class PMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    public float groundDrag;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    
    public float jumpForce = 10f;
    private float horizontalInput;
    private float verticalInput;
    
    private Rigidbody _rb;
    [SerializeField] Transform orientation;
    
    [SerializeField] GameObject Jpole;
    [SerializeField] GameObject FPole;
    [SerializeField] GameObject Vpole;
    
    private Vector3 jumpVector;
    private Vector3 moveDirection;
    private float airMultiplier;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
         _rb.freezeRotation = true;

        if (_rb == null) 
        {
            Debug.LogError("Rigidbody not found on the player object.");
        }
    }

    void Update()
    {
        GroundCheck();
        MyInput();
        PoleUp();
        Jump();
        SpeedControl();
        FPole.transform.forward = orientation.forward;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        if (grounded)
            _rb.drag = groundDrag;
        else
        {
            _rb.drag = 0;
        }
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void PoleUp()
    {
        Jpole.transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.up * jumpForce);
        
        
        Vpole.transform.localScale =  new Vector3(0.1f, _rb.velocity.magnitude * 0.05f, 0.1f); 
        Vpole.transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z);
        Vpole.transform.rotation = Quaternion.FromToRotation(Vector3.up, _rb.velocity);
    }

    void MovePlayer()
    {
        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            _rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);    
        }
        else if(!grounded)
        {
            _rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);    
        }
        else if (_rb.velocity.magnitude < 1)
        {
           _rb.velocity = Vector3.zero;
        }
       else  
       {
           _rb.AddForce(Vector3.Max(_rb.velocity * -0.1f, _rb.velocity.normalized * -1f));
       }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpVector =  transform.up * jumpForce;
            _rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
}