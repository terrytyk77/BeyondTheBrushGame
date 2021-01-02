using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour
{
    private float initialMovementSpeed;

    //FlashStrike         ||
    [Header("FlashStrike Settings:")]
        public float flashStrikeMovementSpeedIncrease = 10;
        public float flashStrikeTimer = 6f;
        public int flashStrikeMaxStack = 1;
    [Space(20)]
        private float flashStrikeTick = 0f;
        private int flashStrikeCurrentStack = 0;
        private bool flashStrikeInUse = false;
    //-------------------||

    private void Start()
    {
        initialMovementSpeed = PlayerData.movementSpeed;
    }

    private void Update()
    {

        //FlashStrike Timer and Resetter||
            if (flashStrikeTick < flashStrikeTimer && flashStrikeInUse == true)
            {
                flashStrikeTick += Time.deltaTime;
            }
            else
            {
                PlayerData.movementSpeed = initialMovementSpeed;
                flashStrikeTick = 0f;
                flashStrikeCurrentStack = 0;
                flashStrikeInUse = false;

            }
        //-----------------------------||


    }

    public void FlashStrike()
    {
        if (PlayerData.talentTreeData.node0 == true)
        {
            flashStrikeTick = 0f;
            flashStrikeInUse = true;
            if(flashStrikeCurrentStack < flashStrikeMaxStack)
            {
                PlayerData.movementSpeed += initialMovementSpeed * flashStrikeMovementSpeedIncrease / 100;
                flashStrikeCurrentStack++;
            }
        }
    }
}
