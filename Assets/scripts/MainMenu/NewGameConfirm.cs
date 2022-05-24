using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameConfirm : MonoBehaviour
{
    public void NewGame()
    {
        MapSingleton.Nodes = new Node[7];
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
