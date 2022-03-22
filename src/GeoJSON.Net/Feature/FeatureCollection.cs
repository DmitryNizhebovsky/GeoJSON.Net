// Copyright © Joerg Battermann 2014, Matt Hunt 2017

using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoJSON.Net.Feature;

/// <summary>
/// Defines the FeatureCollection type.
/// </summary>
public class FeatureCollection<TGeometry> : GeoJSONObject, IEqualityComparer<FeatureCollection<TGeometry>>, IEquatable<FeatureCollection<TGeometry>>
    where TGeometry : IGeometryObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureCollection" /> class.
    /// </summary>
    public FeatureCollection() : this(new List<IFeatureCollectionItem<TGeometry>>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureCollection" /> class.
    /// </summary>
    /// <param name="features">The features.</param>
    public FeatureCollection(List<IFeatureCollectionItem<TGeometry>> features)
    {
        Features = features ?? throw new ArgumentNullException(nameof(features));
    }

    public override GeoJSONObjectType Type => GeoJSONObjectType.FeatureCollection;

    /// <summary>
    /// Gets the features.
    /// </summary>
    /// <value>The features.</value>
    [JsonProperty(PropertyName = "features", Required = Required.Always)]
    [JsonConverter(typeof(FeatureCollectionItemConverter))]
    public List<IFeatureCollectionItem<TGeometry>> Features { get; private set; }

    #region IEqualityComparer, IEquatable

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public override bool Equals(object obj)
    {
        return Equals(this, obj as FeatureCollection<TGeometry>);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object
    /// </summary>
    public bool Equals(FeatureCollection<TGeometry> other)
    {
        return Equals(this, other);
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public bool Equals(FeatureCollection<TGeometry> left, FeatureCollection<TGeometry> right)
    {
        if (base.Equals(left, right))
        {
            return left.Features.SequenceEqual(right.Features);
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal
    /// </summary>
    public static bool operator ==(FeatureCollection<TGeometry> left, FeatureCollection<TGeometry> right)
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
    public static bool operator !=(FeatureCollection<TGeometry> left, FeatureCollection<TGeometry> right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the hash code for this instance
    /// </summary>
    public override int GetHashCode()
    {
        int hash = base.GetHashCode();
        foreach (var feature in Features)
        {
            hash = (hash * 397) ^ feature.GetHashCode();
        }
        return hash;
    }

    /// <summary>
    /// Returns the hash code for the specified object
    /// </summary>
    public int GetHashCode(FeatureCollection<TGeometry> other)
    {
        return other.GetHashCode();
    }

    #endregion
}

public class FeatureCollection : FeatureCollection<IGeometryObject>
{
    [JsonConstructor]
    public FeatureCollection(List<IFeatureCollectionItem<IGeometryObject>> features)
        : base(features)
    {
    }

    public FeatureCollection()
        : base(new List<IFeatureCollectionItem<IGeometryObject>>())
    {
    }
}
