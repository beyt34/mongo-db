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
                AttributeB = entity.AttributeB,
                AttributeC = entity.AttributeC,
                AttributeD = entity.AttributeD,
                AttributeE = entity.AttributeE,
                AttributeF = entity.AttributeF,
                AttributeG = entity.AttributeG,
                AttributeH = entity.AttributeH,
                AttributeI = entity.AttributeI,
                AttributeJ = entity.AttributeJ,
                AttributeK = entity.AttributeK,
                AttributeL = entity.AttributeL,
                AttributeM = entity.AttributeM,
                AttributeN = entity.AttributeN,
                AttributeO = entity.AttributeO,
                AttributeP = entity.AttributeP,
                AttributeQ = entity.AttributeQ,
                AttributeR = entity.AttributeR,
                AttributeS = entity.AttributeS,
                AttributeT = entity.AttributeT,
                AttributeU = entity.AttributeU,
                AttributeV = entity.AttributeV,
                AttributeW = entity.AttributeW,
                AttributeX = entity.AttributeX,
                AttributeY = entity.AttributeY,
                AttributeZ = entity.AttributeZ,
                ChangeDateTime = DateTime.UtcNow
            };
        }
    }
}