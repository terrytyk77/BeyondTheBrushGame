using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPCsystem : MonoBehaviour
{
    //Variables||

        private bool isInRange = false;             //Weather the player is in range or not
        private GameObject interectionDisplay;      //Stored reference of the NPC canvas
        private float requiredHoldTime = 2f;        //Amount of time required to open the chat
        private float currentHold = 0f;             //The current amount of holding that was given
        private KeyCode interectionKey = KeyCode.E; //The key that is used to activate the interection
        private bool showingDialog = false;         //Weather the chat has already been open or not
        private float maximiumSize = 1.2f;          //Maximium Size of the circle on hold
    //_________||

    private void Start()
    {
        interectionDisplay = gameObject.transform.Find("Canvas").gameObject;    //Store the canvas element
        interectionDisplay.SetActive(false);                                    //Hide it from the player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isInRange){           //Check if the element in collision is the player
            isInRange = true;                                                   //Give permission to the fixed update
            interectionDisplay.SetActive(true);                                 //Display the interection circle
            interectionDisplay.transform.transform.Find("Circle").
            localScale = new Vector3(0, 0, 1);                                  //Make the button size = 0
            StartCoroutine(popCircle(true));                                    //Make the circle button pop up
            currentHold = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            isInRange = false;                                              //Stop the rendering from the fixede update
            StartCoroutine(popCircle(false));                               //Make the circle button pop out
        }
            
    }


    private void FixedUpdate()
    {
        Transform circleElement = interectionDisplay.transform.Find("Circle");              //Get the circle element

        if (isInRange && Input.GetKey(interectionKey)){
            if(currentHold + Time.fixedDeltaTime >= requiredHoldTime && !showingDialog)   //Check if the user already holded for enough time
            {
                currentHold = requiredHoldTime; //Fix the time display
                showingDialog = true;           //Tell the holder that the dialog is currently open
                openDialog();                   //Open the dialog method
            }
            else if(currentHold < requiredHoldTime)
            {
                StopAllCoroutines();                                                    //Stop all animations
                circleElement.rotation *= Quaternion.Euler(0, 0, 0.5f);                 //Add rotation to the circle
                circleElement.Find("Text").rotation *= Quaternion.Euler(0, 0, -0.5f);   //Negate this rotation on the text
                currentHold += Time.fixedDeltaTime;                                     //Add the holding amount of time
                float amountToAdd = (currentHold * (maximiumSize - 1)) / requiredHoldTime;

                if (circleElement.localScale.x < maximiumSize)
                    circleElement.localScale = new Vector3(1 + amountToAdd, 1 + amountToAdd, 1);
                else
                    circleElement.localScale = new Vector3(maximiumSize, maximiumSize, maximiumSize);           //Hold this size
            }else{
                currentHold = requiredHoldTime;
            }
                
        }else if(isInRange)
        {
            if(currentHold > 0)
            {
                currentHold -= Time.fixedDeltaTime;
                float amountToAdd = (currentHold * (maximiumSize - 1)) / requiredHoldTime;
                circleElement.rotation *= Quaternion.Euler(0, 0, -0.5f);                 //Add rotation to the circle
                circleElement.Find("Text").rotation *= Quaternion.Euler(0, 0, +0.5f);   //Negate this rotation on the text
                if (circleElement.localScale.x > 1)
                    circleElement.localScale = new Vector3(1 + amountToAdd, 1 + amountToAdd, 1);
                else
                    circleElement.localScale = new Vector3(1, 1, maximiumSize);           //Hold this size
            }
            else{
                if(circleElement.localScale.x > 1)
                {
                    circleElement.localScale = new Vector3(1, 1, maximiumSize);           //Hold this size
                }

                currentHold = 0;                                                            //Reset the holding amount of time
                circleElement.rotation = Quaternion.Euler(0, 0, 0);                         //Reset the rotation from the circle
                circleElement.Find("Text").rotation = Quaternion.Euler(0, 0, 0);            //Reset the rotation from the text
            }

        }

        circleElement.GetComponent<Image>().fillAmount = currentHold / requiredHoldTime;  //Change the circle fill
    }

    private void openDialog(){
        Debug.Log("A dialog was open");
    }


    private IEnumerator popCircle(bool direction){

        float forceSpeed = 0.175f;

        //If the direction = true then pop up
        //If the direction = false then pop out
        if (!direction)
            forceSpeed *= -1;

        Transform circleToChange = interectionDisplay.transform.Find("Circle"); //Get the circle element

        void resizeAction()
        {
            float oldValue = circleToChange.localScale.x;
            if(oldValue + forceSpeed > 1)
                circleToChange.localScale = new Vector3(1, 1, 1);
            else if(oldValue + forceSpeed < 0)
                circleToChange.localScale = new Vector3(0, 0, 1);
            else
                circleToChange.localScale = new Vector3(oldValue + forceSpeed, oldValue + forceSpeed, 1);
        }


        if (direction)
            while(circleToChange.localScale.x < 1){ resizeAction(); yield return new WaitForSeconds(0.05f);}
        else
            while (circleToChange.localScale.x > 0) { resizeAction(); yield return new WaitForSeconds(0.05f); }


        yield return null;
    }

}
