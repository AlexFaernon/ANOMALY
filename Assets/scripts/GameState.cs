using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static GameScene GameScene = GameScene.CharacterInfo;
    public static bool isGame;
}

public enum GameScene
{
    Map,
    CharacterInfo
}
