using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoJSON.Net.Converters;

internal class FeatureCollectionItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(IFeatureCollectionItem<IGeometryObject>).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.StartArray:
                var values = JArray.Load(reader);
                var features = new List<IFeatureCollectionItem<IGeometryObject>>(
                    values.Cast<JObject>().Select(ReadGeoJson).ToArray());
                return features;
        }

        throw new JsonReaderException("expected array token but received " + reader.TokenType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }

    /// <summary>
    /// Reads the geo json.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="JsonReaderException">
    /// json must contain a "type" property
    /// or
    /// type must be a valid geojson object type
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Only Feature and Cluster types are supported
    /// </exception>
    private static IFeatureCollectionItem<IGeometryObject> ReadGeoJson(JObject value)
    {
        if (!value.TryGetValue("type", StringComparison.OrdinalIgnoreCase, out JToken token))
        {
            throw new JsonReaderException("json must contain a \"type\" property");
        }

        if (!Enum.TryParse(token.Value<string>(), true, out GeoJSONObjectType geoJsonType))
        {
            throw new JsonReaderException("type must be a valid geojson object type");
        }

        return geoJsonType switch
        {
            GeoJSONObjectType.Feature => value.ToObject<Feature.Feature>(),
            GeoJSONObjectType.Cluster => value.ToObject<Cluster>(),
            _ => throw new NotSupportedException("Only Feature and Cluster types are supported")
        };
    }
}
