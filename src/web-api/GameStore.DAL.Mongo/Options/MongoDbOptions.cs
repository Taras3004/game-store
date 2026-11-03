namespace GameStore.DAL.Mongo.Options
{
    public record MongoDbOptions
    {
        public const string SectionName = "MongoDbOptions";

        public required string ConnectionString { get; init; }
        public required string DatabaseName { get; init; }
    }
}