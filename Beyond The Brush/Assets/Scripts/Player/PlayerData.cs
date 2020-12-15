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
        static private string _email;

        //local data
        static private int _healthPoints = 100;
        static private int _maxHealthPoints = 100;

        //Cooldowns
        static public float slashCooldownDefault = 0.5f;
        static public float xslashCooldownDefault = 5f;
        static public float shieldCooldownDefault = 6f;

        private class cooldowsClass{
            public float _slashCooldown = 0;
            public float _xslashCooldown = 0;
            public float _shieldCooldown = 0;
        }

        static private cooldowsClass cooldowns = new cooldowsClass();

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

    //Cooldowns
    public static float slashCooldown { get { return cooldowns._slashCooldown; } set { cooldowns._slashCooldown = value; } }
    public static float xslashCooldown { get { return cooldowns._xslashCooldown; } set { cooldowns._xslashCooldown = value; } }
    public static float shieldCooldown { get { return cooldowns._shieldCooldown; } set { cooldowns._shieldCooldown = value; } }

    public static int getNeededExp()
    {
        int result = level * 10;

        return result;
    }

    public static void resetCooldowns()
    {
        //Slash
        if (cooldowns._slashCooldown > 0)
            cooldowns._slashCooldown -= Time.deltaTime;
        else
            cooldowns._slashCooldown = 0;

        //XSlash
        if (cooldowns._xslashCooldown > 0)
            cooldowns._xslashCooldown -= Time.deltaTime;
        else
            cooldowns._xslashCooldown = 0;

        //Shield
        if (cooldowns._shieldCooldown > 0)
            cooldowns._shieldCooldown -= Time.deltaTime;
        else
            cooldowns._shieldCooldown = 0;
    }

    static public void SetPlayerData(accountInfoResponse json)
    {
        _id = json.body._id;
        _username = json.body.name;
        _level = json.body.stats.level;
        _exp = json.body.stats.exp;
        _resources = json.body.stats.ressources;
        _gold = json.body.stats.gold;
        _email = json.body.email;

        //Calculate the max health
        _healthPoints = _maxHealthPoints;
    }

}
