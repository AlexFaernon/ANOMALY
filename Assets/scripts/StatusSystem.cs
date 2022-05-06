using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatusSystem
{
    public static readonly List<Status> StatusList = new List<Status>();
}

public abstract class Status
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Duration { get; set; }
    public abstract IUnit Target { get; set; }

    public abstract void Dispel();
}

public sealed class Invulnerability : Status
{
    public override string Name { get; } = "Отсрочка смерти";
    public override string Description { get; } = "Здоровье не может упасть ниже 1";
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
        EventAggregator.UpdateStatus.Publish();
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
    public override string Name { get; } = "Защита";
    public override string Description { get; } = "При атаке, другой юнит получит урон вместо данного";
    public override int Duration { get; set; } = 1;
    public readonly IUnit Protector;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        Target.ModifyReceivedDamage.Event.RemoveListener(RedirectDamage);
        Debug.Log("Protect removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        EventAggregator.UpdateStatus.Publish();
    }

    public Protect(IUnit target, IUnit protector)
    {
        Target = target;
        Protector = protector;
        if (target == protector)
        {
            Dispel();
            return;
        }
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
    public override string Name { get; } = "Оглушение";
    public override string Description { get; } = "Данный юнит не может действовать";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        Target.CanMove = true;
        EventAggregator.NewTurn.Unsubscribe(KeepStun);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Stun end");
        EventAggregator.UpdateStatus.Publish();
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
    public override string Name { get; } = "Отражение урона";
    public override string Description { get; } = "Наносит 2 урона юнитам, атакующего данного юнита";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        Target.ModifyReceivedDamage.Event.RemoveListener(TakeDamageFromDeflect);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Deflect stop");
        EventAggregator.UpdateStatus.Publish();
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
    public override string Name { get; } = "Берсерк";
    public override string Description { get; } = "Ультимативная способность заменена на разрушительную атаку";
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
        EventAggregator.UpdateStatus.Publish();
    }

    public Berserk(ICharacter target, IAbility ability)
    {
        Target = target;
        
        targetCharacter = target;
        oldAbility = targetCharacter.Ultimate;
        targetCharacter.Ultimate = ability;
        targetCharacter.CanMove = true;
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

public sealed class AmplifyDamage : Status
{
    public override string Name { get; } = "Хрупкость";
    public override string Description { get; } = "Урон по данному юниту увеличен в 1.5 раза";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        Target.ModifyReceivedDamage.Event.RemoveListener(DamageUp);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("DamageUp stop");
        EventAggregator.UpdateStatus.Publish();
    }

    public AmplifyDamage(IUnit target)
    {
        Target = target;
        Target.ModifyReceivedDamage.Event.AddListener(DamageUp);
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("DamageUp start");
    }

    private void DamageUp()
    {
        Target.ModifyReceivedDamage.Damage += 1;
    }

    private void OnTurn()
    {
        Duration -= 1;
        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Debug.Log("DamageUp " + Duration);
    }
}

public sealed class DelayedHealing : Status
{
    public override string Name { get; } = "Регенерация";
    public override string Description { get; } = "Исцеляет на 1 пункт каждый ход";
    public override int Duration { get; set; } = 3;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        EventAggregator.UpdateStatus.Publish();
    }

    public DelayedHealing(IUnit target)
    {
        Target = target;
        Target.TakeDamage(1, null);
        EventAggregator.NewTurn.Subscribe(OnTurn);
    }

    private void OnTurn()
    {
        Target.Heal(1);
        Duration -= 1;
        if (Duration == 0)
        {
            Dispel();
        }
    }
}

public sealed class LifeSteal : Status
{
    public override string Name { get; } = "Кража жизни";
    public override string Description { get; } = "Восстанавливает здоровье в половину от нанесенного урона";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override void Dispel()
    {
        StatusSystem.StatusList.Remove(this);
        EventAggregator.DamageDealtByUnit.Unsubscribe(HealByDamage);
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        Debug.Log("LifeSteal stop");
        EventAggregator.UpdateStatus.Publish();
    }

    public LifeSteal(IUnit target)
    {
        Target = target;
        EventAggregator.DamageDealtByUnit.Subscribe(HealByDamage);
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("LifeSteal start");
    }
    
    private void OnTurn()
    {
        Duration -= 1;
        Debug.Log("LifeSteal " + Duration);
        if (Duration == 0)
        {
            Dispel();
        }
    }

    private void HealByDamage(int damage, IUnit source)
    {
        if (source == Target)
        {
            Target.Heal(1);
        }
    }
}