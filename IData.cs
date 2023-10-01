using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mercenary_data_editor
{
  public interface IData<TSource, TSimplyData>
  {
    public TSimplyData ToSimply();

    public TSource Parse(TSimplyData simplyData);
  }
}
