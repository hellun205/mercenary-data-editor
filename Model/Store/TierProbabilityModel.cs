using System.Collections.ObjectModel;
using mercenary_data_editor.Model.RefreshPrice;

namespace mercenary_data_editor.Model.TierProbability;

public class TierProbabilityModelData
{
  public int tier { get; set; }
  public float probability { get; set; }
}

public class TierProbabilityModel
{
  public ObservableCollection<TierProbabilityModelData> probabilities { get; set; } = new();
}