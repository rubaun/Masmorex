using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody rb;
    private bool isOnGround;
    private bool isAttacking;
    private Vector3 angleRotation;
    private bool isInteracting;
    private bool acionar;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private List<GameObject> inventory = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        angleRotation = new Vector3(0, 90, 0);
        isInteracting = false;
        acionar = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Andar 
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveFoward = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(moveFoward);

        //Rotacionar
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        //Animação
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.SetBool("Walk", true);

            if (Input.GetKey(KeyCode.W) && playerAnimator.GetBool("WalkBack"))
            {
                playerAnimator.SetBool("WalkToBack", true);
            }
        }
        else if(Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("WalkBack", true);

            if (Input.GetKey(KeyCode.S) && playerAnimator.GetBool("Walk"))
            {
                playerAnimator.SetBool("WalkToBack", false);
            }
        }
        else if(Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else
        {
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetBool("WalkBack", false);
            playerAnimator.SetBool("WalkToBack", false);
            
        }

        if (Input.GetKey(KeyCode.E) && isInteracting)
        {
            playerAnimator.SetTrigger("Interact");
            acionar = true;
        }

        //Movimento e Animação do Pulo
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            JumpMove();
            JumpAnimation();
        }
        
        //Animação do ataque
        if (Input.GetMouseButton(0) && isAttacking)
        {
            //isAttacking = true;
            PlayerAttack();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", true);
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            Debug.Log("Interagindo com a porta");
            isInteracting = true;
        }

        if (collision.gameObject.CompareTag("Chest"))
        {
            Debug.Log("Interagindo com o baú");
            isInteracting = true;
        }

        if(collision.gameObject.CompareTag("Breakable"))
        {
            Debug.Log("Interagindo com objeto quebrável");
            isAttacking = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            OpenChest openChest = other.GetComponent<OpenChest>();
            Debug.Log("Colide com Baú");

            isInteracting = true;
            Debug.LogFormat("isInteracting {0}",isInteracting);

            if (isInteracting && acionar)
            {
                openChest.OpenChestDoor();

                foreach(GameObject item in openChest.ItensInside())
                {
                    inventory.Add(item);
                }

                openChest.ChestClean();
            }

            isInteracting = false;
            acionar = false;
        }

        if(other.CompareTag("Door"))
        {
            OpenDoor openDoor = other.GetComponent<OpenDoor>();
            Debug.Log("Colide Porta");
            if(isInteracting)
            {
                Debug.Log("Tentando abrir porta");
                openDoor.DoorOpen();

                if(openDoor.DoorIsLocked())
                {
                    Debug.Log("Porta Trancada");
                    foreach(GameObject item in inventory)
                    {
                        Debug.Log("Varrendo Inventário");
                        if (openDoor.HasKey(item.name))
                        {
                            Debug.Log("Destrancando Porta");
                            openDoor.UnlockDoor();
                        }
                    }
                    
                    openDoor.DoorOpen();
                }
            }

            isInteracting = false;
        }

        if(other.CompareTag("Breakable"))
        {
            Breakable breakable = other.GetComponent<Breakable>();
            Debug.Log("Colide Quebrável");

            if(isAttacking)
            {
                breakable.LifeBreak(10);

                if(breakable.GetLifeBreak() <= 0)
                {
                    breakable.Break();
                }
            }

            isAttacking = false;
        }
    }

    private void JumpMove()
    {
        isOnGround = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void JumpAnimation()
    {
        playerAnimator.SetTrigger("Jumping");
        playerAnimator.SetBool("IsOnGround", false);
    }

    private void PlayerAttack()
    {
        playerAnimator.SetTrigger("Attack");
    }
}
