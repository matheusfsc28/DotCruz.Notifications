using System.Reflection;
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
        BsonSerializer.RegisterSerializer(new DictionaryStringObjectBsonSerializer());

        if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Id);
                cm.MapMember(x => x.CreatedAt);
                cm.MapMember(x => x.UpdatedAt);
                cm.MapMember(x => x.DeletedAt);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Notification)))
        {
            BsonClassMap.RegisterClassMap<Notification>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.AddKnownType(typeof(EmailNotification));
                cm.AddKnownType(typeof(PushNotification));
                cm.AddKnownType(typeof(SmsNotification));

                cm.MapMember(x => x.ServiceId);
                cm.MapMember(x => x.Type);
                cm.MapMember(x => x.Culture);
                cm.MapMember(x => x.Recipient);
                cm.MapMember(x => x.Body);
                cm.MapMember(x => x.TemplateId);
                cm.MapMember(x => x.TemplateData);
                cm.MapMember(x => x.Status);
                cm.MapMember(x => x.ScheduledFor);
                cm.MapMember(x => x.SentAt);
                cm.MapMember(x => x.RetryCount);
                
                cm.MapField("_errors").SetElementName("errors");
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(EmailNotification)))
        {
            BsonClassMap.RegisterClassMap<EmailNotification>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Title);
                
                var ctor = typeof(EmailNotification).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (ctor != null) cm.MapConstructor(ctor);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(PushNotification)))
        {
            BsonClassMap.RegisterClassMap<PushNotification>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Title);
                
                var ctor = typeof(PushNotification).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (ctor != null) cm.MapConstructor(ctor);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(SmsNotification)))
        {
            BsonClassMap.RegisterClassMap<SmsNotification>(cm =>
            {
                cm.AutoMap();
                
                var ctor = typeof(SmsNotification).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (ctor != null) cm.MapConstructor(ctor);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Template)))
        {
            BsonClassMap.RegisterClassMap<Template>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.Code);
                cm.MapMember(x => x.Culture);
                cm.MapMember(x => x.DefaultTitle);
                cm.MapMember(x => x.Body);
                cm.MapMember(x => x.Type);

                var ctor = typeof(Template).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if (ctor != null) cm.MapConstructor(ctor);
            });
        }
    }
}
