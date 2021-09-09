using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Console.Entities
{
    public class Product : ProductTemp
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ChangeDateTime { get; set; }
    }
}