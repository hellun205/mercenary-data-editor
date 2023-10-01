using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mercenary_data_editor.Model.Spawn;

public class EnemyModel
{
  public IList<string> types => Enum.GetNames(typeof(SpawnData.Enemy.Detail.DetailStatus));
  
  public string name { get; set; }

  public float defHp { get; set; }
  public float defDamage { get; set; }
  public float defMoveSpeed { get; set; }
  public float defDropCoin { get; set; }
  
  public float incHp { get; set; }
  public float incDamage { get; set; }
  public float incMoveSpeed { get; set; }
  public float incDropCoin { get; set; }

  public ObservableCollection<EnemyDetailStatus> details { get; set; } = new();
}

public class EnemyDetailStatus
{
  public string status { get; set; }

  public float value { get; set; }

  public EnemyDetailStatus(string status = null, float value = 0f)
  {
    this.value = value;
    this.status = status ?? SpawnData.Enemy.Detail.DetailStatus.Range.ToString();
  }
}
