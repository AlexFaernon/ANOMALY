using System.Collections.Generic;
using UnityEngine;

public static class StatusEffects
{
    public static readonly List<Status> Effects = new List<Status>();
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

    private void MakeInvulnerable()
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

        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Debug.Log("Invul " + Duration);
    }

    private void OnEnd()
    {
        Target.ModifyDamage.Event.RemoveListener(MakeInvulnerable);
        Debug.Log("Invul removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusEffects.Effects.Remove(this);
    }
}

public sealed class Protect : Status
{
    public override int Duration { get; set; } = 1;
    public readonly IUnit Source;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        OnEnd();
    }

    public Protect(IUnit target, IUnit source)
    {
        Target = target;
        Source = source;
        target.ModifyDamage.Event.AddListener(RedirectDamage);
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("Protect added");
    }
    
    private void OnTurn()
    {
        Duration -= 1;

        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Debug.Log("Protect " + Duration);
    }

    private void RedirectDamage()
    {
        var damage = Target.ModifyDamage.Damage;
        Target.ModifyDamage.Damage = 0;
        Source.TakeDamage(damage);
        Debug.Log("redirected");
    }

    private void OnEnd()
    {
        Target.ModifyDamage.Event.RemoveListener(RedirectDamage);
        Debug.Log("Protect removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusEffects.Effects.Remove(this);
    }
}

public sealed class Stun : Status
{
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(KeepStun);
        Debug.Log("Stun end");
    }

    public Stun(IUnit target)
    {
        Target = target;
        Target.CanMove = false;
        EventAggregator.NewTurn.Subscribe(KeepStun);
        Debug.Log("Stun start");
    }

    private void KeepStun()
    {
        Duration -= 1;
        
        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Target.CanMove = false;
        Debug.Log("Stun " + Duration);
    }
}