using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Register : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button registerButton;
    public Button loginButton;

    ArrayList credentials;

    void Start()
    {
        registerButton.onClick.AddListener(writeToFile);
        loginButton.onClick.AddListener(goToLogin);

        if (File.Exists(Application.dataPath + "/credentials.txt"))
        {
            credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/credentials.txt", "");
        }
    }

    void goToLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }

    void writeToFile()
    {
        bool isExists = false;
        
        credentials = new ArrayList(File.ReadAllLines(Application.dataPath + "/credentials.txt"));
        foreach(var i in credentials)
        {
            if(i.ToString().Contains(usernameInput.text))
            {
                isExists = true;
                break;
            }
        }
        if(isExists)
        {
            Debug.Log("Username " + usernameInput.text + " already exists");
        }
        else
        {
            credentials.Add(usernameInput.text + ":" + passwordInput.text);
            File.WriteAllLines(Application.dataPath + "/credentials.txt", (String[])credentials.ToArray(typeof(string)));
            Debug.Log("Account Created");
        }
    }
}
