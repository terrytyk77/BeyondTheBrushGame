﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        changeArmorDrawing();
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

    public void changeHorizontalDirection(bool direction)
    {

        //true is for going to the right
        //false is for going to the left
        if (direction)
        {
            //Go to the right
            horizontalPlayer.transform.Find("Head").Find("RightDrawing").GetComponent<SpriteRenderer>().enabled = false;
            horizontalPlayer.transform.Find("Head").Find("LeftDrawing").GetComponent<SpriteRenderer>().enabled = true;

            horizontalPlayer.transform.Find("Body").Find("RightDrawing").GetComponent<SpriteRenderer>().enabled = false;
            horizontalPlayer.transform.Find("Body").Find("LeftDrawing").GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            //Go to the left
            horizontalPlayer.transform.Find("Head").Find("RightDrawing").GetComponent<SpriteRenderer>().enabled = true;
            horizontalPlayer.transform.Find("Head").Find("LeftDrawing").GetComponent<SpriteRenderer>().enabled = false;

            horizontalPlayer.transform.Find("Body").Find("RightDrawing").GetComponent<SpriteRenderer>().enabled = true;
            horizontalPlayer.transform.Find("Body").Find("LeftDrawing").GetComponent<SpriteRenderer>().enabled = false;
        }

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

            //Handle images showing||

                //Display helmet images
                verticalPlayer.transform.Find("Head").Find("FrontDrawing").GetComponent<SpriteRenderer>().enabled = false;
                verticalPlayer.transform.Find("Head").Find("BackDrawing").GetComponent<SpriteRenderer>().enabled = true;
                
                //Display vest images
                verticalPlayer.transform.Find("Body").Find("FrontDrawing").GetComponent<SpriteRenderer>().enabled = false;
                verticalPlayer.transform.Find("Body").Find("BackDrawing").GetComponent<SpriteRenderer>().enabled = true;
            //_____________________||
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

            //Handle images showing||

                //Display helmet images
                verticalPlayer.transform.Find("Head").Find("FrontDrawing").GetComponent<SpriteRenderer>().enabled = true;
                verticalPlayer.transform.Find("Head").Find("BackDrawing").GetComponent<SpriteRenderer>().enabled = false;

                //Display vest images
                verticalPlayer.transform.Find("Body").Find("FrontDrawing").GetComponent<SpriteRenderer>().enabled = true;
                verticalPlayer.transform.Find("Body").Find("BackDrawing").GetComponent<SpriteRenderer>().enabled = false;
            //_____________________||

        }

    }

    public void changeArmorDrawing()
    {
        //See weather there is a profile to load or not
        bool? hasDrawing = true;

        //Check if the profile is a default one
        if (PlayerData.currentProfile < 2)
            hasDrawing = null;

        accountInfoResponse.profilesData currentProfile = new accountInfoResponse.profilesData();
        if (hasDrawing != null)
        {
           //Get the current profile
           currentProfile = PlayerData.playerProfiles[PlayerData.currentProfile - 2];
        }

        List<Texture2D> textureHolders = new List<Texture2D>();

        void changeDrawingVertical(string bodyPart, string drawingName, string imageString)
        {

            if (hasDrawing != null && imageString != null)
            {

                //Add a new one
                textureHolders.Add(new Texture2D(2 + textureHolders.Count, 2 + textureHolders.Count));
                Texture2D textureHolder = textureHolders[textureHolders.Count - 1];

                textureHolder.LoadImage(Convert.FromBase64String(imageString));
                verticalPlayer.transform.Find(bodyPart).Find(drawingName).GetComponent<SpriteRenderer>().sprite = Sprite.Create(textureHolder, new Rect(0.0f, 0.0f, textureHolder.width, textureHolder.height), new Vector2(0.5f, 0.5f), 200f);
            }
            else
            {
                verticalPlayer.transform.Find(bodyPart).Find(drawingName).GetComponent<SpriteRenderer>().sprite = null;
            }

        }

        void changeDrawingHorizontal(string bodyPart, string drawingName, string imageString)
        {

            if (hasDrawing != null && imageString != null)
            {

                //Add a new one
                textureHolders.Add(new Texture2D(1, 1));
                Texture2D textureHolder = textureHolders[textureHolders.Count - 1];

                textureHolder.LoadImage(Convert.FromBase64String(imageString));
                horizontalPlayer.transform.Find(bodyPart).Find(drawingName).GetComponent<SpriteRenderer>().sprite = Sprite.Create(textureHolder, new Rect(0.0f, 0.0f, textureHolder.width, textureHolder.height), new Vector2(0.5f, 0.5f), 200f);
            }
            else
            {
                horizontalPlayer.transform.Find(bodyPart).Find(drawingName).GetComponent<SpriteRenderer>().sprite = null;
            }

        }

            //Set the helmet drawings||


                //back
                if (hasDrawing != null)
                    changeDrawingVertical(bodyPart: "Head", drawingName: "BackDrawing", imageString: currentProfile.back.Head);
                else
                    changeDrawingVertical(bodyPart: "Head", drawingName: "BackDrawing", imageString: null);

                //front
                if (hasDrawing != null)
                    changeDrawingVertical(bodyPart: "Head", drawingName: "FrontDrawing", imageString: currentProfile.front.Head);
                else
                    changeDrawingVertical(bodyPart: "Head", drawingName: "FrontDrawing", imageString: null);

                //right
                if (hasDrawing != null)
                    changeDrawingHorizontal(bodyPart: "Head", drawingName: "RightDrawing", imageString: currentProfile.right.Head);
                else
                    changeDrawingHorizontal(bodyPart: "Head", drawingName: "RightDrawing", imageString: null);

                //left
                if (hasDrawing != null)
                    changeDrawingHorizontal(bodyPart: "Head", drawingName: "LeftDrawing", imageString: currentProfile.left.Head);
                else
                    changeDrawingHorizontal(bodyPart: "Head", drawingName: "LeftDrawing", imageString: null);
            //_______________________||

            //Set the vest drawings||

                //front
                if (hasDrawing != null)
                    changeDrawingVertical(bodyPart: "Body", drawingName: "FrontDrawing", imageString: currentProfile.front.Chest);
                else
                    changeDrawingVertical(bodyPart: "Body", drawingName: "FrontDrawing", imageString: null);

                //back
                if (hasDrawing != null)
                    changeDrawingVertical(bodyPart: "Body", drawingName: "BackDrawing", imageString: currentProfile.back.Chest);
                else
                    changeDrawingVertical(bodyPart: "Body", drawingName: "BackDrawing", imageString: null);

                //right
                if (hasDrawing != null)
                    changeDrawingHorizontal(bodyPart: "Body", drawingName: "RightDrawing", imageString: currentProfile.right.Chest);
                else
                    changeDrawingHorizontal(bodyPart: "Body", drawingName: "RightDrawing", imageString: null);

                //left
                if (hasDrawing != null)
                    changeDrawingHorizontal(bodyPart: "Body", drawingName: "LeftDrawing", imageString: currentProfile.left.Chest);
                else
                    changeDrawingHorizontal(bodyPart: "Body", drawingName: "LeftDrawing", imageString: null);
        //_____________________||


    }

}
