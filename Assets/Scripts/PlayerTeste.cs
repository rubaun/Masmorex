using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeste : MonoBehaviour
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
    void Update()
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
            acionar = true;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", true);
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest") && other.GetComponent<OpenChest>())
        {
            Debug.Log("Colide com Baú");
            isInteracting = true;
            

            if (isInteracting && acionar)
            {
                other.GetComponent<OpenChest>().OpenChestDoor();

                foreach (GameObject item in other.GetComponent<OpenChest>().ItensInside())
                {
                    inventory.Add(item);
                }

                other.GetComponent<OpenChest>().ChestClean();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInteracting = false;
        acionar = false;
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
}
