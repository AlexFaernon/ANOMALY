using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentGameScene
{
    public static GameScene GameScene = GameScene.CharacterInfo;
}

public enum GameScene
{
    Map,
    CharacterInfo
}
