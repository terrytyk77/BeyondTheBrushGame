using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;

    private GameObject player;
    private float speed = 10f;
    private float autoDestroyDistance = 0.1f;
    private bool destDebounce;
    private GameObject targetCreated;
    Vector3 destination;

    // Update is called once per frame
    void FixedUpdate()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (!destDebounce)
        {
            destDebounce = true;
            destination = player.transform.position;
            targetCreated = Instantiate(target, destination, Quaternion.identity);
        }

        Vector3 moveDir = (destination - transform.position).normalized;

        transform.position += moveDir * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, destination) < autoDestroyDistance)
        {
            Destroy(gameObject);
            Destroy(targetCreated);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DrawingCollider")
        {
            PlayerData.healthPoints -= 10;
        }
    }
}
