using System;
using System.Collections.Generic;

public static class StatsUpgrades
{
    public static readonly Dictionary<StatsUpgradeType, Dictionary<UpgradeLevel, StatsUpgrade>> Upgrades =
        new Dictionary<StatsUpgradeType, Dictionary<UpgradeLevel, StatsUpgrade>>
        {
            { StatsUpgradeType.HP, new Dictionary<UpgradeLevel, StatsUpgrade>
            {
                { UpgradeLevel.Second, new StatsUpgrade("Персонаж получает <color=#5EE67E>1 ед. макс. здоровья.</color>", character => character.MaxHP += 1)},
                { UpgradeLevel.Fourth, new StatsUpgrade("Персонаж получает <color=#5EE67E>1 секцию здоровья.</color>",character => character.MaxHP += character.HPSegmentLength) }
            } },
            { StatsUpgradeType.MP, new Dictionary<UpgradeLevel, StatsUpgrade>
            {
                { UpgradeLevel.Second, new StatsUpgrade("Персонаж получает <color=#4BDFC7>1 ед. макс. маны.</color>", character => character.MaxMP += 1) },
                { UpgradeLevel.Fourth, new StatsUpgrade("Персонаж получает <color=#4BDFC7>2 ед. макс. маны.</color>", character => character.MaxMP += 2) }
            } },
            { StatsUpgradeType.HPMP, new Dictionary<UpgradeLevel, StatsUpgrade>
            {
                { UpgradeLevel.Second, new StatsUpgrade("Персонаж получает <color=#4BDFC7>1 ед. макс. маны</color> и <color=#5EE67E>1 ед. здоровья в каждой секции.</color>", character =>
                {
                    character.MP += 1;
                    character.HPSegmentLength += 1;
                    character.MaxHP += character.HPSegmentLength;
                } ) },
                { UpgradeLevel.Fourth, new StatsUpgrade("Персонаж <color=#4BDFC7>ускоряет восстановление маны на 2 ед. за ход</color> и получает <color=#5EE67E>1 ед. здоровья в каждой секции.</color>", character =>
                {
                    character.MPRegeneration += 2;
                    character.HPSegmentLength += 1;
                    character.MaxHP += character.HPSegmentLength;
                } ) }
            } }
        };

    public class StatsUpgrade
    {
        public readonly string Description;
        public readonly Action<ICharacter> Upgrade;

        public StatsUpgrade(string description, Action<ICharacter> upgrade)
        {
            Description = description;
            Upgrade = upgrade;
        }
    }
}

public enum StatsUpgradeType
{
    None,
    HP,
    MP,
    HPMP
}
