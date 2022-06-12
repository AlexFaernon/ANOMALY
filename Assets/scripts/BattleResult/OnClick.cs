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
        NodeScript.CurrentNodeNumber = 0;
        StatusSystem.DispelAll();
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        GameState.IsGame = false;
        SceneManager.LoadScene("MainMenu");
    }
}
