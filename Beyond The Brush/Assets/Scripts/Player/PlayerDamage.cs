using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile" || collision.tag == "MeleeDamage")
        {
            collision.gameObject.GetComponentInParent<EnemyAI>().dealDamage();

            if(collision.tag == "Projectile")
            {
                collision.gameObject.GetComponent<Projectile>().DestroyProjectile();
            }
        }
    }
}
