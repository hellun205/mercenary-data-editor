using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Item;

namespace mercenary_data_editor.Components;

public partial class Item2 : UserControl
{
  public ItemModel2 model { get; }

  public Item2()
  {
    InitializeComponent();
    model = new ItemModel2();
    this.DataContext = model;
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    model.list.Add(new ItemModelData() { status = ItemStatusItem.Hp.ToString(), value = 0 });
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    model.list.RemoveAt(c_list.SelectedIndex);
  }
}

