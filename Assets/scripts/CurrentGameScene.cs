using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentGameScene //: MonoBehaviour
{
    public static GameScene GameScene = GameScene.CharacterInfo;
}

public enum GameScene
{
    Map,
    CharacterInfo
}
