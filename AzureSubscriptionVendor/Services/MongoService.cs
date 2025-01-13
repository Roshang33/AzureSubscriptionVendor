using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureSubscriptionVendor.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<BsonDocument>(collectionName);
        }

        public async Task StoreJsonAsync(string json)
        {
            var document = BsonDocument.Parse(json);
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateJsonAsync(string json, string subscriptionName)
        {
            // Convert JSON payload to a BsonDocument for update
            var updateDocument = BsonDocument.Parse(json);

            // Build the update filter
            var filter = Builders<BsonDocument>.Filter.Eq("subscriptionname", subscriptionName);
            var update = new BsonDocument("$set", updateDocument);

            // Perform the update
            var updateResult = await _collection.UpdateOneAsync(filter, update);

            // Check if the update was successful
            if (updateResult.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"No document found with subscriptionname '{subscriptionName}'.");
            }
        }
    }
}
