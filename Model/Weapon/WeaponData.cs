using System;
using System.Collections.Generic;
using System.Linq;

namespace mercenary_data_editor.Model.Weapon;

public enum WeaponStatusItem
{
  Damage, AttackSpeed, MultipleCritical,
  Range, Knockback, BleedingDamage,
  PenetrateCount, BulletCount, ErrorRange,
  BulletSpeed, ExplosionRange, IncreaseDamageWhenStop,
  IncreaseEvasionRate, IncreaseBleedingDamagePercent, IncreaseMoveSpeedPercent,
  Price
}

[Serializable]
public class WeaponData
  : IData
  <
    WeaponData,
    Dictionary<string, (Attribute attribute, Dictionary<int, Dictionary<WeaponStatusItem, float>>tiers)>
  >
{
  [Serializable]
  public class Weapon
  {
    [Serializable]
    public class Tier
    {
      [Serializable]
      public class Apply
      {
        public WeaponStatusItem type;
        public float value;
      }

      public int tier;
      public Apply[] status;
    }

    public string name;
    public Attribute attribute;
    public Tier[] tiers;
  }

  public Weapon[] weapons;

  public Dictionary<string, (Attribute attribute, Dictionary<int, Dictionary<WeaponStatusItem, float>> tiers)>
    ToSimply()
    => weapons.ToDictionary
    (
      x => x.name,
      x =>
      (
        x.attribute,
        x.tiers.ToDictionary
        (
          y => y.tier,
          y => y.status.ToDictionary
          (
            z => z.type,
            z => z.value
          )
        )
      )
    );

  public WeaponData Parse
  (
    Dictionary<string, (Attribute attribute, Dictionary<int, Dictionary<WeaponStatusItem, float>> tiers)> simplyData
  )
  {
    weapons = simplyData.Select
    (
      x => new Weapon()
      {
        name = x.Key,
        attribute = x.Value.attribute,
        tiers = x.Value.tiers.Select
        (
          y => new Weapon.Tier()
          {
            tier = y.Key,
            status = y.Value.Select
            (
              z => new Weapon.Tier.Apply()
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
