using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private BoxCollider weaponCollider;
    private string tagName;
    private string objName;

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        tagName = collision.gameObject.tag;
        objName = collision.gameObject.name;
    }

    public string ObjectCollision()
    {
        return tagName;
    }

    public string NameObjectCollision()
    {
        return objName;
    }
}
