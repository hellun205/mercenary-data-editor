using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mercenary_data_editor.Model.Partner;

[Flags]
public enum PartnerStatus
{
  Damage = 1 << 0, AttackSpeed = 1 << 1, MultipleCritical = 1 << 2,
  Range = 1 << 3, BleedingDamage = 1 << 4, Knockback = 1 << 5,
  PenetrateCount = 1 << 6, ErrorRange = 1 << 7, ExplosionRange = 1 << 8,
  CriticalPercent = 1 << 9
}

public class PartnerStatusData
{
  public string attribute { get; set; }
  public int tier { get; set; }
  public string status { get; set; }
  public string value { get; set; }

  public PartnerStatusData() : this(Attribute.None.ToString(), 0, PartnerStatus.Damage.ToString(), "0")
  {
  }

  public PartnerStatusData(string attribute, int tier, string status, string value)
  {
    this.attribute = attribute;
    this.tier = tier;
    this.status = status;
    this.value = value;
  }
}

public enum PartnerType
{
  knight, assassin, spearman,
  futherperson, striker, brute,
  marksman, specialForce, bomber
}

public class PartnerModel
{
  public IList<string> partnerTypes => Utility.GetEnumNames<PartnerType>();
  public IList<string> types => Utility.GetEnumNames<PartnerStatus>();
  public IList<string> attributes => Utility.GetEnumNames<Attribute>();

  public event Action<string> onNameChanged;

  private string _name;

  public string name
  {
    get => _name;
    set
    {
      _name = value;
      onNameChanged?.Invoke(value);
    }
  }

  public ObservableCollection<PartnerStatusData> list { get; set; } = new();
}
