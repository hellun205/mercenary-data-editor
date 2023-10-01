using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Partner;

namespace mercenary_data_editor.Components;

public partial class Partner : UserControl
{
  private PartnerModel _model;

  public PartnerModel model
  {
    get => _model;
    set
    {
      _model = value;
      this.DataContext = _model;
    }
  }

  public TabItem tabItem;

  public Partner(TabItem tabItem)
  {
    InitializeComponent();
    model = new PartnerModel();
    this.tabItem = tabItem;
    model.onNameChanged += s => this.tabItem.Header = s;
  }

  public Partner(TabItem tabItem, string name) : this(tabItem)
  {
    model.name = name;
    tabItem.Header = name;
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    model.list.Add(new PartnerStatusData());
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    model.list.RemoveAt(c_list.SelectedIndex);
  }
}
