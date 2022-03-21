using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Feature;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TGeometry"></typeparam>
/// <typeparam name="TProps"></typeparam>
/// <typeparam name="TOptions"></typeparam>
public class Cluster<TGeometry, TProps, TOptions> : GeoJSONObject, IEquatable<Cluster<TGeometry, TProps, TOptions>>
    where TGeometry : IGeometryObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Cluster{TGeometry, TProps, TOptions}" /> class.
    /// </summary>
    /// <param name="geometry"></param>
    /// <param name="properties"></param>
    /// <param name="options"></param>
    /// <param name="number"></param>
    /// <param name="boundingBox"></param>
    /// <param name="id"></param>
    [JsonConstructor]
    public Cluster(TGeometry geometry, TProps properties, TOptions options, long number, BoundingBox boundingBox, string id = null)
    {
        Geometry = geometry;
        Properties = properties;
        Options = options;
        Id = id;
        Number = number;
        BoundingBox = boundingBox;
    }

    public override GeoJSONObjectType Type => GeoJSONObjectType.Cluster;

    [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; }

    [JsonProperty(PropertyName = "geometry", Required = Required.AllowNull)]
    [JsonConverter(typeof(GeometryConverter))]
    public TGeometry Geometry { get; }

    [JsonProperty(PropertyName = "properties", Required = Required.AllowNull)]
    public TProps Properties { get; }

    [JsonProperty(PropertyName = "options", Required = Required.AllowNull)]
    public TOptions Options { get; }

    [JsonProperty(PropertyName = "number", Required = Required.AllowNull)]
    public long Number { get; }

    [JsonProperty(PropertyName = "bbox", Required = Required.AllowNull)]
    public BoundingBox BoundingBox { get; }

    #region IEquatable

    /// <summary>
    /// Equality comparer.
    /// </summary>
    /// <remarks>
    /// In contrast to <see cref="Cluster.Equals(Cluster)"/>, this implementation returns true only
    /// if <see cref="Id"/>, <see cref="Geometry"/>, <see cref="Properties"/>, <see cref="Options"/>, 
    /// <see cref="Number"/> and <see cref="BoundingBox"/> are also equal.
    /// The rationale here is that a user explicitly specifying the property type most probably cares about the properties equality.
    /// </remarks>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Cluster<TGeometry, TProps, TOptions> other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return EqualityComparer<TGeometry>.Default.Equals(Geometry, other.Geometry)
            && EqualityComparer<BoundingBox>.Default.Equals(BoundingBox, other.BoundingBox)
            && EqualityComparer<TProps>.Default.Equals(Properties, other.Properties)
            && EqualityComparer<TOptions>.Default.Equals(Options, other.Options)
            && EqualityComparer<long>.Default.Equals(Number, other.Number);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return Equals(obj as Cluster<TGeometry, TProps, TOptions>);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Id, Geometry, Properties, Options, BoundingBox, Number);
    }

    public static bool operator ==(Cluster<TGeometry, TProps, TOptions> left, Cluster<TGeometry, TProps, TOptions> right)
    {
        return object.Equals(left, right);
    }

    public static bool operator !=(Cluster<TGeometry, TProps, TOptions> left, Cluster<TGeometry, TProps, TOptions> right)
    {
        return !object.Equals(left, right);
    }

    #endregion
}

/// <summary>
/// A GeoJSON Cluster Object.
/// </summary>
public class Cluster : Cluster<IGeometryObject>
{
    [JsonConstructor]
    public Cluster(
        IGeometryObject geometry,
        long number,
        BoundingBox boundingBox,
        IDictionary<string, object> properties = null,
        IDictionary<string, object> options = null,
        string id = null)
        : base(geometry, properties, options, number, boundingBox, id)
    {
    }

    public Cluster(IGeometryObject geometry, object properties, object options, long number, BoundingBox boundingBox, string id = null)
        : base(geometry, properties, options, number, boundingBox, id)
    {
    }
}

/// <summary>
/// Typed GeoJSON Cluster class
/// </summary>
/// <remarks>Returns correctly typed Geometry property</remarks>
/// <typeparam name="TGeometry"></typeparam>
public class Cluster<TGeometry> : Cluster<TGeometry, IDictionary<string, object>, IDictionary<string, object>>, IEquatable<Cluster<TGeometry>>
    where TGeometry : IGeometryObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Feature" /> class.
    /// </summary>
    /// <param name="geometry">The Geometry Object.</param>
    /// <param name="properties">The properties.</param>
    /// <param name="id">The (optional) identifier.</param>
    [JsonConstructor]
    public Cluster(
        TGeometry geometry,
        long number,
        BoundingBox boundingBox,
        IDictionary<string, object> properties = null,
        IDictionary<string, object> options = null,
        string id = null)
        : base(
            geometry,
            properties ?? new Dictionary<string, object>(),
            options ?? new Dictionary<string, object>(),
            number, boundingBox, id)
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
    public Cluster(TGeometry geometry, object properties, object options, long number, BoundingBox boundingBox, string id = null)
        : this(geometry, number, boundingBox, GetDictionaryOfPublicProperties(properties), GetDictionaryOfPublicProperties(options), id)
    {
    }

    private static Dictionary<string, object> GetDictionaryOfPublicProperties(object properties)
    {
        if (properties == null)
        {
            return new();
        }

        return properties
            .GetType()
            .GetTypeInfo()
            .DeclaredProperties
            .Where(propertyInfo => propertyInfo.GetMethod.IsPublic)
            .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(properties, null));
    }

    #region IEquatable

    public bool Equals(Cluster<TGeometry> other)
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
        return obj.GetType() == GetType() && Equals((Cluster<TGeometry>)obj);
    }

    public override int GetHashCode()
    {
        return Geometry.GetHashCode();
    }

    public static bool operator ==(Cluster<TGeometry> left, Cluster<TGeometry> right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(Cluster<TGeometry> left, Cluster<TGeometry> right)
    {
        return !(left?.Equals(right) ?? right is null);
    }

    #endregion
}
