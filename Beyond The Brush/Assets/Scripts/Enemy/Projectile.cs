using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject player;
    private float speed = 10f;
    private float autoDestroyDistance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 moveDir = (player.transform.position - transform.position).normalized;

        transform.position += moveDir * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, player.transform.position) < autoDestroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
