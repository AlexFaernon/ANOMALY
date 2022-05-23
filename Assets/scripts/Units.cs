using System.Collections.Generic;
using System.Linq;

public static class Units
{
    public static Dictionary<CharacterClass, ICharacter> Characters =
        new Dictionary<CharacterClass, ICharacter>
        {
            { CharacterClass.Damager, new Damager() }, { CharacterClass.Medic, new Medic() },
            { CharacterClass.Tank, new Tank() }
        };
    public static List<IEnemy> Enemies = new List<IEnemy>();

    public static void ResetUnits()
    {
        Characters =
            new Dictionary<CharacterClass, ICharacter>
            {
                { CharacterClass.Damager, new Damager() }, { CharacterClass.Medic, new Medic() },
                { CharacterClass.Tank, new Tank() }
            };
        Enemies = new List<IEnemy>();
    }
}
