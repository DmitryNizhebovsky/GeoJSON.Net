﻿// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Collections.ObjectModel;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoJSON.Net.Converters;

/// <summary>
/// Converts <see cref="IGeometryObject"/> types to and from JSON.
/// </summary>
public class GeometryConverter : JsonConverter
{
    public override bool CanWrite => false;

    /// <summary>
    ///     Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType)
    {
        return typeof(IGeometryObject).IsAssignableFrom(objectType);
    }

    /// <summary>
    ///     Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>
    ///     The object value.
    /// </returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                return null;
            case JsonToken.StartObject:
                var value = JObject.Load(reader);
                return ReadGeoJson(value);
            case JsonToken.StartArray:
                var values = JArray.Load(reader);
                var geometries = new ReadOnlyCollection<IGeometryObject>(
                    values.Cast<JObject>().Select(ReadGeoJson).ToArray());
                return geometries;
        }

        throw new JsonReaderException("expected null, object or array token but received " + reader.TokenType);
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // IGeometryObject can be written without a problem
        throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
    }

    /// <summary>
    /// Reads the geo json.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="JsonReaderException">
    /// json must contain a "type" property
    /// or
    /// type must be a valid geojson geometry object type
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Feature and FeatureCollection types are Feature objects and not Geometry objects
    /// </exception>
    private static IGeometryObject ReadGeoJson(JObject value)
    {
        if (!value.TryGetValue("type", StringComparison.OrdinalIgnoreCase, out JToken token))
        {
            throw new JsonReaderException("json must contain a \"type\" property");
        }

        if (!Enum.TryParse(token.Value<string>(), true, out GeoJSONObjectType geoJsonType))
        {
            throw new JsonReaderException("type must be a valid geojson geometry object type");
        }

        return geoJsonType switch
        {
            GeoJSONObjectType.Point => value.ToObject<Point>(),
            GeoJSONObjectType.MultiPoint => value.ToObject<MultiPoint>(),
            GeoJSONObjectType.LineString => value.ToObject<LineString>(),
            GeoJSONObjectType.MultiLineString => value.ToObject<MultiLineString>(),
            GeoJSONObjectType.Polygon => value.ToObject<Polygon>(),
            GeoJSONObjectType.MultiPolygon => value.ToObject<MultiPolygon>(),
            GeoJSONObjectType.Circle => value.ToObject<Circle>(),
            GeoJSONObjectType.GeometryCollection => value.ToObject<GeometryCollection>(),
            _ => throw new NotSupportedException("Feature and FeatureCollection types are Feature objects and not Geometry objects")
        };
    }
}
