using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace ProductApi.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public bool IsLive { get; set; }


        public string CategoryId { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }
        


    }
}
