using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDiscord : MonoBehaviour
{
    //Variables||

    public GameObject discordPresence;
    //_________||

    // Start is called before the first frame update
    void Start()
    {
        GameObject findDiscordPresence = GameObject.Find("Presence Manager");
        if(findDiscordPresence == null)
        {
            GameObject discordPresence2 = Instantiate(discordPresence);
            discordPresence2.name = "Presence Manager";
        }
    }
}
