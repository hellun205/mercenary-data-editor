using System.Collections.Generic;
using mercenary_data_editor.Model.Player;

namespace mercenary_data_editor.Model;

public class BaseModel
{
  public IList<string> playerStatusTypes => Utility.GetEnumNames<PlayerStatusItem>();
}
