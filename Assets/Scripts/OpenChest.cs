using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
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
    }

    public void OpenChestDoor()
    {
        if(!open)
        {
            chestAnim.SetBool("OpenChest", true);
            Debug.Log("Abrindo Baú");
            open = true;
        }
        else if(open)
        {
            chestAnim.SetBool("OpenChest", false);
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
