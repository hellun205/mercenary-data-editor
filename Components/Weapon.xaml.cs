using System;
using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor.Components;

public partial class Weapon : UserControl
{
  public WeaponModel model { get; }

  public TabItem tabItem { get; }

  public Weapon(TabItem tabItem, Weapons weapon = Weapons.ballista, Attribute attr = Attribute.None)
  {
    InitializeComponent();
    model = new WeaponModel();
    this.tabItem = tabItem;
    model.onWeaponTypeChanged += s => this.tabItem.Header = s;
    model.weaponType = weapon.ToString();

    foreach (var a in Enum.GetValues<Attribute>())
    {
      if (a == Attribute.None) continue;
      if (attr.HasFlag(a))
        model.attributeList.Add(new AttributeData() { attribute = a.ToString() });
    }

    this.DataContext = model;
  }

  public void AddItem((WeaponStatusItem status, float value)[] data = null)
  {
    var item = new TabItem()
    {
      Header = c_tab.Items.Count
    };
    var child = new Weapon2()
    {
      Margin = new Thickness(0, 0, 0, 0),
    };

    if (data != null)
    {
      foreach (var (status, value) in data)
      {
        child.model.list.Add(new WeaponModelData()
        {
          status = status.ToString(),
          value = value
        });
      }
    }
    else if (c_tab.Items.Count > 0)
    {
      foreach (var x in ((Weapon2) ((TabItem) c_tab.Items[^1]).Content).model.list)
      {
        child.model.list.Add(new WeaponModelData()
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

  private void AddAttribute_OnClick(object sender, RoutedEventArgs e)
  {
    model.attributeList.Add(new AttributeData() { attribute = Attribute.None.ToString() });
  }

  private void RemoveAttribute_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_attributes.SelectedIndex == -1) return;
    model.attributeList.RemoveAt(c_attributes.SelectedIndex);
  }
}
