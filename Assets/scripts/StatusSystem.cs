using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatusSystem
{
    public static readonly List<Status> StatusList = new List<Status>();

    public static void DispelAll()
    {
        foreach (var status in StatusList.ToList())
        {
            status.Dispel();
        }
    }

    public static void DispelOnUnit(IUnit unit)
    {
        foreach (var status in StatusList.Where(status => status.Target == unit).ToList())
        {
            status.Dispel();
        }
    }
}

public abstract class Status
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int Duration { get; set; }
    public abstract IUnit Target { get; set; }
    public abstract Sprite Icon { get; set; }

    public abstract void Dispel();
}

public sealed class Invulnerability : Status
{
    public override string Name => "Отсрочка смерти";
    public override string Description => "Здоровье не может упасть ниже 1 ед.";
    public override int Duration { get; set; } = 5;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private readonly int upgradeLevel;

    private void MakeInvulnerable()
    {
        var minHP = new[] { 1, 3, ((ICharacter)Target).HPSegmentLength }[upgradeLevel];
        
        if (Target.HP - Target.ModifyReceivedDamage.Damage < minHP)
        {
            Target.ModifyReceivedDamage.Damage = Math.Max(Target.HP - minHP, 0);
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
    
    public Invulnerability(IUnit target, int upgradeLevel)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/invul");
        Target = target;
        this.upgradeLevel = upgradeLevel;

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

public sealed class HPLoss : Status
{
    public override string Name => "";
    public override string Description => "Цель теряет 1 ед. здоровья каждый ход";
    public override int Duration { get; set; } = 3;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }

    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        EventAggregator.UpdateStatus.Publish();
    }

    public HPLoss(IUnit target)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/bleeding");
        Target = target;

        EventAggregator.NewTurn.Subscribe(OnTurn);
    }

    private void OnTurn()
    {
        Duration -= 1;
        Target.TakeDamage(1, null);

        if (Duration == 0)
        {
            Dispel();
            return;
        }
        
        Debug.Log("HPLoss " + Duration);
    }
}

public sealed class Protect : Status
{
    public override string Name => "Защита";
    public override string Description => "Этот юнит находится под защитой";
    public override int Duration { get; set; } = 1;
    public readonly IUnit Protector;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private readonly double damageReduction;
    public override void Dispel()
    {
        if (Target == Protector)
        {
            Target.ModifyReceivedDamage.Event.RemoveListener(ReduceDamage);
        }
        else
        {
            Target.ModifyReceivedDamage.Event.RemoveListener(RedirectDamage);
        }
        Debug.Log("Protect removed");
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        EventAggregator.UpdateStatus.Publish();
    }

    public Protect(IUnit target, IUnit protector, double damageReduction)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/protect");
        this.damageReduction = damageReduction;
        Target = target;
        Protector = protector;
        if (target == protector)
        {
            target.ModifyReceivedDamage.Event.AddListener(ReduceDamage);
        }
        else
        {
            target.ModifyReceivedDamage.Event.AddListener(RedirectDamage);
        }
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
        Protector.TakeDamage((int)Math.Ceiling(damage*damageReduction), Target.ModifyReceivedDamage.Source);
        Debug.Log("redirected");
    }

    private void ReduceDamage()
    {
        Target.ModifyReceivedDamage.Damage = (int)(Target.ModifyReceivedDamage.Damage * damageReduction);
    }
}

public sealed class Stun : Status
{
    public override string Name => "Оглушение";
    public override string Description => "Данный юнит не может действовать";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }

    public override void Dispel()
    {
        Target.CanMove = true;
        EventAggregator.NewTurn.Unsubscribe(KeepStun);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Stun end");
        EventAggregator.UpdateStatus.Publish();
    }

    public Stun(IUnit target, int duration)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/stun");
        Duration = duration;
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
    public override string Name => "Отражение урона";
    public override string Description => $"Наносит {damage} ед. урона юнитам, атакующего данного юнита";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private int damage;
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        Target.ModifyReceivedDamage.Event.RemoveListener(TakeDamageFromDeflect);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("Deflect stop");
        EventAggregator.UpdateStatus.Publish();
    }

    public Deflect(IUnit target, int damage)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/deflect");
        this.damage = damage;
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
        Target.ModifyReceivedDamage.Source?.TakeDamage(damage, null);
    }
}

