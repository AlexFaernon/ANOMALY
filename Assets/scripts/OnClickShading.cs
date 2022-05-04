using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickShading : MonoBehaviour
{
    public void OnClickWin()
    {
        SceneManager.LoadScene("Map");
    }

    public void OnClickDefeat()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
