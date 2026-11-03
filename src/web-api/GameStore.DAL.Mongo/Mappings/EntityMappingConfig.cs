namespace GameStore.DAL.Mongo.Mappings
{
    internal record EntityMappingConfig(string Prefix, Func<object, string> GetSurrogateKey);
}