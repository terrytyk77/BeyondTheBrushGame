﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : MonoBehaviour
{
    //Buffs variables||

    //_______________||


    [System.Serializable]
    public class treeNode
    {
        public string id;
        public string description;
        public string name;

        public treeNode(string _id, string _description, string _name)
        {
            id = _id; description = _description; name = _name;
        }
    }

    //Static data for the nodes of the tree
    //this is the information which is displayed
    //to the player when browsing around the talent tree

    //ID
    //DESCRIPTION
    //NAME

    public static treeNode node0 = new treeNode(
        /*ID*/ "node0", /*ID*/
        /*Description*/ "Basic spell increases your movement speed by 10% for 6 seconds. Not stackable.", /*Description*/
        /*Name*/ "Flash Strike" /*Name*/
    );

    public static treeNode node1 = new treeNode(
        /*ID*/ "node1", /*ID*/
        /*Description*/ "The basic spell reduces the cooldown of itself by 0.5 seconds for 6 seconds, stackable up to 1 second.", /*Description*/
        /*Name*/ "To Arms" /*Name*/
    );

    public static treeNode node2 = new treeNode(
        /*ID*/ "node2", /*ID*/
        /*Description*/ "Xspell is now able to break heavier objects just like stones and iron.", /*Description*/
        /*Name*/ "Steel Chopper" /*Name*/
    );

    public static treeNode node3 = new treeNode(
        /*ID*/ "node3", /*ID*/ 
        /*Description*/ "Get 2 stacks for the defensive spell.", /*Description*/ 
        /*Name*/ "Shield Block" /*Name*/
    );

    public static treeNode node4 = new treeNode(
        /*ID*/ "node4", /*ID*/ 
        /*Description*/ "Executing an enemy with your Xspell recovers 20% of your HP.", /*Description*/ 
        /*Name*/ "Battle thirst" /*Name*/
    );

    public static treeNode node5 = new treeNode(
        /*ID*/ "node5", /*ID*/ 
        /*Description*/ "You'll get 15% more resources from the dungeon.", /*Description*/ 
        /*Name*/ "Greed is Good" /*Name*/
    );

    public static treeNode node6 = new treeNode(
        /*ID*/ "node6", /*ID*/ 
        /*Description*/ "The players encounters 25% more chests within the dungeon.", /*Description*/ 
        /*Name*/ "Gold Digger" /*Name*/
    );

    public static treeNode node7 = new treeNode(
        /*ID*/ "node7", /*ID*/
        /*Description*/ "Spawns a bubble blocking the first attack against the player.", /*Description*/ 
        /*Name*/ "Bubble Up" /*Name*/
    );

    public static treeNode node8 = new treeNode(
        /*ID*/ "node8", /*Name*/
        /*Description*/ "Every time the player uses 2 consecutive basic attacks, reduce the cooldown of your Xspell by 0.5 seconds.", /*Description*/ 
        /*Name*/ "Demand for Action" /*Name*/
    );

    public static treeNode node9 = new treeNode(
        /*ID*/ "node9", /*ID*/ 
        /*Description*/ "Reduce debuffs on the player by 25% [Passive]", /*Description*/
        /*Name*/ "Tough Skin" /*Name*/
    );

    public static treeNode node10 = new treeNode(
        /*ID*/ "node10", /*ID*/ 
        /*Description*/ "Xspell resets after executing an enemy.", /*Description*/ 
        /*Name*/ "Overkill" /*Name*/
    );

}
