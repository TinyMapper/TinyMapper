using System;
using System.Collections.Generic;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact]
        public void Test()
        {
            TinyMapper.Bind<EntityTypeModel, EntityType>(config =>
            {
                config.Bind(target => target.EntityMappingTypes, typeof(List<EntityMapppingType>));
            });

            TinyMapper.Bind<EntityType, EntityTypeModel>(config => 
            {
                config.Bind(target => target.EntityMappingTypes, typeof(List<EntityMapppingTypeModel>));
            });

            var entityType = new EntityType
            {
                CreateDate = DateTime.Now,
                Id = 1,
                Type = "MyType",
                EntityMappingTypes = new List<EntityMapppingType> { new EntityMapppingType { Value = 5 } }
            };

            EntityTypeModel entityTypeModel = TinyMapper.Map<EntityType, EntityTypeModel>(entityType);
            EntityType newEntityType = TinyMapper.Map<EntityTypeModel, EntityType>(entityTypeModel);
        }


        public partial class EntityType
        {
            public EntityType()
            {
                EntityMappingTypes = new List<EntityMapppingType>();
            }

            public int Id { get; set; }
            public string Type { get; set; }
            public DateTime CreateDate { get; set; }
            public int? CreateBy { get; set; }
            public virtual ICollection<EntityMapppingType> EntityMappingTypes { get; set; }
        }


        public sealed class EntityMapppingType
        {
            public int Value { get; set; }
        }


        public sealed class EntityMapppingTypeModel
        {
            public int Value { get; set; }
        }


        public class EntityTypeModel
        {
            public EntityTypeModel()
            {
                EntityMappingTypes = new List<EntityMapppingTypeModel>();
            }

            public int Id { get; set; }
            public string Type { get; set; }
            public DateTime CreateDate { get; set; }
            public int? CreateBy { get; set; }
            public List<EntityMapppingTypeModel> EntityMappingTypes { get; set; }
        }
    }
}
