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

  public Item(TabItem tabItem, Items item = Items.barbell)
  {
    InitializeComponent();
    model = new ItemModel();
    this.tabItem = tabItem;
    model.onItemTypeChanged += s => this.tabItem.Header = s;
    model.itemType = item.ToString();

    this.DataContext = model;
  }

  public void AddItem((ItemStatusItem status, float value)[] data = null)
  {
    var item = new TabItem()
    {
      Header = c_tab.Items.Count
    };
    var child = new Item2()
    {
      Margin = new Thickness(0, 0, 0, 0),
    };

    if (data != null)
    {
      foreach (var (status, value) in data)
      {
        child.model.list.Add(new ItemModelData()
        {
          status = status.ToString(),
          value = value
        });
      }
    }
    else if (c_tab.Items.Count > 0)
    {
      foreach (var x in ((Item2) ((TabItem) c_tab.Items[^1]).Content).model.list)
      {
        child.model.list.Add(new ItemModelData()
        {
          status = x.status,
          value = x.value
        });
      }
    }

    item.Content = child;
    c_tab.Items.Add(item);
    item.Focus();
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    AddItem();
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_tab.SelectedIndex == -1) return;

    for (var i = c_tab.SelectedIndex + 1; i < c_tab.Items.Count; i++)
    {
      var t = ((TabItem) c_tab.Items[i]);
      t.Header = Convert.ToInt32(t.Header) - 1;
    }

    c_tab.Items.RemoveAt(c_tab.SelectedIndex);
  }
}

