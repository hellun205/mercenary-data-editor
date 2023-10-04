using System;
using System.Collections.Generic;
using System.Linq;

namespace mercenary_data_editor.Model.Partner;

[Serializable]
public class PartnerData
  : IData<PartnerData,
    Dictionary<string, Dictionary<Attribute, Dictionary<int, (PartnerData.Status status, string value)[]>>>>
{
  [Flags]
  public enum Status
  {
    Damage = 1 << 0, AttackSpeed = 1 << 1, MultipleCritical = 1 << 2,
    Range = 1 << 3, BleedingDamage = 1 << 4, Knockback = 1 << 5,
    PenetrateCount = 1 << 6, ErrorRange = 1 << 7, ExplosionRange = 1 << 8,
    CriticalPercent = 1 << 9, Price = 1 << 10
  }

  [Serializable]
  public class Partner
  {
    [Serializable]
    public class Data
    {
      [Serializable]
      public class Tier
      {
        [Serializable]
        public class ApplyStatus
        {
          public Status type;
          public string value;
        }

        public int tier;
        public ApplyStatus[] status;
      }

      public Attribute attribute;
      public Tier[] applyStatus;
    }

    public string name;
    public Data[] status;
  }

  public Partner[] data;

  public Dictionary<string, Dictionary<Attribute, Dictionary<int, (Status status, string value)[]>>> ToSimply()
    => data.ToDictionary
    (
      x => x.name,
      x => x.status.ToDictionary
      (
        y => y.attribute,
        y => y.applyStatus.ToDictionary
        (
          z => z.tier,
          z => z.status.Select(a => (a.type, a.value)).ToArray()
        )
      )
    );

  public PartnerData Parse
  (
    Dictionary<string, Dictionary<Attribute, Dictionary<int, (Status status, string value)[]>>> simplyData
  )
  {
    data = simplyData.Select(x => new Partner()
    {
      name = x.Key,
      status = x.Value.Select
      (
        y => new Partner.Data()
        {
          attribute = y.Key,
          applyStatus = y.Value.Select
          (
            z => new Partner.Data.Tier()
            {
              tier = z.Key,
              status = z.Value.Select(a => new Partner.Data.Tier.ApplyStatus()
              {
                type = a.status,
                value = a.value
              }).ToArray(),
            }
          ).ToArray()
        }
      ).ToArray()
    }).ToArray();

    return this;
  }
}
