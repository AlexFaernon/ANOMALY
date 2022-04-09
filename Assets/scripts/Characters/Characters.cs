using UnityEngine.Events;
public class ModifyDamage
{
    public int Damage { get; set; }
    public readonly UnityEvent Event = new UnityEvent();
}