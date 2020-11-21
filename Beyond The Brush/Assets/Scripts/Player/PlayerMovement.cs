using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float xMove = Input.GetAxis("Horizontal");

        if(xMove != 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xMove, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }

    }
}
