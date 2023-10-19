using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject spike;
    [SerializeField]
    private bool turnOn;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(StartSpike());
    }

    public int Damage()
    {
        return damage;
    }

    IEnumerator StartSpike()
    {
        if (turnOn && spike.transform.position.y < 0)
        {
            spike.transform.position = spike.transform.position + new Vector3(0, speed * 1 * Time.deltaTime, 0);
            
            yield return new WaitForSeconds(1f);
            
            turnOn = false;
        }

        else if(!turnOn && spike.transform.position.y > -2.0f)
        {
            spike.transform.position = spike.transform.position + new Vector3(0, -2 * Time.deltaTime, 0) ;

            yield return new WaitForSeconds(5f);

            turnOn = true;
        }
        
    }
}
