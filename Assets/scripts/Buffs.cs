using System.Collections.Generic;
using UnityEngine;

public static class BuffsClass
{
    public static readonly List<Status> Buffs = new List<Status>();
}

public abstract class Status
{
    public abstract int Duration { get; set; }
    public abstract IUnit Target { get; set; }
    public abstract void Dispel();
}

public sealed class Invulnerability : Status
{
    public override int Duration { get; set; } = 5;
    public override IUnit Target { get; set; }

    public void MakeInvulnerable()
    {
        if (Target.HP - Target.ModifyDamage.Damage < 1)
        {
            Target.ModifyDamage.Damage = Target.HP - 1;
        }
    }
    
    public override void Dispel()
    {
        OnEnd();
    }
    
    public Invulnerability(IUnit target)
    {
        Target = target;
        Target.ModifyDamage.Event.AddListener(MakeInvulnerable);
        Debug.Log("Invul added");

        EventAggregator.NewTurn.Subscribe(OnTurn);
    }

    private void OnTurn()
    {
        Duration -= 1;
        Debug.Log("Invul " + Duration);
        
        if (Duration == 0) {Dispel();}
    }

    private void OnEnd()
    {
        Target.ModifyDamage.Event.RemoveListener(MakeInvulnerable);
        Debug.Log("Invul removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        BuffsClass.Buffs.Remove(this);
    }
}