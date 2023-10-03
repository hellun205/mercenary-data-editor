using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  public class Enemy
  {
    public string name { get; set; }

    public int count { get; set; }

    private int _simultaneousSpawnCount;

    public int simultaneousSpawnCount
    {
      get => _simultaneousSpawnCount;
      set => _simultaneousSpawnCount = Math.Max(1, value);
    }

    private float _delay;

    public float delay
    {
      get => _delay;
      set => _delay = Math.Max(0f, value);
    }
    
    private float _range;

    public float range
    {
      get => _range;
      set => _range = Math.Max(0f, value);
    }

    public Enemy(string name, int count, int simultaneousSpawnCount, float delay, float range)
    {
      this.name = name;
      this.count = count;
      this.simultaneousSpawnCount = simultaneousSpawnCount;
      this.delay = delay;
      this.range = range;
    }
  }
}
