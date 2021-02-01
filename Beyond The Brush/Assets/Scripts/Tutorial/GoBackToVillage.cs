using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToVillage : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){

            PlayerData.tutorial = true;
            sceneTeleport.start(1);
            
        }
    }

}
