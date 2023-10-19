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
    }

    public bool ChestOpened()
    {
        return open;
    }
}
