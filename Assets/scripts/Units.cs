using System.Collections.Generic;
using System.Linq;

public static class Units
{
    public static readonly Dictionary<CharacterClass, ICharacter> Characters =
        new Dictionary<CharacterClass, ICharacter>
        {
            { CharacterClass.Damager, new Damager() }, { CharacterClass.Medic, new Medic() },
            { CharacterClass.Tank, new Tank() }
        };
    public static readonly List<IEnemy> Enemies = new List<IEnemy>();
}
