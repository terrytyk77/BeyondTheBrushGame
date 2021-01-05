using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorChange : MonoBehaviour
{

    //Variables||

        [System.Serializable]
        public class armorHolder
        {
            [System.Serializable]
            public class armorSet
            {
                public Sprite hat;
                public Sprite body_clothes;
                public Sprite right_arm;
                public Sprite left_arm;
                public Sprite shield;
                public Sprite right_shoe;
                public Sprite left_shoe;
                public Sprite sword;
                public Sprite hair;
            }

            public armorSet front;
            public armorSet side;
            public armorSet back;
        }

        //Hold the player armor sets
        public armorHolder default1;
        public armorHolder default2;

        //Hold the player stands
        public GameObject horizontalPlayer;
        public GameObject verticalPlayer;

        static public armorHolder currentDefault;
    //_________||


    // Start is called before the first frame update
    void Start()
    {
        //Call the function to handle the player default armor
        changeDefaultArmor();
    }


    public void changeDefaultArmor()
    {
        //Here it chooses which default profile should the player be using
        switch (PlayerData.currentProfile)
        {
            case 0:
                currentDefault = default1;
                changeClothing(default1);
                break;
            case 1:
                currentDefault = default2;
                changeClothing(default2);
                break;
            default:

                switch (PlayerData.playerProfiles[PlayerData.currentProfile - 2].preset.name)
                {
                    case 0:
                        currentDefault = default1;
                        changeClothing(default1);
                        break;
                    case 1:
                        currentDefault = default2;
                        changeClothing(default2);
                        break;
                }

                break;
        }
    }

    private void changeClothing(armorHolder armorSet)
    {
        //Setup the head/BodyClothes/Shield ||

            //Horizontal helm
            horizontalPlayer.transform.Find("Head").Find("Helm").GetComponent<SpriteRenderer>().sprite = armorSet.side.hat;

            //Horizontal body clothes
            horizontalPlayer.transform.Find("Body").Find("Vest").GetComponent<SpriteRenderer>().sprite = armorSet.side.body_clothes;

            //Vertical
            changedVerticalDirection(PlayerMovement.verticalDirection, armorSet);
        //_________________________________||

        //Change the weapons||

            //sword
            horizontalPlayer.transform.Find("Right Arm").Find("Sword").GetComponent<SpriteRenderer>().sprite = armorSet.side.sword;
            verticalPlayer.transform.Find("Right Arm").Find("Sword").GetComponent<SpriteRenderer>().sprite = armorSet.front.sword;

            //shield
            horizontalPlayer.transform.Find("Left Arm").Find("Shield").GetComponent<SpriteRenderer>().sprite = armorSet.side.shield;
        //__________________||

        //Arms and feet||

            horizontalPlayer.transform.Find("Left Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.side.left_shoe;
            horizontalPlayer.transform.Find("Right Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.side.right_shoe;
        //_____________||

        //Change the arms clothing||

            horizontalPlayer.transform.Find("Left Arm").Find("Armor").GetComponent<SpriteRenderer>().sprite = armorSet.front.left_arm;
            horizontalPlayer.transform.Find("Right Arm").Find("Armor").GetComponent<SpriteRenderer>().sprite = armorSet.front.right_arm;

            verticalPlayer.transform.Find("Left Arm").Find("Armor").GetComponent<SpriteRenderer>().sprite = armorSet.front.left_arm;
            verticalPlayer.transform.Find("Right Arm").Find("Armor").GetComponent<SpriteRenderer>().sprite = armorSet.front.right_arm;
        //________________________||

    }

    public void changedVerticalDirection(bool direction, armorHolder armorSet)
    {

        //true is going up meaning it will show the player back
        //false is going down which means the player will show it's front
        if (direction) 
        {
            //helm    
            verticalPlayer.transform.Find("Head").Find("Helm").GetComponent<SpriteRenderer>().sprite = armorSet.back.hat;

            //hide face
            verticalPlayer.transform.Find("Head").Find("Face").transform.localScale = new Vector3(0, 0, 0);

            //Change the hair direction
            verticalPlayer.transform.Find("Head").Find("Hair").GetComponent<SpriteRenderer>().sprite = armorSet.back.hair;

            //Change clothing direction
            verticalPlayer.transform.Find("Body").Find("Vest").GetComponent<SpriteRenderer>().sprite = armorSet.back.body_clothes;

            //shield
            horizontalPlayer.transform.Find("Left Arm").Find("Shield").GetComponent<SpriteRenderer>().sprite = armorSet.back.shield;

            //Boots
            verticalPlayer.transform.Find("Left Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.back.left_shoe;
            verticalPlayer.transform.Find("Right Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.back.right_shoe;
        }
        else
        {
            //helm
            verticalPlayer.transform.Find("Head").Find("Helm").GetComponent<SpriteRenderer>().sprite = armorSet.front.hat;

            //show face
            verticalPlayer.transform.Find("Head").Find("Face").transform.localScale = new Vector3(1, 1, 1);

            //Change the hair direction
            verticalPlayer.transform.Find("Head").Find("Hair").GetComponent<SpriteRenderer>().sprite = armorSet.front.hair;

            //Change clothing direction
            verticalPlayer.transform.Find("Body").Find("Vest").GetComponent<SpriteRenderer>().sprite = armorSet.front.body_clothes;

            //shield
            horizontalPlayer.transform.Find("Left Arm").Find("Shield").GetComponent<SpriteRenderer>().sprite = armorSet.front.shield;

            //Boots
            verticalPlayer.transform.Find("Left Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.front.left_shoe;
            verticalPlayer.transform.Find("Right Leg").Find("Boot").GetComponent<SpriteRenderer>().sprite = armorSet.front.right_shoe;

        }

    }

}
