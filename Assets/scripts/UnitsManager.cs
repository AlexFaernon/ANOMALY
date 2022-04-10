using System.Collections.Generic;
using System.Linq;

public static class UnitsManager
{
    public static IEnumerable<IUnit> Units => Characters.Select(x => (IUnit)x).Concat(Enemies.Select(x => (IUnit)x));
    public static readonly List<ICharacter> Characters = new List<ICharacter>();
    public static readonly List<IEnemy> Enemies = new List<IEnemy>();
}
