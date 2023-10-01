using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor.Model.Spawn
{
  public class WaveTime
  {
    public int wave { get; set; }
    public float second { get; set; }

    public WaveTime(int wave, float second)
    {
      this.wave = wave;
      this.second = second;
    }
  }
}
