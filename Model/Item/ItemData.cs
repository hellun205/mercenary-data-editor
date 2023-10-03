using System;
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
public class ItemData : IData<ItemData, Dictionary<string, (int tier, Dictionary<ItemStatusItem, float> apply)>>
{
  [Serializable]
  public class Item
  {
    [Serializable]
    public class Apply
    {
      public ItemStatusItem type;
      public float value;
    }

    public string name;
    public int tier;
    public Apply[] applies;
  }

  public Item[] items;

  public Dictionary<string, (int tier, Dictionary<ItemStatusItem, float> apply)>
    ToSimply()
    => items.ToDictionary
    (
      x => x.name,
      x =>
        (x.tier, x.applies.ToDictionary
        (
          y => y.type,
          y => y.value
        ))
    );


  public ItemData Parse
  (
    Dictionary<string, (int tier, Dictionary<ItemStatusItem, float> apply)> simplyData
  )
  {
    items = simplyData.Select
    (
      x => new Item()
      {
        name = x.Key,
        tier = x.Value.tier,
        applies = x.Value.apply.Select
        (
          y => new Item.Apply()
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
