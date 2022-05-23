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

    private static int _basicTokens = 10;
    private static int _advancedTokens = 10;
    private static int _ultimateTokens = 10;
    public static int BasicTokens
    {
        get => _basicTokens;
        set
        {
            _basicTokens = value;
            BattleResultsSingleton.TotalBasicTokens += value;
        }
    }

    public static int AdvancedTokens
    {
        get => _advancedTokens;
        set
        {
            _advancedTokens = value;
            BattleResultsSingleton.TotalAdvancedTokens += value;
        }
    }

    public static int UltimateTokens
    {
        get => _ultimateTokens;
        set
        {
            _ultimateTokens = value;
            BattleResultsSingleton.TotalUltimateTokens += value;
        }
    }
}