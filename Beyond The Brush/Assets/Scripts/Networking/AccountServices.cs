using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountServices : MonoBehaviour
{

    //Variables||

        //Signup
        public InputField usernameSignup;
        public InputField emailSignup;
        public InputField passwordSignup;

        //Login
        public InputField usernameLogin;
        public InputField passwordLogin;

        //Text limits
        public int usernameMaxChar = 15;
        public int usernameMinChar = 4;
    //_________||

    //Data format
    private class FormatedData{

        public string name;
        public string email;
        public string password;

    }


    public void SignupAcccount()
    {
        //Check if the data format is accepted||

        //If not accepted
        bool accepted = true;
        string errorMessage = "";

        //name
        if (usernameSignup.text != null)
        {
            if (usernameSignup.text.Length < usernameMinChar && usernameSignup.text.Length > usernameMaxChar)
            {
                accepted = false;
                errorMessage = "username has incorrect name size";
            }
        }
        else
        {
            //Name was not valid
            accepted = false;
            errorMessage = "the username tpye is not valid";
        }

        //email
        //add for the other data types
        //____________________________________||


        //Check if there was any problem with the user input
        if (accepted)
        {
            //Get the data formated
            FormatedData fmdt = new FormatedData
            {
                name = usernameSignup.text,
                email = emailSignup.text,
                password = passwordSignup.text
            };

            //Ask the server for a responce
            StartCoroutine("SendLoginData", fmdt);

        }
        else
        {
            //Tell what the problem was with the user input
            Debug.Log(errorMessage);
        }

    }

    private IEnumerator SendLoginData(FormatedData data)
    {
        
        //Set the cookie
        WebServices.CookieString = null;

        //Call the server
        var request = WebServices.Post("account/signup", JsonUtility.ToJson(data));

        //Make the code wait until the server responds
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            //Check if there was a network error
            Debug.LogError(request.error);
        }
        else
        {
            //Get the header for proccessing
            WebServices.CookieString = request.GetResponseHeader("set-cookie");

            //The server response
            string result = request.downloadHandler.text;
            Debug.Log(result);

        }

    }

}
