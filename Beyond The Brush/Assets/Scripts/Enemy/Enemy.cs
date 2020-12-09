using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void damage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Killed!");
        }
    }

}
