using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon
{

    //All the script classes||
        [System.Serializable]
        public class StartingSides  //Holds the starting room directions
        {
            //Room sides||
                public bool top = false;
                public bool right = false;
                public bool left = false;
                public bool bottom = false;
            //__________||
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||\\
        //||||||||||||||||||||||||||||||||||||||||||||||||||| This is a class breaker|||||||||||||||||||||||||||||||||||||||||||||||||||
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||//
        [System.Serializable]
        public class room
        {
            //Variables||

            public string roomName = "room";
            public GameObject roomPrefab;
            public sides roomSides = new sides(false, false, false, false);
            private bool explored = false;
            private bool completed = false;
            //_________||

            public bool getExplored()
            {
                return explored;
            }
            public void setExplored(bool state)
            {
                explored = state;
            }

            public void setCompleted(bool state)
            {
                completed = state;
            }

            public bool getCompleted()
            {
                return completed;
            }

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
            public room() { }

            //Constructor to setup the room
            public room(string name, GameObject prefab, sides side)
            {
                roomName = name;
                roomPrefab = prefab;
                roomSides = side;
            }

            public room(string name, GameObject prefab, sides side, bool completed2, bool unexplored)
            {
                roomName = name;
                roomPrefab = prefab;
                roomSides = side;
                completed = completed2;
                explored = unexplored;
            }

        }
    //______________________||

    //Variables||

        public string DungeonName;                  //The dungeons name
        public int baseReward;                      //The base reward of the rooms
        public AudioClip mainMusic;                 //The base music of the dungeon
        public GameObject startingRoom;             //The starting room prefab
        public StartingSides startingRoomSides;     //The starting sides of the starting room
        public List<room> rooms = new List<room>(); //The list of rooms of the layout
    //_________||



}

//Create the list of dungeons to use on the editor
public class DungeonsAPI : MonoBehaviour
{
    public List<Dungeon> dungeons = new List<Dungeon>();
}
