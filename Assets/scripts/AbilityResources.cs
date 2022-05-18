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

    public static int BasicTokens { get; set; } = 10;
    public static int AdvancedTokens { get; set; } = 10;
    public static int UltimateTokens { get; set; } = 10;
}