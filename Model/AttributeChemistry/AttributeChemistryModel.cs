using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  public class AttributeChemistryModel
  {
    public IList<string> types => Enum.GetNames(typeof(ApplyStatus));

    public ObservableCollection<AttributeChemistryItem> attributes { get; set; } = new();
  }
}
