using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  public class AttributeChemistryItem
  {
    public int count { get; set; }
    public string type { get; set; }
    public float amount { get; set; }

    public AttributeChemistryItem(int count, string type, float amount)
    {
      this.count = count;
      this.type = type;
      this.amount = amount;
    }
  }
}
