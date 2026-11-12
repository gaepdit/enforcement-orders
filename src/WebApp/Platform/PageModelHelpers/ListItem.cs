using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enfo.WebApp.Platform.PageModelHelpers;

public record ListItem<TKey>(TKey Id, string Name) where TKey : IEquatable<TKey>;

public record ListItem(Guid Id, string Name) : ListItem<Guid>(Id, Name);

/// <summary>
/// Extension for converting item lists to the data structure used by SELECT elements.
/// </summary>
public static class SelectListExtensions
{
    public static SelectList ToSelectList<TKey>(this IEnumerable<ListItem<TKey>> listItems)
        where TKey : IEquatable<TKey> =>
        new(listItems, nameof(ListItem<TKey>.Id), nameof(ListItem<TKey>.Name));
}
