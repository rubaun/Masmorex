using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSmall : MonoBehaviour
{
    private BoxCollider axeCollider;
    private string tagName;

    // Start is called before the first frame update
    void Start()
    {
        axeCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        tagName = collision.gameObject.tag;
    }

    public string ObjectCollision()
    {
        return tagName;
    }
}
