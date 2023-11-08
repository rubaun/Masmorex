using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    private AudioSource m_Source;
    [SerializeField]
    private AudioClip openning;
    [SerializeField]
    private GameObject chestUp;
    [SerializeField]
    private List<GameObject> itens = new List<GameObject>();
    private Animator chestAnim;
    [SerializeField]
    private bool open = false;
    [SerializeField]
    private int goldChest;

    private void Awake()
    {
        if(goldChest == 0)
        {
            goldChest = Random.Range(10, 100);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        chestAnim = chestUp.GetComponent<Animator>();
        this.AddComponent<AudioSource>();
        m_Source = GetComponent<AudioSource>();
    }

    public void OpenChestDoor()
    {
        if(!open)
        {
            chestAnim.SetBool("OpenChest", true);
            if (!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(openning, 1.0f);
            }
            Debug.Log("Abrindo Baú");
            open = true;
        }
        else if(open)
        {
            chestAnim.SetBool("OpenChest", false);
            if (!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(openning, 1.0f);
            }
            Debug.Log("Fechando Baú");
            open = false;
        }

        

    }

    public List<GameObject> ItensInside()
    {
        return itens;
    }

    public void ChestClean()
    {
        itens.Clear();
        goldChest = 0;
    }

    public int ChestGold()
    {
        return goldChest;
    }

    public bool ChestOpened()
    {
        return open;
    }
}
