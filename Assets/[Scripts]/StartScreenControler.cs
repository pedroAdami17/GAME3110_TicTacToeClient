using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenControler : MonoBehaviour
{
    public void PlayOnline()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PlayOffline()
    {
        SceneManager.LoadScene("LocalPvPScene");
    }
}
