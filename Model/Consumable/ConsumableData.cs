using System;
using System.Collections.Generic;
using System.Linq;

namespace mercenary_data_editor.Model.Consumable;

public enum ConsumableApplyStatus
{
  Hp,
  Regeneration,
  DrainHp,
  MeleeDamage,
  RangedDamage,
  CriticalPercent,
  BleedingDamage,
  AttackSpeed,
  Range,
  Armor,
  EvasionRate,
  Knockback,
  MoveSpeed,
  Luck,
  Price,
  Duration,
  Resurrection,
  KillEnemy
}

public class ConsumableData : IData<ConsumableData, Dictionary<string, Dictionary<ConsumableApplyStatus, string>>>
{
  [Serializable]
  public class Item
  {
    [Serializable]
    public class Apply
    {
      public ConsumableApplyStatus type;
      public string value;
    }

    public string name;
    public Apply[] useStatus;
  }

  public Item[] consumables;

  public Dictionary<string, Dictionary<ConsumableApplyStatus, string>> ToSimply()
    => consumables.ToDictionary
    (
      x => x.name,
      x => x.useStatus.ToDictionary
      (
        y => y.type,
        y => y.value
      )
    );

  public ConsumableData Parse(Dictionary<string, Dictionary<ConsumableApplyStatus, string>> simplyData)
  {
    consumables = simplyData.Select
    (
      x => new Item
      {
        name = x.Key,
        useStatus = x.Value.Select
        (
          y => new Item.Apply
          {
            type = y.Key,
            value = y.Value
          }
        ).ToArray()
      }
    ).ToArray();

    return this;
  }
}