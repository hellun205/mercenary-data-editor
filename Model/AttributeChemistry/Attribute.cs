using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  [Flags]
  public enum Attribute
  {
    None = 0, Sword = 1 << 0, Sharpness = 1 << 1,
    Spear = 1 << 2, Lazer = 1 << 3, Bluntness = 1 << 4,
    Heavy = 1 << 5, Gun = 1 << 6, AutomaticFire = 1 << 7,
    Explosion = 1 << 8,
  }
}
