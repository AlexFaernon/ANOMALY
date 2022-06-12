using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveScript
{
    public const string TankSavePath = "/Tank.succ";
    public const string MedicSavePath = "/Medic.succ";
    public const string DamagerSavePath = "/Damager.succ";
    public const string BasicTokensSavePath = "/Basic.succ";
    public const string AdvancedTokensSavePath = "/Advanced.succ";
    public const string UltimateTokensSavePath = "/Utlimate.succ";
    public const string LevelsSavePath = "/Levels.succ";
    public const string GameStateSavePath = "/GameState.succ";
    private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

    public static void SaveCharacters()
    {
        foreach (CharacterClass characterClass in Enum.GetValues(typeof(CharacterClass)))
        {
            var character = Units.Characters[characterClass];
            var path = characterClass switch
            {
                CharacterClass.Damager => DamagerSavePath,
                CharacterClass.Medic => MedicSavePath,
                CharacterClass.Tank => TankSavePath,
                _ => throw new ArgumentOutOfRangeException(nameof(characterClass), characterClass, null)
            };
            var file = File.Create(Application.persistentDataPath + path);
            binaryFormatter.Serialize(file, character);
            file.Close();
        }
    }

    public static void SaveTokens()
    {
        var basicFile = File.Create(Application.persistentDataPath + BasicTokensSavePath);
        var advancedFile = File.Create(Application.persistentDataPath + AdvancedTokensSavePath);
        var ultimateFile = File.Create(Application.persistentDataPath + UltimateTokensSavePath);
        binaryFormatter.Serialize(basicFile, AbilityResources.BasicTokens);
        binaryFormatter.Serialize(advancedFile, AbilityResources.AdvancedTokens);
        binaryFormatter.Serialize(ultimateFile, AbilityResources.UltimateTokens);
        basicFile.Close();
        advancedFile.Close();
        ultimateFile.Close();
    }

    public static void SaveLevels()
    {
        var file = File.Create(Application.persistentDataPath + LevelsSavePath);
        binaryFormatter.Serialize(file, NodeScript.CurrentNodeNumber);
        file.Close();
    }
    
    public static void SaveGameState()
    {
        var file = File.Create(Application.persistentDataPath + GameStateSavePath);
        binaryFormatter.Serialize(file, GameState.IsGame);
        file.Close();
    }
}
