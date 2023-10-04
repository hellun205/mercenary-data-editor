using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.TierProbability;

namespace mercenary_data_editor.Components;

public partial class TierProbabilities : UserControl
{
  public TierProbabilityModel model { get; }
  
  public TierProbabilities()
  {
    InitializeComponent();
    model = new TierProbabilityModel();
    DataContext = model;
  }

  public void Add(float probability)
  {
    model.probabilities.Add(new TierProbabilityModelData
    {
      tier = model.probabilities.Count,
      probability = probability
    });
  }

  private void Add_OnClick(object sender, RoutedEventArgs e)
  {
    Add(model.probabilities.Count > 0 ? model.probabilities[^1].probability : 0);
  }

  private void Remove_OnClick(object sender, RoutedEventArgs e)
  {
    if (c_list.SelectedIndex == -1) return;
    for (var i = c_list.SelectedIndex + 1; i < model.probabilities.Count; i++)
      model.probabilities[i] = new TierProbabilityModelData
      {
        probability = model.probabilities[i].probability,
        tier = model.probabilities[i].tier - 1
      };
    model.probabilities.RemoveAt(c_list.SelectedIndex);
  }

  public void Clear()
  {
    model.probabilities.Clear();
  }
}