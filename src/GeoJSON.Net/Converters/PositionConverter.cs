// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Converters;

/// <summary>
/// Converter to read and write an <see cref="IPosition" />, 
/// that is, the coordinates of a <see cref="Point" />.
/// </summary>
public class PositionConverter : JsonConverter
{
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// <see langword="true"/> if this instance can convert the specified object type; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool CanConvert(Type objectType)
    {
        return typeof(IPosition).IsAssignableFrom(objectType);
    }

    /// <summary>
    /// Reads the JSON representation of the object.
    /// </summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>
    /// The object value.
    /// </returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        double[] coordinates;

        try
        {
            coordinates = serializer.Deserialize<double[]>(reader);
        }
        catch (Exception e)
        {
            throw new JsonReaderException("error parsing coordinates", e);
        }
        return coordinates?.ToPosition() ?? throw new JsonReaderException("coordinates cannot be null");
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is IPosition coordinates)
        {
            writer.WriteStartArray();

            writer.WriteValue(coordinates.Longitude);
            writer.WriteValue(coordinates.Latitude);

            writer.WriteEndArray();
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
