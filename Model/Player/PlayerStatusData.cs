using System;
using System.Collections.Generic;
using System.Linq;

namespace mercenary_data_editor.Model.Player;

public enum PlayerStatusItem
{
  MaxHp, Hp, Regeneration,
  DrainHp, MeleeDamage, RangedDamage,
  BleedingDamage, CriticalPercent, AttackSpeed,
  Range, KnockBack, ExplosionRange,
  Armor, MoveSpeed, Luck,
  EvasionRate, CoinDetectRange, Coin
}

[Serializable]
public class PlayerStatusData : IData<PlayerStatusData, Dictionary<PlayerStatusItem, float>>
{
  [Serializable]
  public class Status
  {
    public PlayerStatusItem type;
    public float value;
  }

  public Status[] status;

  public Dictionary<PlayerStatusItem, float> ToSimply() => status.ToDictionary(x => x.type, x => x.value);

  public PlayerStatusData Parse(Dictionary<PlayerStatusItem, float> simplyData)
  {
    status = simplyData.Select(x => new Status() { type = x.Key, value = x.Value }).ToArray();
    return this;
  }
}
