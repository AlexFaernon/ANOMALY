using System.Collections.Generic;

public static class AbilityResources
{
    public static Dictionary<AbilityType, int> Resources => new Dictionary<AbilityType, int>
    {
        { AbilityType.Basic, BasicTokens },
        { AbilityType.First, AdvancedTokens },
        { AbilityType.Second, AdvancedTokens },
        { AbilityType.Ultimate, UltimateTokens }
    };

    public static int _basicTokens;
    public static int _advancedTokens;
    public static int _ultimateTokens;
    public static int BasicTokens
    {
        get => _basicTokens;
        set
        {
            _basicTokens = value;
            BattleResultsSingleton.TotalBasicTokens += value;
            SaveScript.SaveTokens();
        }
    }

    public static int AdvancedTokens
    {
        get => _advancedTokens;
        set
        {
            _advancedTokens = value;
            BattleResultsSingleton.TotalAdvancedTokens += value;
            SaveScript.SaveTokens();
        }
    }

    public static int UltimateTokens
    {
        get => _ultimateTokens;
        set
        {
            _ultimateTokens = value;
            BattleResultsSingleton.TotalUltimateTokens += value;
            SaveScript.SaveTokens();
        }
    }
}