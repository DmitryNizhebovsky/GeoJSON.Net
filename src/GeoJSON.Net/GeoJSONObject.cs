// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Collections.Generic;
using GeoJSON.Net.Converters;
using GeoJSON.Net.CoordinateReferenceSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GeoJSON.Net;

/// <summary>
///     Base class for all IGeometryObject implementing types
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public abstract class GeoJSONObject : IGeoJSONObject, IEqualityComparer<GeoJSONObject>, IEquatable<GeoJSONObject>
{
    internal static readonly DoubleTenDecimalPlaceComparer DoubleComparer = new();

    /// <summary>
    ///     Gets or sets the (optional)
    ///     <see href="https://tools.ietf.org/html/rfc7946#section-4">
    ///         Coordinate Reference System Object.
    ///     </see>
    /// </summary>
    /// <value>
    ///     The Coordinate Reference System Objects.
    /// </value>
    [JsonProperty(PropertyName = "crs", Required = Required.Default, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        NullValueHandling = NullValueHandling.Include)]
    [JsonConverter(typeof(CrsConverter))]
    //[DefaultValue(typeof(DefaultCRS), "")]
    public ICRSObject CRS { get; set; }

    /// <summary>
    ///     The (mandatory) type of the
    ///     <see href="https://tools.ietf.org/html/rfc7946#section-3">GeoJSON Object</see>.
    /// </summary>
    [JsonProperty(PropertyName = "type", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
    [JsonConverter(typeof(StringEnumConverter))]
    public abstract GeoJSONObjectType Type { get; }

    #region IEqualityComparer, IEquatable

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public override bool Equals(object obj)
    {
        return Equals(this, obj as GeoJSONObject);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public bool Equals(GeoJSONObject other)
    {
        return Equals(this, other);
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public bool Equals(GeoJSONObject left, GeoJSONObject right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (right is null)
        {
            return false;
        }

        if (left.Type != right.Type)
        {
            return false;
        }

        return Equals(left.CRS, right.CRS);
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public static bool operator ==(GeoJSONObject left, GeoJSONObject right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (right is null)
        {
            return false;
        }
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether the specified object instances are not considered equal
    /// </summary>
    public static bool operator !=(GeoJSONObject left, GeoJSONObject right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hash code for this instance
    /// </summary>
    public override int GetHashCode()
    {
        return ((int)Type).GetHashCode();
    }

    /// <summary>
    /// Returns the hash code for the specified object
    /// </summary>
    public int GetHashCode(GeoJSONObject obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}
