using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class playerData
{
    //Set the variables||

    public string id;
    public string username;
    public int level;
    public int exp;
    public int resources;
    public int gold;
    //_________________||

}

public class PlayerData : MonoBehaviour
{
    //Variables||

    public playerData playerInfo;
    //_________||

    // Start is called before the first frame update
    void Start()
    {
        //Start the player
        playerInfo = new playerData();
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerData(string id, string username, int level, int exp, int resources, int gold)
    {
        //Set the player data to the class

        //unique data
        playerInfo.id = id;
        playerInfo.username = username;

        //levels
        playerInfo.level = level;
        playerInfo.exp = exp;

        //currencies
        playerInfo.resources = resources;
        playerInfo.gold = gold;
    }



}
