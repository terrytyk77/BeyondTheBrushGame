using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRegion : MonoBehaviour
{
    //Variables||

        RegionalSystem regionalSystem;
    //_________||

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.name != regionalSystem.currentArea && collision.gameObject.CompareTag("Player")){ 
            regionalSystem.currentArea = gameObject.name;
            regionalSystem.changedArea();
        }
            
    }

    // Start is called before the first frame update
    void Start()
    {
        regionalSystem = GameObject.FindGameObjectWithTag("mainUI").GetComponent<RegionalSystem>();
    }


}
