using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : MonoBehaviour
{
    //Variables||

        //Database data
        static private string _id = "";
        static private string _username = "Offline Player";
        static private int _level = 1;
        static private int _exp = 0;
        static private int _resources = 0;
        static private int _gold = 0;

        //local data
        static private int _healthPoints = 100;
        static private int _maxHealthPoints = 100;
        static private float _slashCooldown = 10f;
    //_________||

    //Get the database data
    public static string id {get{return _id;} set { _id = value; } }
    public static string username { get { return _username; } set { _username = value; } }
    public static int level { get { return _level; } set { _level = value; } }
    public static int exp { get { return _exp; } set { _exp = value; } }
    public static int resources { get { return _resources; } set { _resources = value; } }
    public static int gold { get { return _gold; } set { _gold = value; } }

    //Get the local data
    public static int healthPoints { get { return _healthPoints; } set { _healthPoints = value; } }
    public static int maxHealthPoints { get { return _maxHealthPoints; } set { _maxHealthPoints = value; } }
    public static float slashCooldown { get { return _slashCooldown; } set { _slashCooldown = value; } }

    static public void SetPlayerData(string id, string username, int level, int exp, int resources, int gold)
    {
        _id = id;
        _username = username;
        _level = level;
        _exp = exp;
        _resources = resources;
        _gold = gold;

        //Calculate the max health
        _healthPoints = _maxHealthPoints;
    }

}
