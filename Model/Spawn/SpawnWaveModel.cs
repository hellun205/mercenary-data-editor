using mercenary_data_editor.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace mercenary_data_editor
{
  public class SpawnWaveModel
  {
    public IList<string> types => enemyData.Select(x => ((EnemyData) x.Content).model.name).ToList();

    public ObservableCollection<Enemy> enemies { get; set; } = new ObservableCollection<Enemy>();

    public List<TabItem> enemyData;

    public SpawnWaveModel(List<TabItem> enemyData)
    {
      this.enemyData = enemyData;
    }
  }
}
