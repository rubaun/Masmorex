using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    private int life;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LifeBreak(int hit)
    {
        life -= hit;

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
