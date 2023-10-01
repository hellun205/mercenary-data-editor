﻿using System;
using System.Collections.Generic;
using System.Linq;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor.Model.Item;

public enum ItemStatusItem
{
  Hp, Regenaration, DrainHp,
  MeleeDamage, RangedDamage, CriticalPercent,
  BleedingDamage, AttackSpeed, Range,
  Armor, KnockBack, MoveSpeed,
  Luck, Price, EvasionRate
}

[Serializable]
public class ItemData : IData<ItemData, Dictionary<string, Dictionary<int, Dictionary<ItemStatusItem, float>>>>
{
  [Serializable]
  public class Item
  {
    [Serializable]
    public class Tier
    {
      [Serializable]
      public class Apply
      {
        public ItemStatusItem type;
        public float value;
      }

      public int tier;
      public Apply[] status;
    }

    public string name;
    public Tier[] tiers;
  }

  public Item[] items;

  public Dictionary<string, Dictionary<int, Dictionary<ItemStatusItem, float>>>
    ToSimply()
    => items.ToDictionary
    (
      x => x.name,
      x =>
        x.tiers.ToDictionary
        (
          y => y.tier,
          y => y.status.ToDictionary
          (
            z => z.type,
            z => z.value
          )
        )
    );

  public ItemData Parse
  (
    Dictionary<string, Dictionary<int, Dictionary<ItemStatusItem, float>>> simplyData
  )
  {
    items = simplyData.Select
    (
      x => new Item()
      {
        name = x.Key,
        tiers = x.Value.Select
        (
          y => new Item.Tier()
          {
            tier = y.Key,
            status = y.Value.Select
            (
              z => new Item.Tier.Apply()
              {
                type = z.Key,
                value = z.Value
              }
            ).ToArray()
          }
        ).ToArray()
      }
    ).ToArray();

    return this;
  }
}
