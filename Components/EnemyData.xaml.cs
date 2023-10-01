using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mercenary_data_editor.Model.Spawn;

namespace mercenary_data_editor.Components
{
  /// <summary>
  /// Interaction logic for EnemyData.xaml
  /// </summary>
  public partial class EnemyData : UserControl
  {
    public EnemyModel model = new();

    public TabItem tabItem;

    public EnemyData(string name)
    {
      InitializeComponent();
      model.name = name;
    }

    public EnemyData
    (
      string name,
      (float hp, float damage, float moveSpeed, float dropCoin) defStatus,
      (float hp, float damage, float moveSpeed, float dropCoin) incStatus,
      EnemyDetailStatus[] detailStatus
    ) : this(name)
    {
      model.defHp = defStatus.hp;
      model.defDamage = defStatus.damage;
      model.defMoveSpeed = defStatus.moveSpeed;
      model.defDropCoin = defStatus.dropCoin;
      model.incHp = incStatus.hp;
      model.incDamage = incStatus.damage;
      model.incMoveSpeed = incStatus.moveSpeed;
      model.incDropCoin = incStatus.dropCoin;

      model.details = new ObservableCollection<EnemyDetailStatus>(detailStatus);
      this.DataContext = model;
    }

    private void c_name_LostFocus(object sender, RoutedEventArgs e)
    {
      tabItem.Header = model.name;
    }

    private void DetailsAdd_OnClick(object sender, RoutedEventArgs e)
    {
      model.details.Add(new EnemyDetailStatus());
    }

    private void DetailsRemove_OnClick(object sender, RoutedEventArgs e)
    {
      if (c_detail_list.SelectedIndex == -1) return;
      model.details.RemoveAt(c_detail_list.SelectedIndex);
    }
  }
}
