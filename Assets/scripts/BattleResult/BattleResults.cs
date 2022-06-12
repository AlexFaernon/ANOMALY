using TMPro;
using UnityEngine;

public static class BattleResultsSingleton
{
    public static int LevelsCompleted => NodeScript.CurrentNodeNumber;
    public static int EnemiesKilled;
    public static int UpgradesMade;
    public static int TotalBasicTokens;
    public static int TotalAdvancedTokens;
    public static int TotalUltimateTokens;

    public static void ResetResults()
    {
        EnemiesKilled = 0;
        UpgradesMade = 0;
        TotalBasicTokens = 0;
        TotalAdvancedTokens = 0;
        TotalUltimateTokens = 0;
    }
}

public class BattleResults : MonoBehaviour
{
    [SerializeField] private TMP_Text locations;
    [SerializeField] private TMP_Text enemies;
    [SerializeField] private TMP_Text upgrades;
    [SerializeField] private TMP_Text basic;
    [SerializeField] private TMP_Text advanced;
    [SerializeField] private TMP_Text ultimate;

    private void Awake()
    {
        locations.text = BattleResultsSingleton.LevelsCompleted.ToString();
        enemies.text = BattleResultsSingleton.EnemiesKilled.ToString();
        upgrades.text = BattleResultsSingleton.UpgradesMade.ToString();
        basic.text = BattleResultsSingleton.TotalBasicTokens.ToString();
        advanced.text = BattleResultsSingleton.TotalAdvancedTokens.ToString();
        ultimate.text = BattleResultsSingleton.TotalUltimateTokens.ToString();
    }
}
