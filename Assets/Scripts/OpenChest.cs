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
    private bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        chestAnim = chestUp.GetComponent<Animator>();
    }

    public void OpenChestDoor()
    {
        Debug.Log("Abrindo Baú");

        if(open)
        {
            chestAnim.SetBool("OpenChest", true);
        }
        else
        {
            chestAnim.SetBool("OpenChest", false);
        }

        open = !open;

    }

    public List<GameObject> ItensInside()
    {
        return itens;
    }

    public void ChestClean()
    {
        itens.Clear();
    }
}
