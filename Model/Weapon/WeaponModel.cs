using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mercenary_data_editor.Model.Weapon;

public enum Weapons
{
  ballista, bamboo_spear, baseball_bat,
  bow, frying_pan, granade_launcher,
  hand_gun, katana, knife,
  lance, lazer_gun, lazer_sword,
  long_sword, mini_gun, rapier,
  rocket_launcher, shot_gun, sniper_gun,
  spear, submachine_gun
}

public class WeaponModelData
{
  public string status { get; set; }
  public float value { get; set; }
}

public class AttributeData
{
  public string attribute { get; set; }
}

public class WeaponModel
{
  public IList<string> weaponTypes => Utility.GetEnumNames<Weapons>();

  public IList<string> attributes => Utility.GetEnumNames<Attribute>();

  public event Action<string> onWeaponTypeChanged;

  private string _weaponType;

  public string weaponType
  {
    get => _weaponType;
    set
    {
      _weaponType = value;
      onWeaponTypeChanged?.Invoke(value);
    }
  }

  public ObservableCollection<AttributeData> attributeList { get; set; } = new();

}

public class WeaponModel2
{
  public IList<string> statusTypes => Utility.GetEnumNames<WeaponStatusItem>();
  
  public ObservableCollection<WeaponModelData> list { get; set; } = new();
}