using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private AudioSource m_Source;
    [SerializeField]
    private AudioClip hitting;
    [SerializeField]
    private int life;

    // Start is called before the first frame update
    void Start()
    {
        this.AddComponent<AudioSource>();
        m_Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LifeBreak(int hit)
    {
        life -= hit;
        if (!m_Source.isPlaying)
        {
            m_Source.PlayOneShot(hitting, 1.5f);
        }

        if (life <= 0)
        {
            Break();
        }
    }

    public int GetLifeBreak()
    {
        return life;
    }

    public void Break()
    {
        Destroy(this.gameObject);
    }
}
