using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapOnClick : MonoBehaviour
{
    public void ToUpgrades()
    {
        CurrentGameScene.GameScene = GameScene.Map;
        SceneManager.LoadScene("AbilityImprovement");
    }
}
