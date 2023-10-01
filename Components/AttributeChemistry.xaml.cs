using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
  /// Interaction logic for AttributeChemistry.xaml
  /// </summary>
  public partial class AttributeChemistry : UserControl {
    private AttributeChemistryModel _model;
    public AttributeChemistryModel model {
      get => _model; set {
        _model = value;
        this.DataContext = _model;
      }
    }

    public AttributeChemistry() {
      InitializeComponent();
      model = new AttributeChemistryModel();
      c_type.ItemsSource = Enum.GetNames(typeof(Attribute)).ToList();
    }

    public TabItem tabItem;

    private void Add_OnClick(object sender, RoutedEventArgs e) {
      model.attributes.Add(new AttributeChemistryItem(0, "", 0f));
    }

    private void Remove_OnClick(object sender, RoutedEventArgs e) {
      try {
        model.attributes.RemoveAt(c_list.SelectedIndex);
      }
      catch {
      }
    }

    private void c_type_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      tabItem.Header = ((ComboBox)sender).SelectedItem;
    }
  }
}
