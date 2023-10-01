using System;
using System.Collections.Generic;

namespace mercenary_data_editor;

public static class Utility
{
  public static T ParseEnum<T>(this string text) where T : struct, Enum
    => (T) Enum.Parse<T>(text);

  public static string[] GetEnumNames<T>() where T: struct, Enum
    => Enum.GetNames<T>();
}
