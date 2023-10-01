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

namespace mercenary_data_editor.Components {
  /// <summary>
  /// Interaction logic for SpawnWave.xaml
  /// </summary>
  public partial class SpawnWave : UserControl {
    private SpawnWaveModel _model;
    public SpawnWaveModel model {
      get => _model; set {
        _model = value;
        this.DataContext = _model;
      }
    }

    public SpawnWave(List<TabItem> enemies) {
      InitializeComponent();
      model = new SpawnWaveModel(enemies);
    }

    private void Add_OnClick(object sender, RoutedEventArgs e) {
      model.enemies.Add(new Enemy("", 0));
    }

    private void Remove_OnClick(object sender, RoutedEventArgs e) {
      try {
        model.enemies.RemoveAt(c_list.SelectedIndex);
      }
      catch {
      }
    }
  }
}
