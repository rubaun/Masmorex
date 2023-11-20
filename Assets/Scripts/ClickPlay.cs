using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPlay : MonoBehaviour
{
    private bool click;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StartPlay();
        }
    }


    public void StartPlay()
    {
        SceneManager.LoadScene("Stage1");
    }
}
