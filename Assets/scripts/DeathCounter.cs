using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] private GameObject ShadingWin;
    [SerializeField] private TMP_Text newBasicTokens;
    [SerializeField] private TMP_Text newAdvancedTokens;
    [SerializeField] private TMP_Text newUltimateTokens;
    private Random random;

    private void Awake()
    {
        random = new Random();
        EventAggregator.CharacterDied.Subscribe(CharacterDied);
        EventAggregator.EnemyDied.Subscribe(EnemyDied);
    }

    private void CharacterDied(ICharacter character)
    {
        if (!Units.Characters.Values.All(character1 => character1.IsDead)) return;
        
        SceneManager.LoadScene("BattleResult");
        SaveScript.SaveCharacters();
    }
    
    private void EnemyDied(IEnemy enemy)
    {
        Units.Enemies.Remove(enemy);
        BattleResultsSingleton.EnemiesKilled++;
        if (Units.Enemies.Count != 0) return;
        
        NodeScript.CurrentNodeNumber++;
        CreateNewTokens();
        StatusSystem.DispelAll();
        SaveScript.SaveCharacters();
        ShadingWin.SetActive(true);
    }

    private void CreateNewTokens()
    {
        var basic = random.Next(1, 4);
        var advanced = random.Next(1, 4);
        var ultimate = random.Next(3);

        AbilityResources.BasicTokens += basic;
        AbilityResources.AdvancedTokens += advanced;
        AbilityResources.UltimateTokens += ultimate;

        newBasicTokens.text = basic.ToString();
        newAdvancedTokens.text = advanced.ToString();
        newUltimateTokens.text = ultimate.ToString();
    }

    private void OnDestroy()
    {
        EventAggregator.CharacterDied.Unsubscribe(CharacterDied);
        EventAggregator.EnemyDied.Unsubscribe(EnemyDied);
    }
}
