using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatusSystem
{
    public static readonly List<Status> StatusList = new List<Status>();
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
        if (Target.HP - Target.ModifyReceivedDamage.Damage < 1)
        {
            Target.ModifyReceivedDamage.Damage = Target.HP - 1;
        }
    }
    
    public override void Dispel()
    {
        Target.ModifyReceivedDamage.Event.RemoveListener(MakeInvulnerable);
        Debug.Log("Invul removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
    }
    
    public Invulnerability(IUnit target)
    {
        Target = target;
        Target.ModifyReceivedDamage.Event.AddListener(MakeInvulnerable);
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
}

public sealed class Protect : Status
{
    public override int Duration { get; set; } = 1;
    public readonly IUnit Protector;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        Target.ModifyReceivedDamage.Event.RemoveListener(RedirectDamage);
        Debug.Log("Protect removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
    }

    public Protect(IUnit target, IUnit protector)
    {
        Target = target;
        Protector = protector;
        target.ModifyReceivedDamage.Event.AddListener(RedirectDamage);
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
        var damage = Target.ModifyReceivedDamage.Damage;
        Target.ModifyReceivedDamage.Damage = 0;
        Protector.TakeDamage(damage, Target.ModifyReceivedDamage.Source);
        Debug.Log("redirected");
    }
}

public sealed class Stun : Status
{
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(KeepStun);
        StatusSystem.StatusList.Remove(this);
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

public sealed class Deflect : Status
{
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Deflect stop");
    }

    public Deflect(IUnit target)
    {
        Target = target;
        Target.ModifyReceivedDamage.Event.AddListener(TakeDamageFromDeflect);
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("Deflect start");
    }

    private void OnTurn()
    {
        Duration -= 1;
        if (Duration == 0)
        {
            Dispel();
            Debug.Log("Deflect stop");
            return;
        }
        
        Debug.Log("Deflect " + Duration);
    }

    private void TakeDamageFromDeflect()
    {
        Target.ModifyReceivedDamage.Source?.TakeDamage(2, null);
    }
}

public sealed class Berserk : Status
{
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    private readonly ICharacter targetCharacter;
    private readonly IAbility oldAbility;
    public override void Dispel()
    {
        targetCharacter.Ultimate = oldAbility;
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Berserk returned");
    }

    public Berserk(ICharacter target, IAbility ability)
    {
        Target = target;
        targetCharacter = target;
        oldAbility = targetCharacter.Ultimate;
        targetCharacter.Ultimate = ability;
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("Berserk Replaced");
    }

    private void OnTurn()
    {
        Duration -= 1;
        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Debug.Log("Berserk " + Duration);
    }
}