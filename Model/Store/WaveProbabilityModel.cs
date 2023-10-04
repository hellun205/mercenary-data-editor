using System.Collections.ObjectModel;

namespace mercenary_data_editor.Model.Store;

public class WaveProbabilityModelData
{
  public int wave { get; set; }
  public float probability { get; set; }
}

public class WaveProbabilityModel
{
  public ObservableCollection<WaveProbabilityModelData> probabilities { get; set; } = new();
}