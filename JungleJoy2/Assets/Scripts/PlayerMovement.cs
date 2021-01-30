using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private float moveSpeed;    
    private Vector3 moveDirection;
    public string animationName;
    public int points = 0;

    public AudioSource sounds;
    [SerializeField] public AudioClip coinSound;
    [SerializeField] public AudioClip pearlSound;

    [SerializeField] private bool isNotSliding; // is on a slope or not
    public float slideFriction = 0.5f; // ajusting the friction of the slope
    public float knockbackForce;
    public float knocbackTime;
    private float knocbackCounter;
    private Vector3 hitNormal; //orientation of the slope.

    private Vector3 hitPoint; //orientation of the slope.

    [SerializeField] private float gravity;
    [SerializeField] private float attackedEnemy;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float jumpHeight;   
    
    //REFERENCES
    private CharacterController controller;
    private Animator anim;
    

    void OnControllerColliderHit (ControllerColliderHit hit) 
    {

        
        hitNormal = hit.normal;
        hitPoint = hit.point;
        if(hit.gameObject.tag.Equals("Enemy") )
        {
                
            Debug.Log("Trace 1 zycie");
            GetComponent<Health>().Damage();
            Vector3 pushDir = transform.position - hit.transform.position; // albo odwrotnie jak cos
            pushDir = pushDir.normalized;
            Knocback(pushDir);            

        }
        if (hit.gameObject.tag.Equals("Coin"))
        {
            sounds.PlayOneShot(coinSound);
            hit.gameObject.SendMessage("destroy");
            points++;
        }
        if (hit.gameObject.tag.Equals("Pearl"))
        {
            sounds.PlayOneShot(pearlSound);
            hit.gameObject.SendMessage("destroy");
            points = points + 10;
        }
        if(hit.gameObject.tag.Equals("Enemy") && animationName=="Hurricane Kick")
        {
            
        
            Vector3 flyDir = new Vector3(transform.position.x + (transform.forward.x * 400)  , 0, transform.position.z + (transform.forward.z * 400)  );
            Debug.Log(transform.forward);
            Debug.Log(flyDir);
            Debug.Log(transform.position);
            hit.gameObject.SendMessage("Die",flyDir);
        }

   
            
    }


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        sounds = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {   
        
        animationName= anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        float moveZ = 0;        
        if(isNotSliding && knocbackCounter <= 0)
        {
            moveZ = Input.GetAxis("Vertical");
        }
                      
       

        if ( controller.isGrounded && _velocity.y < 0  && knocbackCounter <= 0) 
        {
            _velocity.y = -2f;
        }
        
        if(controller.isGrounded && isNotSliding && knocbackCounter <= 0)
        {   
         moveDirection = new Vector3(0, 0, moveZ);
         moveDirection = transform.TransformDirection(moveDirection);
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
        }
         if(animationName=="Hurricane Kick" )
         {
            controller.radius = 2.1f;
            controller.center = new Vector3(0, 2.1f, 0);
         }
         else
         {
            controller.radius = 0.6f;
            controller.center = new Vector3(0, 1.69f, 0); 
         }
       
        if(Input.GetKeyDown(KeyCode.R) && animationName!="Hurricane Kick" )
         {
             Attack();
         }
         if (knocbackCounter > 0) 
         {
             knocbackCounter -= Time.deltaTime;
         }

                  

        _velocity.y += gravity * Time.deltaTime;
        if (!isNotSliding)
        {   
            anim.SetBool("isNotSliding",   isNotSliding);
            _velocity.y += gravity * Time.deltaTime;
            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (slideFriction);
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (slideFriction);

        }       
        
        controller.Move(moveDirection * Time.deltaTime * moveSpeed + (_velocity * Time.deltaTime));
        
        
        isNotSliding = Vector3.Angle (Vector3.up, hitNormal) <=  controller.slopeLimit;
        //controller.Move(_velocity * Time.deltaTime);
        anim.SetFloat("directionY", Mathf.Abs(_velocity.y), 0.1f, Time.deltaTime);
        anim.SetBool("isGrounded",  controller.isGrounded);
        anim.SetBool("isNotSliding",   isNotSliding);
  
        

        
        
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
        anim.Play("Hurricane Kick", 0, 0.25f);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Score : " + points);
    }
    public void Knocback(Vector3 direction)
    {
        knocbackCounter = knocbackTime;

        moveDirection = direction * knockbackForce ;
        moveDirection.y = knockbackForce;

    }
    IEnumerator Wait(float duration)
    {
        
    yield return new WaitForSeconds(duration);   //Wait
        
    }
}
