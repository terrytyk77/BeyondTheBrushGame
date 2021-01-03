using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passives : MonoBehaviour
{
    private float initialMovementSpeed;
    private float initialSlashDefaultCooldown;

    //FlashStrike        ||
    [Header("FlashStrike Settings:")]
        public float FlashStrikeMovementSpeedIncrease = 10;
        public float FlashStrikeTimer = 6f;
        public int FlashStrikeMaxStack = 1;
        private float FlashStrikeTick = 0f;
        private int FlashStrikeCurrentStack = 0;
        private bool FlashStrikeInUse = false;
    //-------------------||

    //FlashStrike       ||
    [Header("ToArms Settings:")]
        public float ToArmsReduceSlashTimer = 0.5f;
        public float ToArmsTimer = 6f;
        public int ToArmsMaxStack = 1;
        private float ToArmsTick = 0f;
        private int ToArmsCurrentStack = 0;
        private bool ToArmsInUse = false;
    //-------------------||

    //BattleThrist       ||
    [Header("BattleThrist Settings:")]
        public int BattleThristHealingPercentage = 10;
    //-------------------||

    //DemandForAction    ||
    [Header("DemandForAction Settings:")]
        public int DemandForActionConsecutiveHits = 2;
        public float DemandForActionXSpellTimeReducion = 1f;
        private int DemandForActionSlashCounter = 0;
    //-------------------||

    //ShieldBlock                  ||
    [Header("ShieldBlock Settings:")]
        public GameObject ShieldBubble;
        public int ShieldBlockMaxStack = 2;
    //-----------------------------||

    //BubbleUpS
    [Header("BubbleUp Settings:")]
        public int BubbleUpDamageBlock = 100;
        public Color32 BubbleUpShieldColor;
    //-----------------------------||

    private void Start()
    {
        initialMovementSpeed = PlayerData.movementSpeed;
        initialSlashDefaultCooldown = PlayerData.slashCooldownDefault;
    }

    private void Update()
    {

        //FlashStrike Timer and Resetter||
        if (FlashStrikeTick < FlashStrikeTimer && FlashStrikeInUse == true)
        {
            FlashStrikeTick += Time.deltaTime;
        }
        else
        {
            PlayerData.movementSpeed = initialMovementSpeed;
            FlashStrikeTick = 0f;
            FlashStrikeCurrentStack = 0;
            FlashStrikeInUse = false;
        }
        //-----------------------------||

        //ToArms Timer and Resetter||
        if (ToArmsTick < ToArmsTimer && ToArmsInUse == true)
        {
            ToArmsTick += Time.deltaTime;
        }
        else
        {
            PlayerData.slashCooldownDefault = initialSlashDefaultCooldown;
            ToArmsTick = 0f;
            ToArmsCurrentStack = 0;
            ToArmsInUse = false;
        }
        //-----------------------------||

        //Shield                       ||
        if (PlayerData.isShielded == true)
        {
            ShieldBubble.SetActive(true);
        }
        else
        {
            ShieldBubble.SetActive(false);
        }
        //-----------------------------||

        ShieldBlock();
        BubbleUp();
    }

    public void FlashStrike()
    {
        if (PlayerData.talentTreeData.node0 == true)
        {
            FlashStrikeTick = 0f;
            FlashStrikeInUse = true;
            if (FlashStrikeCurrentStack < FlashStrikeMaxStack)
            {
                PlayerData.movementSpeed += initialMovementSpeed * FlashStrikeMovementSpeedIncrease / 100;
                FlashStrikeCurrentStack++;
            }
        }
    }

    public void ToArms()
    {
        if (PlayerData.talentTreeData.node1 == true)
        {
            ToArmsTick = 0f;
            ToArmsInUse = true;
            if (ToArmsCurrentStack < ToArmsMaxStack)
            {
                PlayerData.slashCooldown = PlayerData.slashCooldownDefault;
                PlayerData.slashCooldownDefault -= ToArmsReduceSlashTimer;
                ToArmsCurrentStack++;
            }
            else
            {
                PlayerData.slashCooldown = PlayerData.slashCooldownDefault;
            }
        }
    }

    public void BattleThrist()
    {
        if (PlayerData.talentTreeData.node4 == true)
        {
            PlayerData.healthPoints += PlayerData.maxHealthPoints * BattleThristHealingPercentage / 100;
            if (PlayerData.healthPoints > PlayerData.maxHealthPoints)
            {
                PlayerData.healthPoints = PlayerData.maxHealthPoints;
            }
        }
    }

    public void DemandForAction(int damage)
    {
        if (PlayerData.talentTreeData.node8 == true)
        {
            if (damage == PlayerData.slashDamage)
            {
                DemandForActionSlashCounter++;
            }
            else
            {
                DemandForActionSlashCounter = 0;
            }

            if (DemandForActionSlashCounter >= DemandForActionConsecutiveHits)
            {
                if (PlayerData.xslashCooldown > DemandForActionXSpellTimeReducion)
                {
                    PlayerData.xslashCooldown -= DemandForActionXSpellTimeReducion;
                }
                else
                {
                    PlayerData.xslashCooldown = 0;
                }

                DemandForActionSlashCounter = 0;
            }
        }
    }

    public void Overkill()
    {
        if (PlayerData.talentTreeData.node10 == true)
        {
            PlayerData.xslashCooldown = 0f;
        }
    }

    public void ShieldBlock()
    {
        if(PlayerData.talentTreeData.node3 == true)
        {
            PlayerData.shieldMaxStack = ShieldBlockMaxStack;
        }
    }

    public void BubbleUp()
    {
        if(PlayerData.talentTreeData.node7 == true)
        {
            ShieldBubble.GetComponent<SpriteRenderer>().color = BubbleUpShieldColor;
            PlayerData.shieldDamageReduction = BubbleUpDamageBlock;
        }
    }


}
