using System.Windows;
using System.Windows.Controls;
using mercenary_data_editor.Model.Store;

namespace mercenary_data_editor.Components;

public partial class WaveProbability : UserControl
{
  public WaveProbabilityModel model { get; }

  public WaveProbability()
  {
    InitializeComponent();
    model = new WaveProbabilityModel();
    DataContext = model;
  }


  public void Add(float probability)
  {
    model.probabilities.Add(new WaveProbabilityModelData
    {
      wave = model.probabilities.Count,
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
      model.probabilities[i] = new WaveProbabilityModelData
      {
        probability = model.probabilities[i].probability,
        wave = model.probabilities[i].wave - 1
      };
    model.probabilities.RemoveAt(c_list.SelectedIndex);
  }

  public void Clear()
  {
    model.probabilities.Clear();
  }
}