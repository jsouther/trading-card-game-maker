using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterMenu : MonoBehaviour
{
    public static int USERNAME_LENGTH = 3;
    public static int PASSWORD_LENGTH = 5;

    public InputField usernameField;
    public InputField passwordField;
    public InputField confirmPasswordField;

    public Button registerButton;

    public Text errorText;

    public void CallCreateUser() {
        StartCoroutine(CreateUser());
    }

    IEnumerator CreateUser() {
        //Create form inputs for POST request
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        //Send POST request
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/RegisterUser.php", form)) {
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                var result = www.downloadHandler.text;

                //User registered to database, navigate to main menu
                if (result == "Success.") {
                    Debug.Log("Registration form upload complete.");
                    SceneManager.LoadScene(0);
                } else {  //Username is taken, database has not been updated
                    errorText.text = result;
                    errorText.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ValidateInputs() {
        string username = usernameField.text;
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        bool passwordsMatch = true;
        bool fieldsAreNotBlank = true;
        bool userIsLongEnough = true;
        bool passIsLongEnough = true;

        errorText.text = "";

        if (password != confirmPassword) {
            errorText.text = "Passwords must match!";
            passwordsMatch = false;
        }

        else if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password)) {
            errorText.text = "Entries must not be blank or only whitespace.";
            fieldsAreNotBlank = false;
        }

        else if (username.Length < USERNAME_LENGTH) {
            errorText.text = "Username must be at least " + USERNAME_LENGTH + " characters.";
            userIsLongEnough = false;
        }

        else if (password.Length < PASSWORD_LENGTH) {
            errorText.text = "Password must be at least " + PASSWORD_LENGTH + " characters.";
            passIsLongEnough = false;
        }

        errorText.gameObject.SetActive(!passwordsMatch || !fieldsAreNotBlank || !userIsLongEnough || !passIsLongEnough);

        registerButton.interactable = (
            username.Length >= USERNAME_LENGTH
            && !string.IsNullOrWhiteSpace(username)
            && password.Length >= PASSWORD_LENGTH
            && !string.IsNullOrEmpty(password)
            && password == confirmPassword
        );
    }

    public void GoToLoginScene() {
        SceneManager.LoadScene(0);
    }

    
}
