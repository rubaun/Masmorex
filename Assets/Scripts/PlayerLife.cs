using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> hearts = new List<GameObject>();
    private int nHearts;
    private float vHearts;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        nHearts = hearts.Count;
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        vHearts = player.PlayerLife() / nHearts;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.PlayerLife() <= 65)
        {
            hearts[0].SetActive(false);
        }

        if (player.PlayerLife() <= 35)
        {
            hearts[1].SetActive(false);
        }

        if (player.PlayerLife() <= 0)
        {
            hearts[2].SetActive(false);
        }
    }
}
