using GameStore.DAL.Mongo.Entities.ConfigurationEntities;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// Entry point for every Northwind read. Decorators append aggregation stages that repair
    /// broken legacy documents on the server, so filters always run against normalized data.
    /// </summary>
    internal interface INorthwindQuerySource<TDocument>
        where TDocument : NorthwindEntity
    {
        IQueryable<TDocument> Query();
    }
}
