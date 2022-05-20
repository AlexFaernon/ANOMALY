using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeReturnButton : MonoBehaviour
{
    public void Back()
    {
        switch (CurrentGameScene.GameScene)
        {
            case GameScene.Map:
                SceneManager.LoadScene("Map");
                break;
            case GameScene.CharacterInfo:
                SceneManager.LoadScene("CharacterInfo");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
