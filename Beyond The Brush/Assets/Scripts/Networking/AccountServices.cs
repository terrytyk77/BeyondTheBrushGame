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

        //server routes
        public string signupRoute;
        public string loginRoute;

        //Text limits
        public int usernameMaxChar = 15;
        public int usernameMinChar = 4;

        public int emailMinChar = 1;
        public int emailMaxChar = 30;

        public int passwordMinChar = 8;
        public int passwordMaxChar = 15;
    //_________||

    //Data format
    private class FormatedData{

        public string name;
        public string email;
        public string password;

    }

    public void LoginAccount()
    {

        //Get the data formated
        FormatedData fmdt = new FormatedData
        {
            name = usernameLogin.text,
            password = passwordLogin.text
        };

        //Ask the server for a responce
        StartCoroutine("SendLoginRequest", fmdt);

    }


    private IEnumerator SendLoginRequest(FormatedData data)
    {

        //Set the cookie
        WebServices.CookieString = null;

        //Call the server
        var request = WebServices.Post(loginRoute, JsonUtility.ToJson(data));

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

            //TO DO
            Debug.Log(result);

        }

    }

    public void SignupAccount()
    {

        //Check if the data format is accepted||

            //If not accepted
            bool accepted = true;
            string errorMessage = "";

            void dataError(string msg)
            {
                accepted = false;
                errorMessage = msg;
            }


            //name
            if (usernameSignup.text != null)
            {
                if (usernameSignup.text.Length < usernameMinChar || usernameSignup.text.Length > usernameMaxChar)
                    dataError("The username has an invalid size");
            }
            else
                dataError("The username has an invalid data type");

            //email
            if (emailSignup.text != null)
            {
                if (emailSignup.text.Length < emailMinChar || emailSignup.text.Length > emailMaxChar)
                    dataError("The email has an invalid size");
            }
            else
                dataError("The email has an invalid data type");

            //password
            if (passwordSignup.text != null)
            {
                if (passwordSignup.text.Length < passwordMinChar || passwordSignup.text.Length > passwordMaxChar)
                    dataError("The password has an invalid size");
            }
            else
                dataError("The password has an invalid data type");
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
            StartCoroutine("SendSignupRequest", fmdt);

        }
        else
        {
            //Tell what the problem was with the user input
            Debug.Log(errorMessage);
        }

    }

    private IEnumerator SendSignupRequest(FormatedData data)
    {
        
        //Set the cookie
        WebServices.CookieString = null;

        //Call the server
        var request = WebServices.Post(signupRoute, JsonUtility.ToJson(data));

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

            //TO DO
            Debug.Log(result);

        }

    }

}
