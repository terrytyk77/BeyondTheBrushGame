using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public GameObject healthBar;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void damage(float damage)
    {
        currentHealth -= damage;
        healthBar.GetComponent<Image>().fillAmount -= damage / maxHealth;

        if (currentHealth <= 0)
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
