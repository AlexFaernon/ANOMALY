using System;
using System.Linq;
using TMPro;
using UnityEngine;

public static class BattleResultsSingleton
{
    public static bool isWin;
    public static int LevelsCompleted => MapSingleton.Nodes.Count(node => node.IsCompleted);
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
    [SerializeField] private TMP_Text resultTitle;
    [SerializeField] private GameObject locationUnlocked;

    private void Awake()
    {
        locations.text = BattleResultsSingleton.LevelsCompleted.ToString();
        enemies.text = BattleResultsSingleton.EnemiesKilled.ToString();
        upgrades.text = BattleResultsSingleton.UpgradesMade.ToString();
        basic.text = BattleResultsSingleton.TotalBasicTokens.ToString();
        advanced.text = BattleResultsSingleton.TotalAdvancedTokens.ToString();
        ultimate.text = BattleResultsSingleton.TotalUltimateTokens.ToString();
        locationUnlocked.gameObject.SetActive(BattleResultsSingleton.isWin);
        resultTitle.text = BattleResultsSingleton.isWin ? "ПОБЕДА!" : "Поражение!";
    }
}
