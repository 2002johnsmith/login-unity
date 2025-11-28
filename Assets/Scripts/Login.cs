using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public string url = "http://localhost/login3.php";
    public InputField emailInputField;
    public InputField passwordInputField;
    public Button loginButton;
    [SerializeField] LoginData loginData;
    //[SerializeField] Response response;


    void Start()
    {
        // Asegúrate de que el botón tenga asignado el evento de clic
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    void OnLoginButtonClicked()
    {
        loginData.username = emailInputField.text;
        loginData.password = passwordInputField.text;
        this.loginButton.interactable = false;
        //print(loginData.email + " " + loginData.password);
        StartCoroutine(TryLogin());
    }
    
    IEnumerator TryLogin()
    {
        string jsonData = JsonUtility.ToJson(loginData);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            this.loginButton.interactable = true;
        }
        else
        {
            Debug.Log("Respuesta: " + request.downloadHandler.text);
            //response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            //if (response.status == "error") 
            //{
            //    // Aquí puedes manejar el error de inicio de sesión
            //    Debug.LogError("Login failed: " + response.message);
            //    this.loginButton.interactable = true;
            //}
            //else if (response.status == "success")
            //{
            //    // Aquí puedes manejar la respuesta exitosa
            //    Debug.Log("Login successful: " + response.status);
            //    SceneHelper.LoadScene("SampleScene");
            //    // Puedes cargar una nueva escena o realizar otra acción
            //}
        }
    }
}
[System.Serializable]
public class LoginData
{
    public string username;
    public string password;
}

[System.Serializable]
public class Response
{
    public string status;
    public string message;
}
