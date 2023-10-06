using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Consumable;

namespace mercenary_data_editor.Components;

public partial class Consumable : UserControl
{
  public ConsumableModel model { get; }

  public TabItem tabItem { get; }

  public Consumable(TabItem tabItem, string item)
  {
    InitializeComponent();
    model = new ConsumableModel();
    this.tabItem = tabItem;
    model.onItemNameChanged += s => this.tabItem.Header = s;
    model.itemName = item;

    this.DataContext = model;
  }

  public void AddItem(string type, string value)
  {
    model.list.Add(new ConsumableModelData { status = type, value = value });
  }
  
  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    AddItem(ConsumableApplyStatus.Hp.ToString(), "0");
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    model.list.RemoveAt(c_list.SelectedIndex);
  }
}