using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private AudioSource m_Source;
    [SerializeField]
    private AudioClip openning;
    [SerializeField]
    private AudioClip locked;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private bool hasKey;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    private string nameKey;
    private Animator doorAnim;
    [SerializeField]
    private bool open = false;


    // Start is called before the first frame update
    void Start()
    {
        doorAnim = door.GetComponent<Animator>();
        this.AddComponent<AudioSource>();
        m_Source = GetComponent<AudioSource>();
    }

    public void DoorOpen()
    {
        if (!open && isLocked)
        {
            doorAnim.SetTrigger("OpenDoor");
            doorAnim.SetBool("DoorOpen", true);
            if (!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(locked, 0.3f);
            }
            Debug.Log("Porta Trancada");
        }
        else if (!open && !isLocked)
        {
            doorAnim.SetBool("HasKey", true);
            doorAnim.SetTrigger("OpenDoor");
            doorAnim.SetBool("DoorOpen", true);
            if(!m_Source.isPlaying)
            {
                m_Source.PlayOneShot(openning, 0.3f);
            }
            Debug.Log("Abrindo Fechada");
        }
        else if(open)
        {
            doorAnim.SetBool("DoorOpen", false);
            doorAnim.SetTrigger("OpenDoor");
            Debug.Log("Porta Aberta");
        }

        open = !open;
    }

    public bool HasKey(string name)
    {
        if(nameKey.Equals(name))
        {
            hasKey = true;
            return true;
        }
        
        return false;
    }

    public bool DoorIsLocked()
    {
        if(isLocked)
        {
            return true;
        }

        return false;
    }

    public void UnlockDoor()
    {
        if(isLocked)
        {
            isLocked = false;
        }
    }
}
