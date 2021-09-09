using System;
using Mongo.Console.Entities;

namespace Mongo.Console.Extensions
{
    public static class MappingExtensions
    {
        public static Product Map(this ProductTemp entity)
        {
            return new()
            {
                Code = entity.Code,
                Name = entity.Name,
                Desc = entity.Desc,
                AttributeA = entity.AttributeA,
                ChangeDateTime = DateTime.UtcNow
            };
        }
    }
}