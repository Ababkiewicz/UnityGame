using UnityEngine;

public class GameEngine : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private float moveSpeed;
    private Vector3 moveDirection;
    public string animationName;
    public int points = 0;
    [SerializeField] private bool isNotSliding; 
    public float slideFriction = 0.5f; 
    public float knockbackForce;
    public float knocbackTime;
    private float knocbackCounter;
    private Vector3 hitNormal; 
    private Vector3 hitPoint; 
    public string hitTarget;
    private int phase;
    public GameObject prevTarget;
    [SerializeField] private float gravity;
    [SerializeField] private float attackedEnemy;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float jumpHeight;

    //SOUNDS
    public AudioSource sounds;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip pearlSound;
    [SerializeField] private AudioClip characterHit;
    [SerializeField] private AudioClip characterDeath;
    [SerializeField] private AudioClip characterSpin;
    [SerializeField] private AudioClip characterJump;
    [SerializeField] private AudioClip musicForest;
    [SerializeField] private AudioClip musicGoblin;
    [SerializeField] private AudioClip musicStronghold;
    public AudioSource music;



    //REFERENCES
    [SerializeField] private GameOver gameOver;
    private CharacterController controller;
    private Animator anim;
    private Animator transitionAnim;
    

    void OnControllerColliderHit(ControllerColliderHit hit) 
    {


        hitNormal = hit.normal;
        hitPoint = hit.point;
        hitTarget = hit.gameObject.tag;

        if (hitTarget.Equals("Enemy") && animationName == "Hurricane Kick" )
        {
            Vector3 flyDir = new Vector3(transform.forward.x , 0.2f, transform.forward.z);            
            flyDir = flyDir.normalized;
            hit.gameObject.SendMessage("Die", flyDir);
            
            
        }
        else if (hitTarget.Equals("Enemy") && hit.gameObject != prevTarget)
        {
            
            Damage();
            Vector3 pushDir = transform.position - hit.transform.position; 
            pushDir = pushDir.normalized;
            Knocback(pushDir);   
        }
        
        if (hitTarget.Equals("Coin"))
        {
            sounds.PlayOneShot(coinSound);
            hit.gameObject.SendMessage("destroy");
            points++;
        }
        if (hitTarget.Equals("Pearl"))
        {
            sounds.PlayOneShot(pearlSound);
            hit.gameObject.SendMessage("destroy");
            points = points + 10;
            if (GetComponent<Health>().numOfHearts < 3)
            {
                GetComponent<Health>().numOfHearts += 1;
            }
        }
        if (hitTarget.Equals("Water"))
        {
            Damage();
            Damage();
            Damage();
        }
        prevTarget = hit.gameObject;
    }
    

    private void Start()
    {
        transitionAnim = GameObject.Find("WallTransition").GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        sounds = GetComponent<AudioSource>();
        music = GameObject.Find("WallTransition").GetComponentInChildren<AudioSource>();
        transitionAnim.SetTrigger("start");
        transitionAnim.SetTrigger("spiders");
        phase = 1;

    }

    private void Update()
    {
        if (GetComponent<Health>().numOfHearts <= 0)
        {
            GameOver();
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        Phase();
        animationName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        float moveZ = 0;
        if (hitTarget == "Terrain")
        {
            isNotSliding = Vector3.Angle(Vector3.up, hitNormal) <= controller.slopeLimit;
        }
        else
        {
            isNotSliding = true;
        }

        if (isNotSliding && knocbackCounter <= 0)
        {
            moveZ = Input.GetAxis("Vertical");
        }



        if (controller.isGrounded && _velocity.y < 0 && knocbackCounter <= 0)
        {
            _velocity.y = -2f;
        }

        if (controller.isGrounded && isNotSliding && knocbackCounter <= 0)
        {
            moveDirection = new Vector3(0, 0, moveZ);
            moveDirection = transform.TransformDirection(moveDirection);
            if (Input.GetAxis("Vertical") < 0)
            {
                Backwards();
            }
            else if (moveDirection.z == 0)
            {
                Idle();
            }
            else
            {
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        if (animationName == "Hurricane Kick")
        {
            controller.radius = 2.1f;
            controller.center = new Vector3(0, 2.1f, 0);
        }
        else
        {
            controller.radius = 0.6f;
            controller.center = new Vector3(0, 1.69f, 0);
        }

        if (Input.GetKeyDown(KeyCode.R) && animationName != "Hurricane Kick")
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
            anim.SetBool("isNotSliding", isNotSliding);
            _velocity.y += gravity * Time.deltaTime;
            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (slideFriction);
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (slideFriction);

        }

        controller.Move(moveDirection * Time.deltaTime * moveSpeed + (_velocity * Time.deltaTime));



        
        
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetBool("isNotSliding", isNotSliding);
        anim.SetInteger("life", GetComponent<Health>().numOfHearts);




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
        sounds.PlayOneShot(characterJump);
        _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

    }
    private void Attack()
    {
        sounds.PlayOneShot(characterSpin);
        anim.Play("Hurricane Kick", 0, 0.25f);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Score : " + points);
    }
    public void Knocback(Vector3 direction)
    {
        knocbackCounter = knocbackTime;

        moveDirection = direction * knockbackForce;
        moveDirection.y = knockbackForce*0.5f;

    }
    public void Damage()
    {
        if (GetComponent<Health>().numOfHearts > 1)
        {
            GetComponent<Health>().Damage();
            sounds.PlayOneShot(characterHit);
        }
        else
        {
            GetComponent<Health>().Damage();
            sounds.PlayOneShot(characterDeath);

        } 
            
    }

    public void GameOver()
    {
                    
        GameObject.Find("Main Camera").GetComponent<CameraControllerNew>().enabled = false;
        gameOver.Setup(points);
    }
    public void Phase()
    {
        if(transform.position.z >= 185 && transform.position.z <= 186 && phase == 1)
        {
            transitionAnim.SetTrigger("goblins");
            music.clip = musicGoblin;
            music.Play();
            phase = 2;
        }
        else if(transform.position.z >= 369 && transform.position.z <= 370 && phase == 2)
        {
            transitionAnim.SetTrigger("castle");
            music.clip=musicStronghold;
            music.Play(0);
            phase = 3;
        }
        else if(transform.position.z >= 183 && transform.position.z <= 184 && phase == 2)
        {
            transitionAnim.SetTrigger("spiders");
            music.clip = musicForest;
            music.Play(0);
            phase = 1;
        }
        else if (transform.position.z >= 367 && transform.position.z <= 368 && phase == 3)
        {
            transitionAnim.SetTrigger("goblins");
            music.clip = musicGoblin;
            music.Play();
            phase = 2;
        }

    }


}
