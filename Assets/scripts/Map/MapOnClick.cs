using UnityEngine;
using UnityEngine.SceneManagement;

public class MapOnClick : MonoBehaviour
{
    public void ToUpgrades()
    {
        GameState.GameScene = GameScene.Map;
        SceneManager.LoadScene("AbilityImprovement");
    }
}
