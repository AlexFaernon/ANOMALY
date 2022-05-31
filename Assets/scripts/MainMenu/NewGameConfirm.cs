using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameConfirm : MonoBehaviour
{
    public void NewGame()
    {
        NodeScript.currentNodeNumber = 0;
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        GameState.isGame = false;
        SceneManager.LoadScene("GlobalMap");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
