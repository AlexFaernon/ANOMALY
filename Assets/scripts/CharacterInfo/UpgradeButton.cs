using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    public void ToUpgrades() 
    {
        GameState.GameScene = GameScene.CharacterInfo;
        SceneManager.LoadScene("AbilityImprovement");
    }
}
