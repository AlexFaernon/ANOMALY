using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    public void ToUpgrades() 
    {
        CurrentGameScene.GameScene = GameScene.CharacterInfo;
        SceneManager.LoadScene("AbilityImprovement");
    }
}
