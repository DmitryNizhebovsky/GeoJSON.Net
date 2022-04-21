using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Converters;

internal class BoundingBoxConverter : JsonConverter
{
    private static readonly PositionEnumerableConverter LineStringConverter = new();

    public override bool CanConvert(Type objectType)
    {
        return typeof(BoundingBox).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var positions = ((IEnumerable<IPosition>) LineStringConverter.ReadJson(reader, objectType, existingValue, serializer)).ToArray();

        return new BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            (Position) positions[0],
            (Position) positions[1]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is BoundingBox bbox)
        {
            writer.WriteStartArray();

            writer.WriteStartArray();
            writer.WriteValue(bbox.From.Longitude);
            writer.WriteValue(bbox.From.Latitude);
            writer.WriteEndArray();

            writer.WriteStartArray();
            writer.WriteValue(bbox.To.Longitude);
            writer.WriteValue(bbox.To.Latitude);
            writer.WriteEndArray();

            writer.WriteEndArray();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
