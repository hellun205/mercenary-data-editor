using System.Collections.ObjectModel;

namespace mercenary_data_editor.Model.RefreshPrice;

public class RefreshPriceModelData
{
  public int count { get; set; }
  public int price { get; set; }
}

public class RefreshPriceModel
{
  public ObservableCollection<RefreshPriceModelData> prices { get; set; } = new();
}