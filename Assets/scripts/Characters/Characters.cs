using UnityEngine.Events;
public class ModifyReceivedDamage
{
    public int Damage { get; set; }
    public IUnit Source;
    public readonly UnityEvent Event = new UnityEvent();
}
