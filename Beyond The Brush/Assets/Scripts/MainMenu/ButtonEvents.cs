using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        sceneTeleport.start(1);
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

    public void CancelOffline()
    {
        //This closes the window
        confirmationWindow.SetActive(false);
    }

}

