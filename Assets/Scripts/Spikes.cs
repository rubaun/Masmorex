using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private AudioSource m_Source;
    private BoxCollider m_Collider;
    [SerializeField]
    private AudioClip hitting;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject spike;
    [SerializeField]
    private bool turnOn;

    private void Start()
    {
        this.AddComponent<AudioSource>();
        m_Source = GetComponent<AudioSource>();
        m_Source.rolloffMode = AudioRolloffMode.Linear;
        m_Source.maxDistance = 10.0f;
        m_Source.minDistance = 2.5f;
        m_Source.spatialBlend = 1.0f;
        StartCoroutine(StartSpike());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Damage()
    {
        return damage;
    }

    IEnumerator StartSpike()
    {
        yield return new WaitForSeconds(3.0f);

        m_Source.PlayOneShot(hitting, 1.0f);

        if (turnOn && spike.transform.position.y <= 0)
        {
            spike.transform.localPosition = new Vector3(0, 0, 0);

            turnOn = false;
        }
        else if(!turnOn && spike.transform.position.y >= -2.0f)
        {
            spike.transform.localPosition = new Vector3(0, -2, 0);

            turnOn = true;
        }

        StartCoroutine(StartSpike());
    }
}
