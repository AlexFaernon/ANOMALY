using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        NodeScript.currentNodeNumber = 0;
        StatusSystem.DispelAll();
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        GameState.isGame = false;
        SceneManager.LoadScene("MainMenu");
    }
}
