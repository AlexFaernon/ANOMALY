using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeButton : MonoBehaviour
{
    public void ToUpgrades() 
    {
        GameState.GameScene = GameScene.CharacterInfo;
        SceneManager.LoadScene("AbilityImprovement");
    }
}