public sealed class Berserk : Status
{
    public override string Name => "Берсерк";
    public override string Description => "Ультимативная способность заменена на разрушительную атаку";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
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
        Icon = Resources.Load<Sprite>("StatusIcons/berserk");
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
    public override string Name => "Уязвимость";
    public override string Description => $"Данный юнит получает на {additionalDamage} ед. урона больше";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private readonly int additionalDamage;
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        Target.ModifyReceivedDamage.Event.RemoveListener(DamageUp);
        StatusSystem.StatusList.Remove(this);
        Debug.Log("DamageUp stop");
        EventAggregator.UpdateStatus.Publish();
    }

    public AmplifyDamage(IUnit target, int additionalDamage)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/fragile");
        this.additionalDamage = additionalDamage;
        Target = target;
        Target.ModifyReceivedDamage.Event.AddListener(DamageUp);
        EventAggregator.NewTurn.Subscribe(OnTurn);
        Debug.Log("DamageUp start");
    }

    private void DamageUp()
    {
        Target.ModifyReceivedDamage.Damage += additionalDamage;
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
    public override string Name => "Регенерация";
    public override string Description => "Восстанавливает 1 ед. здоровья каждый ход";
    public override int Duration { get; set; } = 3;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private readonly int healPower;
    public override void Dispel()
    {
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        StatusSystem.StatusList.Remove(this);
        EventAggregator.UpdateStatus.Publish();
    }

    public DelayedHealing(IUnit target, int healPower, int lifeTake)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/delayed healing");
        Target = target;
        this.healPower = healPower;
        Target.TakeDamage(lifeTake, null);
        EventAggregator.NewTurn.Subscribe(OnTurn);
    }

    private void OnTurn()
    {
        Target.Heal(healPower);
        Duration -= 1;
        if (Duration == 0)
        {
            Dispel();
        }
    }
}

public sealed class LifeSteal : Status
{
    public override string Name => "Кража жизни";
    public override string Description => $"Восстанавливает здоровье при нанесении урона. {(upgradeLevel == 3 ? $" Восстановит больше при отсутствии урона (урон {(damageTaken ? "получен" : "не получен")})." : "")}";
    public override int Duration { get; set; } = 2;
    public override IUnit Target { get; set; }
    public override Sprite Icon { get; set; }
    private readonly int upgradeLevel;
    private bool damageTaken;
    public override void Dispel()
    {
        StatusSystem.StatusList.Remove(this);
        
        EventAggregator.UnitDamagedUnit.Unsubscribe(HealByDamage);
        EventAggregator.UnitDamagedUnit.Unsubscribe(CheckDamage);
        EventAggregator.NewTurn.Unsubscribe(OnTurn);
        
        Debug.Log("LifeSteal stop");
        EventAggregator.UpdateStatus.Publish();
    }

    public LifeSteal(IUnit target, int upgradeLevel)
    {
        Icon = Resources.Load<Sprite>("StatusIcons/life steal");
        this.upgradeLevel = upgradeLevel;
        Target = target;
        
        EventAggregator.UnitDamagedUnit.Subscribe(HealByDamage);
        EventAggregator.UnitDamagedUnit.Subscribe(CheckDamage);
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

    private void CheckDamage(int damage, IUnit source, IUnit target)
    {
        if (target == Target)
        {
            damageTaken = true;
        }
    }

    private void HealByDamage(int damage, IUnit source, IUnit target)
    {
        var heal = new[] { 1, damage / 2, !damageTaken && Duration == 1 ? (int)Math.Floor(damage * 0.8) : damage / 2 }[upgradeLevel];
        
        if (source == Target)
        {
            Target.Heal(heal);
        }
    }
}