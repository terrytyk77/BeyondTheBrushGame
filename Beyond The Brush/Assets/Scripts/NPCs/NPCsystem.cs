using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NPCsystem : MonoBehaviour
{
    //Dialog structure||

        [System.Serializable]
        public class dialogShape{
            public string id;
            public string message;

            public dialogShape(string i, string m){
                id = i;
                message = m;
            }
        }
    //________________||

    //Handle events||

        public delegate void EndedConversation(string id);
        public static event EndedConversation convChanged;
    //_____________||

    //Variables||

        private bool isInRange = false;             //Weather the player is in range or not
        private GameObject interectionDisplay;      //Stored reference of the NPC canvas
        private float requiredHoldTime = 1.2f;      //Amount of time required to open the chat
        private float currentHold = 0f;             //The current amount of holding that was given
        private KeyCode interectionKey = KeyCode.E; //The key that is used to activate the interection
        private bool showingDialog = false;         //Weather the chat has already been open or not
        private float maximiumSize = 1.2f;          //Maximium Size of the circle on hold
        private GameObject dialogBox;               //Holds the dialog box for the NPCs

        private bool waitingForUserInput = false;   //It's waiting for the user to press something
        private int currentMessage = -1;            //The current NPC message that we're on

        private Vector2 markerStartingPos;          //Store the starting position of the marker
    //_________||

    //Editable variables||

        [Header("npcs details")]
        public string name = "";
        public string startingMessage = "";
        public List<dialogShape> dialogMessages;
    //__________________||

    private void Awake()
    {
        interectionDisplay = gameObject.transform.Find("Canvas").gameObject;                            //Store the canvas element
        interectionDisplay.SetActive(false);                                                            //Hide it from the player
        dialogBox = GameObject.FindGameObjectWithTag("mainUI").transform.Find("DialogBox").gameObject;  //Store the dialog box
        markerStartingPos = dialogBox.transform.Find("Marker").transform.localPosition;                 //Store the marker position
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


    private void Update()
    {

        if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {    
            if(waitingForUserInput && dialogBox.GetComponent<CanvasGroup>().alpha == 1)
            {
                Image markerComponent = dialogBox.transform.Find("Marker").GetComponent<Image>();
                markerComponent.color = new Color(255, 255, 255, 0);
                currentMessage++;                           //Move to the next index
                StopAllCoroutines();                        //Reset the coroutines to stop the text
                handleConversation();                       //Continue the conversation if possible
            }
        }

        Transform circleElement = interectionDisplay.transform.Find("Circle");              //Get the circle element

        if (isInRange && Input.GetKey(interectionKey) && dialogBox.GetComponent<CanvasGroup>().alpha == 0)
        {
            if(currentHold + Time.deltaTime >= requiredHoldTime && !showingDialog && dialogBox.GetComponent<CanvasGroup>().alpha == 0)   //Check if the user already holded for enough time
            {
                StartNPCdialog();
            }
            else if(currentHold < requiredHoldTime)
            {
                StopAllCoroutines();                                                    //Stop all animations
                circleElement.rotation *= Quaternion.Euler(0, 0, 0.5f);                 //Add rotation to the circle
                circleElement.Find("Text").rotation *= Quaternion.Euler(0, 0, -0.5f);   //Negate this rotation on the text
                currentHold += Time.deltaTime;                                     //Add the holding amount of time
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
                currentHold -= Time.deltaTime;
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

    public void StartNPCdialog(){
        currentHold = requiredHoldTime; //Fix the time display
        showingDialog = true;           //Tell the holder that the dialog is currently open
        openDialog();                   //Open the dialog method
    }

    private void openDialog(){
        GameObject.FindGameObjectWithTag("Player")
                  .GetComponent<Rigidbody2D>()
                  .constraints = RigidbodyConstraints2D.FreezeAll;                  //Freeze the player as he speaks to the NPC

        dialogBox.transform.Find("NamePlate").GetComponent<Image>().fillAmount = 0; //Reset the name plate fill
        dialogBox.transform.Find("MainFrame").GetComponent<Image>().fillAmount = 0; //Reset the main frame fill
        dialogBox.GetComponent<CanvasGroup>().alpha = 1;                            //Turn the box visible
        dialogBox.transform.Find("NamePlate").Find("npcName").GetComponent<Text>().text = "";
        dialogBox.transform.Find("MainFrame").Find("npcMessage").GetComponent<Text>().text = "";
        StartCoroutine(paintDialog());                                              //Start the painting animation
    }

    private IEnumerator paintDialog(){                                                  //Dynamacly draw the dialog box

        Image mainPlate = dialogBox.transform.Find("MainFrame").GetComponent<Image>();  //Hold the main frame image
        Image namePlate = dialogBox.transform.Find("NamePlate").GetComponent<Image>();  //Hold the name place image

        while (mainPlate.fillAmount < 1){                                               //Check if the fill has already complete
            mainPlate.fillAmount += 0.035f;                                              //Add more fill into the image
            yield return new WaitForSeconds(0.005f);                                    //Wait for x amount of time
        }

        while(namePlate.fillAmount < 1){                                                //Check if the fill has already complete
            namePlate.fillAmount += 0.035f;                                              //Add more fill into the image
            yield return new WaitForSeconds(0.005f);                                    //Wait for x amount of time
        }


        StartCoroutine(writeNameplate());                                               //Write the name of the NPC
        yield return null;                                                              //End the coroutine
    }

    private IEnumerator writeNameplate(){
        Text npcName = dialogBox.transform.Find("NamePlate")        //Get the text componenet of the nameplate
                                          .Find("npcName")
                                          .GetComponent<Text>();

        while(npcName.text.Length < name.Length){                   //Check if the text is already complete or not
            npcName.text += name[npcName.text.Length];              //Add a letter to the text
            yield return new WaitForSeconds(0.075f);                //yield for x amount of time
        }

        handleConversation();                                       //Handle the following conversation

        yield return null;
    }

    private void handleConversation(){

        if(currentMessage == -1)
        {
            waitingForUserInput = true;                     //The code can now expect user input
            StartCoroutine(writeMessage(startingMessage));  //Start the chat
        }else if(currentMessage < dialogMessages.Count)
        {

            if (convChanged != null && dialogMessages.Count > 0 && currentMessage != 0)
                convChanged(dialogMessages[currentMessage-1].id);                   //Trigger the event
            StartCoroutine(writeMessage(dialogMessages[currentMessage].message));  //Start the chat
        }else{
            if (convChanged != null && dialogMessages.Count > 0)
                convChanged(dialogMessages[dialogMessages.Count - 1].id);                        //Trigger the event
            conversationEnded();                                                    //Reset everything
        }
    }

    private void conversationEnded(){
        showingDialog = false;                      //Tell the code that the dialog is no longer being shown
        waitingForUserInput = false;                //No longer listening to user input
        currentMessage = -1;                        //Reset the current message index
        GameObject.FindGameObjectWithTag("Player")  //Unfreeze the player
          .GetComponent<Rigidbody2D>()
          .constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(dialogFadeAway());           //Make the dialog go away
    }

    IEnumerator dialogFadeAway(){
        CanvasGroup boxCanvas = dialogBox.GetComponent<CanvasGroup>();

        while(boxCanvas.alpha > 0){
            boxCanvas.alpha -= 0.075f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return null;
    }

    private IEnumerator writeMessage(string message){

        Text npcMessage = dialogBox.transform.Find("MainFrame")     //Get the text component of the NPC message
                                     .Find("npcMessage")
                                     .GetComponent<Text>();

        npcMessage.text = "";                                       //Reset the text message

        while(npcMessage.text.Length < message.Length){
            npcMessage.text += message[npcMessage.text.Length];     //Add a character to the message
            yield return new WaitForSeconds(0.025f);                //Yield for x amount of time
        }

        StartCoroutine(markerMovement());                           //Start the market movement
        yield return null;
    }

    private IEnumerator markerMovement(){

        Image markerComponent = dialogBox.transform.Find("Marker").GetComponent<Image>();
        markerComponent.color = new Color(255, 255, 255, 255);
        bool direction = false; //False for down, true for up

        while(true){
            if(direction){
                markerComponent.transform.localPosition += new Vector3(0, 0.375f, 0);
                if (markerComponent.transform.localPosition.y > markerStartingPos.y + 6)
                    direction = false;

                yield return new WaitForSeconds(0.025f);
            }
            else{
                markerComponent.transform.localPosition -= new Vector3(0, 0.375f, 0);

                if (markerComponent.transform.localPosition.y < markerStartingPos.y - 6)
                    direction = true;

                yield return new WaitForSeconds(0.025f);
            }
        }

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
