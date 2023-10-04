using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.RefreshPrice;
using mercenary_data_editor.Model.Store;

namespace mercenary_data_editor.Components;

public partial class RefreshPrice : UserControl
{
  public RefreshPriceModel model { get; }

  public RefreshPrice()
  {
    InitializeComponent();
    model = new RefreshPriceModel();
    DataContext = model;
  }

  public void Add(int price)
  {
    model.prices.Add(new RefreshPriceModelData
    {
      count = model.prices.Count,
      price = price
    });
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    Add(model.prices.Count > 0 ? model.prices[^1].price : 0);
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    for (var i = c_list.SelectedIndex + 1; i < model.prices.Count; i++)
      model.prices[i] = new RefreshPriceModelData
      {
        price = model.prices[i].price,
        count = model.prices[i].count - 1
      };
    model.prices.RemoveAt(c_list.SelectedIndex);
  }

  public void Clear()
  {
    model.prices.Clear();
  }
}