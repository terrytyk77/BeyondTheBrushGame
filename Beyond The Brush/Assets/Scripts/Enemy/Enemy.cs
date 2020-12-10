using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;
    public GameObject healthBar;

    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    public void damage(float damage)
    {
        health -= damage;
        healthBar.GetComponent<Image>().fillAmount -= damage / maxHealth;

        if (health <= 0)
        {
            // Delay Death For Animation
            Invoke("death", 0.2f);
        }
    }

    private void death()
    {
        Destroy(gameObject);
        Debug.Log("Enemy Killed!");
    }

}
