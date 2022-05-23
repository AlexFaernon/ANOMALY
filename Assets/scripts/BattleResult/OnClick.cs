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
        MapSingleton.Nodes = new Node[7];
        Units.ResetUnits();
        BattleResultsSingleton.ResetResults();
        SceneManager.LoadScene("MainMenu");
    }
}
