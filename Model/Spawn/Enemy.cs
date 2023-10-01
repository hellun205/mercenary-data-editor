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

    public Enemy(string name, int count = 0)
    {
      this.name = name;
      this.count = count;
    }
  }
}
