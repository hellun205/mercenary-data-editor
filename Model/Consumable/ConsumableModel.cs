using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using mercenary_data_editor.Model.Item;

namespace mercenary_data_editor.Model.Consumable;

public class ConsumableModelData
{
  public string status { get; set; }

  public string value { get; set; }
}

public class ConsumableModel
{
  private string _itemName;

  public string itemName
  {
    get => _itemName;
    set
    {
      _itemName = value;
      onItemNameChanged?.Invoke(value);
    }
  }

  public event Action<string> onItemNameChanged;

  public IList<string> statusTypes => Utility.GetEnumNames<ConsumableApplyStatus>();

  public ObservableCollection<ConsumableModelData> list { get; set; } = new();
}