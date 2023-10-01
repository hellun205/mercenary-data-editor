using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Weapon;

namespace mercenary_data_editor.Components;

public partial class Weapon2 : UserControl
{
  public WeaponModel2 model { get; }

  public Weapon2()
  {
    InitializeComponent();
    model = new WeaponModel2();
    this.DataContext = model;
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    model.list.Add(new WeaponModelData() { status = WeaponStatusItem.Damage.ToString(), value = 0 });
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    model.list.RemoveAt(c_list.SelectedIndex);
  }
}
