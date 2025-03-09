using System.ComponentModel.DataAnnotations;

namespace CooleWebapp.Core.Entities;

/// <summary>
/// Application user data
/// </summary>
public record CoolUser
{
  /// <summary>
  /// Unique id that identifies the user
  /// </summary>
  public long Id { get; set; }
  /// <summary>
  /// The id of the related WebappUser that contains login information
  /// </summary>
  public string? WebappUserId { get; set; } = string.Empty;
  /// <summary>
  /// The name of the user
  /// </summary>
  [MaxLength(256)]
  public string Name { get; set; } = string.Empty;
  /// <summary>
  /// The title of the user (e.g. Lord)
  /// </summary>
  [MaxLength(256)]
  public string? Title { get; set; }
  /// <summary>
  /// The three letter initials of the user
  /// </summary>
  [MaxLength(3)]
  public string Initials { get; set; } = string.Empty;
  /// <summary>
  /// Flag that tells if the user has been deleted
  /// </summary>
  public bool IsDeleted { get; set; } = false;

  public ICollection<Order>? Orders { get; set; }
  public ICollection<Deposit>? Deposits { get; set; }
  public Balance? Balance { get; set; }
  public UserSettings? UserSettings { get; set; }
}
