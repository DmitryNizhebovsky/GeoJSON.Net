// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Feature;

/// <summary>
/// A GeoJSON Feature Object; generic version for strongly typed <see cref="Geometry"/>
/// and <see cref="Properties"/>
/// </summary>
/// <remarks>
/// See <see href="https://tools.ietf.org/html/rfc7946#section-3.2"/>
/// </remarks>
public class Feature<TGeometry, TProps, TOptions> : GeoJSONObject, IEquatable<Feature<TGeometry, TProps, TOptions>>
    where TGeometry : IGeometryObject
{
    [JsonConstructor]
    public Feature(TGeometry geometry, TProps properties, TOptions options, string id = null)
    {
        Geometry = geometry;
        Properties = properties;
        Options = options;
        Id = id;
    }

    public override GeoJSONObjectType Type => GeoJSONObjectType.Feature;
    
    [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; }
    
    [JsonProperty(PropertyName = "geometry", Required = Required.AllowNull)]
    [JsonConverter(typeof(GeometryConverter))]
    public TGeometry Geometry { get; }
    
    [JsonProperty(PropertyName = "properties", Required = Required.AllowNull)]
    public TProps Properties { get; }

    [JsonProperty(PropertyName = "options", Required = Required.AllowNull)]
    public TOptions Options { get; }

    #region IEquatable

    /// <summary>
    /// Equality comparer.
    /// </summary>
    /// <remarks>
    /// In contrast to <see cref="Feature.Equals(Feature)"/>, this implementation returns true only
    /// if <see cref="Id"/>, <see cref="Geometry"/>, <see cref="Properties"/> and <see cref="Options"/> are also equal.
    /// The rationale here is that a user explicitly specifying the property type most probably cares about the properties equality.
    /// </remarks>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Feature<TGeometry, TProps, TOptions> other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other)
               && string.Equals(Id, other.Id)
               && EqualityComparer<TGeometry>.Default.Equals(Geometry, other.Geometry)
               && EqualityComparer<TProps>.Default.Equals(Properties, other.Properties)
               && EqualityComparer<TOptions>.Default.Equals(Options, other.Options);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Feature<TGeometry, TProps, TOptions>) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id, Geometry, Properties, Options);
    }

    public static bool operator ==(Feature<TGeometry, TProps, TOptions> left, Feature<TGeometry, TProps, TOptions> right)
    {
        return object.Equals(left, right);
    }

    public static bool operator !=(Feature<TGeometry, TProps, TOptions> left, Feature<TGeometry, TProps, TOptions> right)
    {
        return !object.Equals(left, right);
    }

    #endregion
}

/// <summary>
/// A GeoJSON Feature Object.
/// </summary>
/// <remarks>
/// See <see href="https://tools.ietf.org/html/rfc7946#section-3.2"/>
/// </remarks>
public class Feature : Feature<IGeometryObject>
{
    [JsonConstructor]
    public Feature(IGeometryObject geometry, IDictionary<string, object> properties = null, IDictionary<string, object> options = null, string id = null) 
        : base(geometry, properties, options, id)
    {
    }

    public Feature(IGeometryObject geometry, object properties, object options, string id = null) 
        : base(geometry, properties, options, id)
    {
    }
}

/// <summary>
/// Typed GeoJSON Feature class
/// </summary>
/// <remarks>Returns correctly typed Geometry property</remarks>
/// <typeparam name="TGeometry"></typeparam>
public class Feature<TGeometry> : Feature<TGeometry, IDictionary<string, object>, IDictionary<string, object>>, IEquatable<Feature<TGeometry>>
    where TGeometry : IGeometryObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Feature" /> class.
    /// </summary>
    /// <param name="geometry">The Geometry Object.</param>
    /// <param name="properties">The properties.</param>
    /// <param name="id">The (optional) identifier.</param>
    [JsonConstructor]
    public Feature(TGeometry geometry, IDictionary<string, object> properties = null, IDictionary<string, object> options = null, string id = null)
        : base(geometry, properties ?? new Dictionary<string, object>(), options ?? new Dictionary<string, object>(), id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Feature" /> class.
    /// </summary>
    /// <param name="geometry">The Geometry Object.</param>
    /// <param name="properties">
    /// Class used to fill feature properties. Any public member will be added to feature
    /// properties
    /// </param>
    /// <param name="id">The (optional) identifier.</param>
    public Feature(TGeometry geometry, object properties, object options, string id = null)
        : this(geometry, GetDictionaryOfPublicProperties(properties), GetDictionaryOfPublicProperties(options), id)
    {
    }

    private static Dictionary<string, object> GetDictionaryOfPublicProperties(object properties)
    {
        if (properties == null)
        {
            return new Dictionary<string, object>();
        }

        return properties
            .GetType()
            .GetTypeInfo()
            .DeclaredProperties
            .Where(propertyInfo => propertyInfo.GetMethod.IsPublic)
            .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(properties, null));
    }

    #region IEquatable

    public bool Equals(Feature<TGeometry> other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (Geometry == null && other.Geometry == null)
        {
            return true;
        }

        if (Geometry == null && other.Geometry != null)
        {
            return false;
        }

        if (Geometry == null)
        {
            return false;
        }

        return EqualityComparer<TGeometry>.Default.Equals(Geometry, other.Geometry);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Feature<TGeometry>) obj);
    }

    public override int GetHashCode()
    {
        return Geometry.GetHashCode();
    }

    public static bool operator ==(Feature<TGeometry> left, Feature<TGeometry> right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Feature<TGeometry> left, Feature<TGeometry> right)
    {
        return !(left?.Equals(right) ?? right is null);
    }

    #endregion
}
