using DotCruz.Notifications.Domain.Entities.Base;
using DotCruz.Notifications.Domain.Entities.Notifications;
using DotCruz.Notifications.Domain.Entities.Templates;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Mappings;

public static class MongoDbMappings
{
    public static void Configure()
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfDefaultConvention(true)
        };
        ConventionRegistry.Register("DotCruzConventions", pack, t => true);

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.Document));

        if (!BsonClassMap.IsClassMapRegistered(typeof(Notification)))
        {
            BsonClassMap.RegisterClassMap<Notification>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.AddKnownType(typeof(EmailNotification));
                cm.AddKnownType(typeof(PushNotification));
                cm.AddKnownType(typeof(SmsNotification));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
            BsonClassMap.RegisterClassMap<BaseEntity>(cm => cm.AutoMap());

        if (!BsonClassMap.IsClassMapRegistered(typeof(Template)))
            BsonClassMap.RegisterClassMap<Template>(cm => cm.AutoMap());
            
        if (!BsonClassMap.IsClassMapRegistered(typeof(EmailNotification)))
            BsonClassMap.RegisterClassMap<EmailNotification>(cm => cm.AutoMap());

        if (!BsonClassMap.IsClassMapRegistered(typeof(PushNotification)))
            BsonClassMap.RegisterClassMap<PushNotification>(cm => cm.AutoMap());

        if (!BsonClassMap.IsClassMapRegistered(typeof(SmsNotification)))
            BsonClassMap.RegisterClassMap<SmsNotification>(cm => cm.AutoMap());
    }
}
