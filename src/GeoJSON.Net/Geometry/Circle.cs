using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry;

/// <summary>
/// Defines the Circle type.
/// </summary>
public class Circle : GeoJSONObject, IGeometryObject, IEqualityComparer<Circle>, IEquatable<Circle>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Circle" /> class.
    /// </summary>
    /// <param name="coordinates">The Position.</param>
    /// <param name="coordinates">The radius in meters.</param>
    public Circle(IPosition coordinates, double radius)
    {
        Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        Radius = radius;
    }

    public override GeoJSONObjectType Type => GeoJSONObjectType.Circle;

    /// <summary>
    /// The <see cref="IPosition" /> underlying this point.
    /// </summary>
    [JsonProperty("coordinates", Required = Required.Always)]
    [JsonConverter(typeof(PositionConverter))]
    public IPosition Coordinates { get; }

    /// <summary>
    /// The radius of the circle in meters.
    /// </summary>
    [JsonProperty("radius", Required = Required.Always)]
    public double Radius { get; }

    #region IEqualityComparer, IEquatable

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public override bool Equals(object obj)
    {
        return Equals(this, obj as Circle);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public bool Equals(Circle other)
    {
        return Equals(this, other);
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public bool Equals(Circle left, Circle right)
    {
        if (base.Equals(left, right))
        {
            return left.Coordinates.Equals(right.Coordinates)
                && left.Radius.Equals(right.Radius);
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public static bool operator ==(Circle left, Circle right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (right is null)
        {
            return false;
        }
        return left != null && left.Equals(right);
    }

    /// <summary>
    /// Determines whether the specified object instances are not considered equal
    /// </summary>
    public static bool operator !=(Circle left, Circle right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hash code for this instance
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Coordinates, Radius);
    }

    /// <summary>
    /// Returns the hash code for the specified object
    /// </summary>
    public int GetHashCode([DisallowNull] Circle other)
    {
        return other.GetHashCode();
    }

    #endregion
}
