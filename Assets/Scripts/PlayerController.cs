using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private AudioSource player;
    private bool isPaused;
    private bool isAlive;
    private bool animaDeath;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private GameObject imageKey;
    [SerializeField]
    private TextMeshProUGUI coins;
    [SerializeField]
    private AudioClip passo;
    [SerializeField]
    private AudioClip attack;
    [SerializeField] 
    private AudioClip jump;
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
        player = GetComponent<AudioSource>();
        angleRotation = new Vector3(0, 90, 0);
        playerLife = 100;
        startSpeed = speed;
        imageKey.SetActive(false);
        isPaused = false;
        isAlive = true;
        animaDeath = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WalkMobo();

        //Andar 
        Walk();

        //Rotacionar
        Rotate();

        AnimatePlayer();

        SpeedRun();
        //Joystick1Button0 => A
        //Movimento e Animação do Pulo
        if (Input.GetKey(KeyCode.Space) && isOnGround || Input.GetKey(KeyCode.Joystick1Button0) && isOnGround)
        {
            JumpMove();
            JumpAnimation();
            if (!player.isPlaying)
            {
                player.PlayOneShot(jump);
            }
        }
        //Joystick1Button0 => X
        //Interagir
        if (Input.GetKey(KeyCode.E) && isInteracting || Input.GetKey(KeyCode.Joystick1Button1) && isInteracting)
        {
            playerAnimator.SetTrigger("Interact");
            InteractToChest();
            InteractToDoor();
        }
        //Joystick1Button1 => B
        //Atacar
        if (Input.GetMouseButtonDown(0) && isAttacking || Input.GetKey(KeyCode.Joystick1Button2) && isAttacking)
        {
            Attack();
            if (!player.isPlaying)
            {
                player.PlayOneShot(attack, 0.5f);
            }
        }

        coins.text = goldPlayer.ToString();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isAlive)
        {
            SceneManager.LoadScene("Title");
        }

        if (playerLife <= 0)
        {
            isAlive = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAnimator.SetBool("IsOnGround", true);
            isOnGround = true;
        }

        //if (collision.gameObject.CompareTag("Spikes"))
        //{
        //    Debug.Log("Colide com spike");
        //    Spikes actualSpike = collision.gameObject.GetComponent<Spikes>();
        //    int damage = actualSpike.Damage();
        //    Debug.LogFormat("Dano {0}", damage.ToString());
        //    playerLife -= actualSpike.Damage();
        //}
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

        if (other.CompareTag("Spikes"))
        {
            Debug.Log("Colide com spike");
            Spikes actualSpike = other.GetComponent<Spikes>();
            int damage = actualSpike.Damage();
            Debug.LogFormat("Dano {0}", damage.ToString());
            playerLife -= actualSpike.Damage();
        }
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
                    if(item.tag == "Key")
                    {
                        imageKey.SetActive(true);
                    }
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
                        imageKey.SetActive(false);
                    }
                }

                door.DoorOpen();
            }
        }
    }

    private void Attack()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit) && breakObj)
        {   
            playerAnimator.SetTrigger("Attack");
            breakObj.LifeBreak(10);
        }
    }

    private void Walk()
    {
        if(isAlive)
        {
            float fowardInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * fowardInput;
            Vector3 moveFoward = rb.position + moveDirection * speed * Time.deltaTime;
            rb.MovePosition(moveFoward);
        }
        
    }

    private void Rotate()
    {
        if (isAlive)
        {
            float sideInput = Input.GetAxis("Horizontal");
            Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    private void AnimatePlayer()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0)
        {
            playerAnimator.SetBool("Walk", true);
            if(!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.2f);
            }
            

            if (Input.GetKey(KeyCode.W) && playerAnimator.GetBool("WalkBack"))
            {
                playerAnimator.SetBool("WalkToBack", true);
                if (!player.isPlaying)
                {
                    player.PlayOneShot(passo, 0.2f);
                }
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < 0)
        {
            playerAnimator.SetBool("WalkBack", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.2f);
            }

            if (Input.GetKey(KeyCode.S) && playerAnimator.GetBool("Walk"))
            {
                playerAnimator.SetBool("WalkToBack", false);
                if (!player.isPlaying)
                {
                    player.PlayOneShot(passo, 0.2f);
                }
            }
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            playerAnimator.SetBool("Walk", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.2f);
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
        {
            playerAnimator.SetBool("Walk", true);
            if (!player.isPlaying)
            {
                player.PlayOneShot(passo, 0.2f);
            }
        }
        else
        {
            playerAnimator.SetBool("Walk", false);
            playerAnimator.SetBool("WalkBack", false);
            playerAnimator.SetBool("WalkToBack", false);

        }

        if(!isAlive && animaDeath)
        {
            playerAnimator.SetTrigger("Death");
            animaDeath = false;
            deathScreen.SetActive(true);
        }
    }

    private void SpeedRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8;
        }
        else
        {
            speed = startSpeed;
        }
    }

    public bool PlayerIsAlive()
    {
        return isAlive;
    }

    public int PlayerLife()
    {
        return playerLife;
    }

    public void WalkMobo()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Moved)
            {
                // Obtém a posição do toque anterior e atual.
                Vector2 posicaoAnterior = toque.position - toque.deltaPosition;
                Vector3 direcao = new Vector3(toque.deltaPosition.x, 0, toque.deltaPosition.y);

                // Converte a direção para a orientação do mundo.
                direcao = Camera.main.transform.TransformDirection(direcao);
                direcao.y = 0; // Mantém a movimentação no plano XZ.

                // Move o personagem na direção determinada pelo toque.
                transform.Translate(direcao * 2.0f * Time.deltaTime);
            }

        }
    }
}

