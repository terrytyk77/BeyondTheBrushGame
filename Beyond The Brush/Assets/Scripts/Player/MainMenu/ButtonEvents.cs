using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour
{
    //Variables||

        //Notifaction
        public GameObject notifactionError;

        //UI windows
        public GameObject confirmationWindow;
        public GameObject loginForm;
        public GameObject signupForm;
        public GameObject loadingScreen;

        public GameObject background;
        public GameObject loadingBar;

        public GameObject createAccountLabel;

        //The current index for the game build
        public int MainGame = 0;
    //_________||

    public void CloseErrorNotification()
    {
        //Close the window
        notifactionError.SetActive(false);
    }

public void OpenNotification()
    {
        //This is gonna show up the confirmation box
        confirmationWindow.SetActive(!confirmationWindow.activeSelf);
    }

    public void StartOffline()
    {
        //Start the scene loading
        sceneTeleport.start(3);
    }

    public void ChangeCurrentWindow()
    {
        //Get the current window state
        bool onLogin = loginForm.activeSelf;

        if (onLogin)
        {
            //In case the user is on the login window
            loginForm.SetActive(false);
            signupForm.SetActive(true);

            createAccountLabel.GetComponent<Text>().text = "Already have an account?";
        }
        else
        {
            //In case the user is on the signup window
            loginForm.SetActive(true);
            signupForm.SetActive(false);

            createAccountLabel.GetComponent<Text>().text = "Create Account";
        }

    }

    public void closeGame()
    {
        Application.Quit();
    }

    public void CancelOffline()
    {
        //This closes the window
        confirmationWindow.SetActive(false);
    }

    //Navigation variables||

        private GameObject lastSelectedObject = null;
    //____________________||

    private void Update()
    {
        //Store the next UI element
        Selectable next = null;
        bool elementExists = (EventSystem.current.currentSelectedGameObject != null);

        //Choose the direction for the action
        if (lastSelectedObject != null && ((Input.GetKeyDown(KeyCode.Tab) && elementExists) || (Input.GetKeyDown(KeyCode.DownArrow) && elementExists)) && elementExists)
            next = lastSelectedObject.GetComponent<Selectable>().navigation.selectOnDown;
        else if (lastSelectedObject != null && Input.GetKeyDown(KeyCode.UpArrow) && elementExists)
            next = lastSelectedObject.GetComponent<Selectable>().navigation.selectOnUp;
        else if ((Input.GetKeyDown(KeyCode.Tab) && elementExists) || (Input.GetKeyDown(KeyCode.DownArrow) && elementExists))
            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().navigation.selectOnDown;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && elementExists)
            next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().navigation.selectOnUp;

        //Go to the next navigation object
        if (next != null)
        {
            //Check if the gameobjecet has an input field
            Selectable inputfield = next.GetComponent<Selectable>();
            Button buttonInput = next.GetComponent<Button>();

            if (buttonInput != null)
                lastSelectedObject = next.gameObject;
            else
                lastSelectedObject = null;

            if (inputfield != null)
                inputfield.OnPointerDown(new PointerEventData(EventSystem.current));

            //Change the current focus of the navigation
            EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
        }

    }

}

