using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LoadScript
{
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
    
    public static void Load()
    {
        LoadTank();
        LoadDamager();
        LoadMedic();
        LoadTokens();
        LoadLevels();
        LoadGameState();
    }

    private static void LoadTank()
    {
        if (!File.Exists(Application.persistentDataPath + SaveScript.TankSavePath)) return;
        
        var file = File.Open(Application.persistentDataPath + SaveScript.TankSavePath, FileMode.Open);
        Units.Characters[CharacterClass.Tank] = (ICharacter)binaryFormatter.Deserialize(file);
        file.Close();
    }
    
    private static void LoadDamager()
    {
        if (!File.Exists(Application.persistentDataPath + SaveScript.DamagerSavePath)) return;
        
        var file = File.Open(Application.persistentDataPath + SaveScript.DamagerSavePath, FileMode.Open);
        Units.Characters[CharacterClass.Damager] = (ICharacter)binaryFormatter.Deserialize(file);
        file.Close();
    }
    
    private static void LoadMedic()
    {
        if (!File.Exists(Application.persistentDataPath + SaveScript.MedicSavePath)) return;
        
        var file = File.Open(Application.persistentDataPath + SaveScript.MedicSavePath, FileMode.Open);
        Units.Characters[CharacterClass.Medic] = (ICharacter)binaryFormatter.Deserialize(file);
        file.Close();
    }

    private static void LoadTokens()
    {
        if (File.Exists(Application.persistentDataPath + SaveScript.BasicTokensSavePath))
        {
            var basicFile = File.Open(Application.persistentDataPath + SaveScript.BasicTokensSavePath, FileMode.Open);
            AbilityResources._basicTokens = (int)binaryFormatter.Deserialize(basicFile);
            basicFile.Close();
        }
        
        if (File.Exists(Application.persistentDataPath + SaveScript.AdvancedTokensSavePath))
        {
            var advancedFile = File.Open(Application.persistentDataPath + SaveScript.AdvancedTokensSavePath,
                FileMode.Open);
            AbilityResources._advancedTokens = (int)binaryFormatter.Deserialize(advancedFile);
            advancedFile.Close();
        }
        
        if (File.Exists(Application.persistentDataPath + SaveScript.UltimateTokensSavePath))
        {
            var ultimateFile = File.Open(Application.persistentDataPath + SaveScript.UltimateTokensSavePath,
                FileMode.Open);
            AbilityResources._ultimateTokens = (int)binaryFormatter.Deserialize(ultimateFile);
            ultimateFile.Close();
        }
    }
    
    private static void LoadLevels()
    {
        if (!File.Exists(Application.persistentDataPath + SaveScript.LevelsSavePath)) return;
        
        var file = File.Open(Application.persistentDataPath + SaveScript.LevelsSavePath, FileMode.Open);
        NodeScript._currentNodeNumber = (int)binaryFormatter.Deserialize(file);
        file.Close();
    }
    
    private static void LoadGameState()
    {
        if (!File.Exists(Application.persistentDataPath + SaveScript.GameStateSavePath)) return;
        
        var file = File.Open(Application.persistentDataPath + SaveScript.GameStateSavePath, FileMode.Open);
        GameState._isGame = (bool)binaryFormatter.Deserialize(file);
        file.Close();
    }
}
