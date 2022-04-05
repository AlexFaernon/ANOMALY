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
}

public sealed class Invulnerability : Status
{
    public override int Duration { get; set; } = 5;
    public override IUnit Target { get; set; }

    private delegate void InvulnerableDelegate(IUnit target);

    private InvulnerableDelegate Delegate;

    public void MakeInvulnerable(IUnit target)
    {
        if (target.HP - target.ModifyDamage.Damage < 1)
        {
            target.ModifyDamage.Damage = target.HP - 1;
        }
    }
    
    public Invulnerability(IUnit target)
    {
        Delegate = MakeInvulnerable;
        Target = target;
        Target.ModifyDamage.Event.AddListener(Delegate(Target));
        Debug.Log("Invul added");

        EventAggregator.NewTurn.Subscribe(OnTurn);
    }

    private void OnTurn()
    {
        Duration -= 1;
        Debug.Log("Invul " + Duration);
        
        if (Duration == 0) {OnEnd();}
    }

    private void OnEnd()
    {
        Target.ModifyDamage.Event.RemoveListener(() => MakeInvulnerable(Target));
        Debug.Log("Invul removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        BuffsClass.Buffs.Remove(this);
    }
}