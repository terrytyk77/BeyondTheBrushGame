using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public GameObject doorToOpen;
    public GameObject symbole;

    private SpriteRenderer Trigger;
    private SpriteRenderer Symbole;

    public Sprite TriggerIn;
    public Sprite TriggerOut;

    public Sprite[] ArraySymboleIn;
    public Sprite[] ArraySymboleOut;
    //Add All the Prefab Name by same order of Array Symbole
    private string[] ArrayPrefabs = new string[] {"Stone(Clone)", "Crate(Clone)"};

    private int isEmpty;
    private int randomSymbole;
    private Sprite symboleIn;
    private Sprite symboleOut;

    private void Start()
    {
        Trigger = gameObject.GetComponent<SpriteRenderer>();
        Symbole = symbole.GetComponent<SpriteRenderer>();

        // Pick Randomly The Drawing Symbole!
        randomSymbole = Random.Range(0, ArraySymboleIn.Length);

        symboleIn = ArraySymboleIn[randomSymbole];
        symboleOut = ArraySymboleOut[randomSymbole];

        //Initial State
        Trigger.sprite = TriggerOut;
        Symbole.sprite = symboleOut;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == ArrayPrefabs[randomSymbole])
        {
            doorToOpen.GetComponent<LockedDoor>().DestroyDoor();
            Trigger.sprite = TriggerIn;
            Symbole.sprite = symboleIn;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == ArrayPrefabs[randomSymbole])
        {
            Trigger.sprite = TriggerOut;
            Symbole.sprite = symboleOut;
        }
    }
}
