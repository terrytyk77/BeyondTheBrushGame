using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon
{

    //Variables||

        public string DungeonName;
        public int baseReward;

        public GameObject startingRoom;
        public List<room> rooms = new List<room>();
    //_________||


    public Dungeon(string name)
    {
        DungeonName = name;
    }

    [System.Serializable]
    public class room
    {
        //Variables||

            public string roomName = "room";
            public GameObject roomPrefab;
            public sides roomSides = new sides(false, false ,false ,false);
            public bool explored = false;
            public bool completed = false;
        //_________||

        [System.Serializable]
        public class sides
        {
            //Room sides||
                public bool top = false;
                public bool right = false;
                public bool left = false;
                public bool bottom = false;
            //__________||

            public sides(bool t, bool r, bool l, bool b)
            {
                top = t;
                right = r;
                left = l;
                bottom = b;
            }

        }





        public room()
        {

        }

        //Constructor to setup the room
        public room(string name, GameObject prefab, sides side)
        {
            roomName = name;
            roomPrefab = prefab;
            roomSides = side;
        }

    }

}

public class DungeonsAPI : MonoBehaviour
{
    public List<Dungeon> dungeons = new List<Dungeon>();
}
