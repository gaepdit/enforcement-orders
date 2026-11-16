namespace Enfo.Domain.BaseEntities;

public interface ISearchDto : ISorting, IRouteValues;

public interface ISorting
{
    public string SortByName { get; }
}

public interface IRouteValues
{
    IDictionary<string, string> AsRouteValues();
}
