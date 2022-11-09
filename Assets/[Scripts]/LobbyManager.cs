using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Button localButton;
    public Button onlineButton;

    void Start()
    {
        localButton.onClick.AddListener(localPlay);
        onlineButton.onClick.AddListener(onlinePlay);
    }

    void localPlay()
    {
        SceneManager.LoadScene("LocalPvPScene");
    }

    void onlinePlay()
    {

    }
}
