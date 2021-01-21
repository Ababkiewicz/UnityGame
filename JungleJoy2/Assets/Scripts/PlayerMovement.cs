using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private float moveSpeed;    
    private Vector3 moveDirection;

    private bool isNotSliding; // is on a slope or not
    public float slideFriction = 0.1f; // ajusting the friction of the slope
    private Vector3 hitNormal; //orientation of the slope.

    //[SerializeField] private bool isGrounded;
    //[SerializeField] private float groundCheckDistance;
    //[SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private Vector3 _velocity;

    [SerializeField] private float jumpHeight;
    private float directionY;

    //REFERENCES
    private CharacterController controller;
    private Animator anim;
    

    void OnControllerColliderHit (ControllerColliderHit hit) 
    {
        hitNormal = hit.normal;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveZ = 0;
        
        if(isNotSliding)
        {
            moveZ = Input.GetAxis("Vertical");
        }               
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if ( controller.isGrounded && _velocity.y < 0 ) 
        {
            _velocity.y = -2f;
        }
        
        if(controller.isGrounded && isNotSliding)
        {   
            if(Input.GetAxis ("Vertical") < 0)
         {
            Backwards();
         }
        else if(moveDirection.z == 0 )
        {   
           Idle();
        }
        else
            {
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
        
         if(Input.GetKeyDown(KeyCode.Space))
         {
             Jump();
         }
         if(Input.GetKeyDown(KeyCode.F))
         {
             Attack();
         }
        }
        

                  

        _velocity.y += gravity * Time.deltaTime;
        if (!isNotSliding)
        {
            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);

        }       
        
        controller.Move(moveDirection * Time.deltaTime * moveSpeed + (_velocity * Time.deltaTime));
        
        
        isNotSliding = Vector3.Angle (Vector3.up, hitNormal) <=  controller.slopeLimit;
        //controller.Move(_velocity * Time.deltaTime);
        anim.SetFloat("directionY", Mathf.Abs(_velocity.y), 0.1f, Time.deltaTime);
        anim.SetBool("isGrounded",  controller.isGrounded);
        
    }
    private void Idle()
    {
        anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }
    private void Backwards()
    {
        anim.SetFloat("Speed", -1f, 0.1f, Time.deltaTime);
    }
    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

    }
    private void Attack()
    {
    }
}
