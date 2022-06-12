public static class GameState
{
    public static GameScene GameScene = GameScene.CharacterInfo;
    public static bool _isGame;
    public static bool IsGame
    {
        get => _isGame;
        set
        {
            _isGame = value;
            SaveScript.SaveGameState();
        }
    }
}

public enum GameScene
{
    Map,
    CharacterInfo
}
