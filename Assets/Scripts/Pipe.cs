using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //destroy pipe after getting out of screen
        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }
}
