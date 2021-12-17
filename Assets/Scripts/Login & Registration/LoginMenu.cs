using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;

    public Text errorText;

    public void CallLoginUser() {
        StartCoroutine(LoginUser());
    }

    IEnumerator LoginUser() {
        //Create form inputs for POST request
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        //Send POST request
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/LoginUser.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                var result = www.downloadHandler.text;
                //Display error (username does not exist or incorrect pw)
                if (result.Contains("Error:")) {
                    errorText.text = result;
                    errorText.gameObject.SetActive(true);
                } else {    //Username and password match
                    Debug.Log("Login successful.");
                    UserManager.userID = result;
                    UserManager.username = usernameField.text;
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    public void GoToRegisterScene() {
        SceneManager.LoadScene(1);
    }
}
