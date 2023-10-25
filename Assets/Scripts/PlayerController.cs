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
    private OpenChest chest;
    private OpenDoor door;
    private Breakable breakObj;
    private float startSpeed;
    [SerializeField]
    private int goldPlayer;
    [SerializeField]
    private bool isInteracting;
    [SerializeField]
    private int playerLife;
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
        playerLife = 100;
        startSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Andar 
        Walk();

        //Rotacionar
        Rotate();

        AnimatePlayer();

        SpeedRun();

        //Movimento e Animação do Pulo
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            JumpMove();
            JumpAnimation();
        }

        //Interagir
        if (Input.GetKey(KeyCode.E) && isInteracting)
        {
            playerAnimator.SetTrigger("Interact");
            InteractToChest();
            InteractToDoor();
        }

        //Atacar
        if (Input.GetMouseButtonDown(0) && isAttacking)
        {
            Attack();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", true);
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Spikes"))
        {
            Debug.Log("Colide com spike");
            Spikes actualSpike = collision.gameObject.GetComponent<Spikes>();
            int damage = actualSpike.Damage();
            Debug.LogFormat("Dano {0}", damage.ToString());
            playerLife -= actualSpike.Damage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            Debug.Log("Colide com Baú");
            OpenChest actualChest = other.GetComponent<OpenChest>();
            chest = actualChest;
            isInteracting = true;
        }

        if (other.CompareTag("Door"))
        {
            Debug.Log("Colide com Porta");
            OpenDoor actualDoor = other.GetComponent<OpenDoor>();
            door = actualDoor;
            isInteracting = true;
        }

        if (other.CompareTag("Breakable"))
        {
            Debug.Log("Colide com Quebrável");
            Breakable actualBreakable = other.GetComponent<Breakable>();
            breakObj = actualBreakable;
            isAttacking = true;
        }

        //if (other.CompareTag("Spikes"))
        //{
        //    Debug.Log("Colide com spike");
        //    Spikes actualSpike = other.GetComponent<Spikes>();
        //    int damage = actualSpike.Damage();
        //    Debug.LogFormat("Dano {0}", damage.ToString());
        //    playerLife -= actualSpike.Damage();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            OpenChest actualChest = other.GetComponent<OpenChest>();
            Debug.Log("Saindo do Baú");
            if (chest == actualChest)
            {
                chest = null;
                isInteracting = false;
            }
        }

        if (other.CompareTag("Door"))
        {
            OpenDoor actualDoor = other.GetComponent<OpenDoor>();
            Debug.Log("Saindo da Porta");
            if (door == actualDoor)
            {
                door = null;
                isInteracting = false;
            }
        }

        if (other.CompareTag("Breakable"))
        {
            Breakable actualBreakable = other.GetComponent<Breakable>();
            Debug.Log("Saindo do Quebrável");
            if (breakObj == actualBreakable)
            {
                breakObj = null;
                isInteracting = false;
            }
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

    private void InteractToChest()
    {
        if (chest)
        {
            chest.OpenChestDoor();

            if (chest.ChestOpened())
            {
                foreach (GameObject item in chest.ItensInside())
                {
                    inventory.Add(item);
                }

                goldPlayer += chest.ChestGold();
            }

            chest.ChestClean();
        }

    }

    private void InteractToDoor()
    {
        if (door)
        {
            Debug.Log("Tentando abrir porta");
            door.DoorOpen();

            if (door.DoorIsLocked())
            {
                Debug.Log("Porta Trancada");
                foreach (GameObject item in inventory)
                {
                    Debug.Log("Varrendo Inventário");
                    if (door.HasKey(item.name))
                    {
                        Debug.Log("Destrancando Porta");
                        door.UnlockDoor();
                        inventory.Remove(item);
                    }
                }

                door.DoorOpen();
            }
        }
    }

    private void Attack()
    {
        playerAnimator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit) && breakObj)
        {
            breakObj.LifeBreak(10);
        }
    }

    private void Walk()
    {
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveFoward = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(moveFoward);
    }

    private void Rotate()
    {
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void AnimatePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerAnimator.SetBool("Walk", true);

            if (Input.GetKey(KeyCode.W) && playerAnimator.GetBool("WalkBack"))
            {
                playerAnimator.SetBool("WalkToBack", true);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerAnimator.SetBool("WalkBack", true);

            if (Input.GetKey(KeyCode.S) && playerAnimator.GetBool("Walk"))
            {
                playerAnimator.SetBool("WalkToBack", false);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerAnimator.SetBool("Walk", true);
        }
        else
        {
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetBool("WalkBack", false);
            playerAnimator.SetBool("WalkToBack", false);

        }
    }

    private void SpeedRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10;
        }
        else
        {
            speed = startSpeed;
        }
    }
}
