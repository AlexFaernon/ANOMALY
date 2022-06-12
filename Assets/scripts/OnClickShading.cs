using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickShading : MonoBehaviour
{
    public void OnClickWin()
    {
        SceneManager.LoadScene("Map");
    }

    public void AbandonBattle()
    {
        NodeScript.CurrentNodeNumber = 0;
        StatusSystem.DispelAll();
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        GameState.IsGame = false;
        SceneManager.LoadScene("MainMenu");
    }
}
