using System.Text.RegularExpressions;

namespace CooleWebapp.Core.Utilities;

public static class Globbing
{
  /// <summary>
  /// Compares the string against a given pattern.
  /// </summary>
  /// <param name="str">The string.</param>
  /// <param name="pattern">
  /// The pattern to match, where "*" means any sequence of characters
  /// and "?" means any single character.
  /// </param>
  /// <returns>
  /// <c>true</c> if the string matches the given pattern; otherwise <c>false</c>.
  /// </returns>
  public static bool IsLike(this string str, string pattern) =>
    new Regex($"^{Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".")}$",
      RegexOptions.IgnoreCase | RegexOptions.Singleline
    ).IsMatch(str);
}
