using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{
    //public static NetworkedClient Instance;

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5492;
    byte error;
    bool isConnected = false;
    int ourClientID;

    GameObject gameSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if(go.GetComponent<GameSystemManager>() != null)
            {
                gameSystemManager = go;
            }
        }

            Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            SendMessageToHost("Hello from client");

        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.0.142", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }
    
    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        if(signifier == ServerToClientSignifiers.RegisterComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.Lobby);
        }
        else if(signifier == ServerToClientSignifiers.LoginComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.Lobby);
        }
        else if(signifier == ServerToClientSignifiers.OpponentPlay)
        {
            Debug.Log("Opponent Has played");
        }
        else if(signifier == ServerToClientSignifiers.GameStart)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.Game);
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }

}

public static class ClientToServerSignifiers
{
    public const int RegisterAccount = 1;
    public const int LoginToAccount = 2;
    public const int JoinGameRoomQueue = 3;
    public const int TicTacToeGame = 4;
}

public static class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;

    public const int RegisterComplete = 3;
    public const int RegisterFailed = 4;

    public const int OpponentPlay = 5;

    public const int GameStart = 6;
}
