// Copyright © Joerg Battermann 2014, Matt Hunt 2017

namespace GeoJSON.Net;

/// <summary>
/// Base Interface for GeoJSONObject types.
/// </summary>
public interface IGeoJSONObject
{
    /// <summary>
    /// Gets the (mandatory) type of the GeoJSON Object.
    /// </summary>
    /// <remarks>
    /// See <see href="https://tools.ietf.org/html/rfc7946#section-3"/>
    /// </remarks>
    /// <value>
    /// The type of the object.
    /// </value>
    GeoJSONObjectType Type { get; }

    /// <summary>
    /// Gets the (optional) Coordinate Reference System Object.
    /// </summary>
    /// <remarks>
    /// See <see href="https://tools.ietf.org/html/rfc7946#section-4"/>
    /// </remarks>
    /// <value>
    /// The Coordinate Reference System Objects.
    /// </value>
    CoordinateReferenceSystem.ICRSObject CRS { get; }
}
