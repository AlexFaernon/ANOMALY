using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickShading : MonoBehaviour
{
    public void OnClickWin()
    {
        StatusSystem.DispelAll();
        SceneManager.LoadScene("Map");
    }

    public void AbandonBattle()
    {
        MapSingleton.Nodes = new Node[7];
        StatusSystem.DispelAll();
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        GameState.isGame = false;
        SceneManager.LoadScene("MainMenu");
    }
}
