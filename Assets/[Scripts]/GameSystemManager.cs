using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{

    GameObject submitButton, userNameInput, passwordInput, registerToggle, loginToggle;

    GameObject textNameInfo, textPasswordInfo;

    GameObject GameRoomButton;

    GameObject networkedClient;

    GameObject TicTacToeBoard;
    void Start()
    {

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "NameInputField")
                userNameInput = go;
            else if (go.name == "PasswordInputField")
                passwordInput = go;
            else if (go.name == "EnterButton")
                submitButton = go;
            else if (go.name == "RegisterToggle")
                registerToggle = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "NetworkedClient")
                networkedClient = go;
            else if (go.name == "GameRoomButton")
                GameRoomButton = go;
            else if (go.name == "NameText")
                textNameInfo = go;
            else if (go.name == "PasswordText")
                textPasswordInfo = go;
            else if(go.name == "TicTacToeBoard")
                TicTacToeBoard = go;


        }

        submitButton.GetComponent<Button>().onClick.AddListener(EnterButtonPressed);
        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleSwitch);
        registerToggle.GetComponent<Toggle>().onValueChanged.AddListener(RegisterToggleSwitch);

        GameRoomButton.GetComponent<Button>().onClick.AddListener(GameRoomButtonPressed);
        

        ChangeState(GameStates.LoginMenu);
    }

    
    void Update()
    {
        
    }

    public void EnterButtonPressed()
    {
        string n = userNameInput.GetComponent<InputField>().text;
        string p = passwordInput.GetComponent<InputField>().text;

        string msg;

        if (registerToggle.GetComponent<Toggle>().isOn)
        {
            msg = ClientToServerSignifiers.RegisterAccount + "," + n + "," + p;
        }
        else
        {
            msg = ClientToServerSignifiers.LoginToAccount + "," + n + "," + p;
        }

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

    }

    public void LoginToggleSwitch(bool isSelected)
    {
        registerToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!isSelected);
    }

    public void RegisterToggleSwitch(bool isSelected)
    {
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!isSelected);
    }

    public void ChangeState(int newState)
    {
        submitButton.SetActive(false);
        userNameInput.SetActive(false);
        passwordInput.SetActive(false);
        registerToggle.SetActive(false);
        loginToggle.SetActive(false);
        GameRoomButton.SetActive(false);
        textNameInfo.SetActive(false);
        textPasswordInfo.SetActive(false);
        TicTacToeBoard.SetActive(false);

        if (newState == GameStates.LoginMenu)
        {
            submitButton.SetActive(true);
            userNameInput.SetActive(true);
            passwordInput.SetActive(true);
            registerToggle.SetActive(true);
            loginToggle.SetActive(true);
            textNameInfo.SetActive(true);
            textPasswordInfo.SetActive(true);
        }
        else if(newState == GameStates.Lobby)
        {
            GameRoomButton.SetActive(true);

        }
        else if(newState == GameStates.WaitingForPlayers)
        {

        }
        else if(newState == GameStates.Game)
        {
            TicTacToeBoard.SetActive(true);
        }

    }

    public void GameRoomButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinGameRoomQueue + "");
        ChangeState(GameStates.WaitingForPlayers);
    }

    public void boardPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToeGame + "");
    }
}

public static class GameStates
{
    public const int LoginMenu = 1;
    public const int Lobby = 2;
    public const int WaitingForPlayers = 3;
    public const int Game = 4;
}