using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor.Model.Item;

public enum Items
{
  barbell, boxing_gloves, bullet,
  canine, chocolate, fortune_cookies,
  glasses, iron_ingot, leather_gloves,
  lens, needle, paper_cup,
  rainboots
}

public class ItemModelData
{
  public string status { get; set; }
  public float value { get; set; }
}

public class ItemModel
{
  public IList<string> itemTypes => Utility.GetEnumNames<Items>();

  public event Action<string> onItemTypeChanged;

  private string _itemType;

  public string itemType
  {
    get => _itemType;
    set
    {
      _itemType = value;
      onItemTypeChanged?.Invoke(value);
    }
  }
}

public class ItemModel2
{
  public IList<string> statusTypes => Utility.GetEnumNames<ItemStatusItem>();

  public ObservableCollection<ItemModelData> list { get; set; } = new();
}
