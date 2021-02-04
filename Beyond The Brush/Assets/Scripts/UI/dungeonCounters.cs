using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dungeonCounters : MonoBehaviour
{

    //GameObject references||

        public GameObject chestLabel;
        public GameObject roomLabel;
        public GameObject deathLabel;
    //_____________________||

    //Text references||

        private Text chestsText;
        private Text roomsText;
        private Text deathsText;
    //_______________||

    //Store positions||

        private Vector2 chestsPos;
        private Vector2 roomsPos;
        private Vector2 deathsPos;
    //_______________||

    //Coroutines||

        private IEnumerator chestsCoroutine;
        private IEnumerator roomsCoroutine;
        private IEnumerator deathsCoroutine;
    //__________||

    private void Start()
    {
        //Add listeners to the add events
        CurrentDungeonData.onChestCollect += addedChestAnim;
        CurrentDungeonData.onRoomComplete += addedRoomAnim;
        CurrentDungeonData.onDeath += addedDeathAnim;

        //Get the text componenets of the labels
        chestsText = chestLabel.GetComponent<Text>();
        roomsText = roomLabel.GetComponent<Text>();
        deathsText = deathLabel.GetComponent<Text>();

        //Resets the numbers
        chestsText.text = "0";
        roomsText.text = "0";
        deathsText.text = "0";

        //Collect the positions
        chestsPos = chestLabel.transform.position;
        roomsPos = roomLabel.transform.position;
        deathsPos = deathLabel.transform.position;
    }

    private void addedChestAnim(int amount){
        if (chestsCoroutine != null)
            StopCoroutine(chestsCoroutine);

        chestLabel.transform.position = chestsPos;
        chestsCoroutine = addAnimation(chestLabel, chestsPos, amount.ToString());
        StartCoroutine(chestsCoroutine);
    }

    private void addedDeathAnim(int amount){
        if (deathsCoroutine != null)
            StopCoroutine(deathsCoroutine);

        deathLabel.transform.position = deathsPos;
        deathsCoroutine = addAnimation(deathLabel, deathsPos, amount.ToString());
        StartCoroutine(deathsCoroutine);
    }

    private void addedRoomAnim(int amount){
        if (roomsCoroutine != null)
            StopCoroutine(roomsCoroutine);

        roomLabel.transform.position = roomsPos;
        roomsCoroutine = addAnimation(roomLabel, roomsPos, amount.ToString());
        StartCoroutine(roomsCoroutine);
    }

    private IEnumerator addAnimation(GameObject labelElement, Vector2 lastPosition, string newValue){

        while(labelElement.transform.position.y < lastPosition.y + 10)
        {
            labelElement.transform.position += new Vector3(0, 1, 0);
            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitForSeconds(.5f);

        while (labelElement.transform.position.y > lastPosition.y)
        {
            labelElement.transform.position -= new Vector3(0, 1, 0);
            yield return new WaitForSeconds(.02f);
        }

        labelElement.transform.position = lastPosition;

        labelElement.GetComponent<Text>().text = newValue;

        yield return null;
    }

}
