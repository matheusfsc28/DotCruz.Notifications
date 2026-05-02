using System.Collections;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DotCruz.Notifications.Infrastructure.DataAccess.Mappings;

public class DictionaryStringObjectBsonSerializer : SerializerBase<Dictionary<string, object>>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Dictionary<string, object> value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteStartDocument();
        foreach (var kvp in value)
        {
            context.Writer.WriteName(kvp.Key);
            SerializeValue(context, kvp.Value);
        }
        context.Writer.WriteEndDocument();
    }

    private void SerializeValue(BsonSerializationContext context, object value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
            return;
        }

        if (value is JsonElement jsonElement)
        {
            SerializeJsonElement(context, jsonElement);
        }
        else if (value is IDictionary dictionary)
        {
            context.Writer.WriteStartDocument();
            foreach (DictionaryEntry entry in dictionary)
            {
                context.Writer.WriteName(entry.Key.ToString());
                SerializeValue(context, entry.Value);
            }
            context.Writer.WriteEndDocument();
        }
        else if (value is IEnumerable list && !(value is string))
        {
            context.Writer.WriteStartArray();
            foreach (var item in list)
            {
                SerializeValue(context, item);
            }
            context.Writer.WriteEndArray();
        }
        else
        {
            // Para tipos primitivos simples, podemos usar o serializador padrão sem risco de loop
            var type = value.GetType();
            var serializer = BsonSerializer.LookupSerializer(type);
            serializer.Serialize(context, value);
        }
    }

    private void SerializeJsonElement(BsonSerializationContext context, JsonElement jsonElement)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                context.Writer.WriteStartDocument();
                foreach (var property in jsonElement.EnumerateObject())
                {
                    context.Writer.WriteName(property.Name);
                    SerializeValue(context, property.Value);
                }
                context.Writer.WriteEndDocument();
                break;

            case JsonValueKind.Array:
                context.Writer.WriteStartArray();
                foreach (var item in jsonElement.EnumerateArray())
                {
                    SerializeValue(context, item);
                }
                context.Writer.WriteEndArray();
                break;

            case JsonValueKind.String:
                context.Writer.WriteString(jsonElement.GetString());
                break;

            case JsonValueKind.Number:
                // JSON numbers are double precision by default. 
                // Using GetDouble() ensures we preserve exactly what was sent without forcing into integers unless necessary.
                context.Writer.WriteDouble(jsonElement.GetDouble());
                break;

            case JsonValueKind.True:
                context.Writer.WriteBoolean(true);
                break;

            case JsonValueKind.False:
                context.Writer.WriteBoolean(false);
                break;

            case JsonValueKind.Null:
                context.Writer.WriteNull();
                break;

            default:
                context.Writer.WriteString(jsonElement.ToString());
                break;
        }
    }

    public override Dictionary<string, object> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonDocument = BsonDocumentSerializer.Instance.Deserialize(context, args);
        return ConvertBsonDocumentToDictionary(bsonDocument);
    }

    private Dictionary<string, object> ConvertBsonDocumentToDictionary(BsonDocument document)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (var element in document)
        {
            dictionary.Add(element.Name, ConvertBsonValue(element.Value));
        }
        return dictionary;
    }

    private object ConvertBsonValue(BsonValue value)
    {
        switch (value.BsonType)
        {
            case BsonType.Document:
                return ConvertBsonDocumentToDictionary(value.AsBsonDocument);
            case BsonType.Array:
                var list = new List<object>();
                foreach (var item in value.AsBsonArray)
                {
                    list.Add(ConvertBsonValue(item));
                }
                return list;
            case BsonType.Boolean:
                return value.AsBoolean;
            case BsonType.DateTime:
                return value.ToUniversalTime();
            case BsonType.Double:
                return value.AsDouble;
            case BsonType.Int32:
                return value.AsInt32;
            case BsonType.Int64:
                return value.AsInt64;
            case BsonType.String:
                return value.AsString;
            case BsonType.ObjectId:
                return value.AsGuid;
            case BsonType.Null:
                return null;
            default:
                return value.ToString();
        }
    }
}
