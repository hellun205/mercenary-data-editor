using System;
using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Item;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor.Components;

public partial class Item : UserControl
{
  public ItemModel model { get; }

  public TabItem tabItem { get; }

  public Item(TabItem tabItem, string item)
  {
    InitializeComponent();
    model = new ItemModel();
    this.tabItem = tabItem;
    model.onItemTypeChanged += s => this.tabItem.Header = s;
    model.itemType = item;

    this.DataContext = model;
  }

  public void AddItem(string type, float value)
  {
    model.list.Add(new ItemModelData() { status = type, value = value });
  }
  
  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    AddItem(ItemStatusItem.Hp.ToString(), 0);
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    model.list.RemoveAt(c_list.SelectedIndex);
  }
}

