using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;

namespace GeoJSON.Net.Geometry;

/// <summary>
/// A bounding rectangle described by the positions of two opposite corners.
/// </summary>
[JsonConverter(typeof(BoundingBoxConverter))]
public class BoundingBox : IEqualityComparer<BoundingBox>, IEquatable<BoundingBox>
{
    /// <summary>
    /// The from.
    /// </summary>
    public Position From { get; private set; }

    /// <summary>
    /// The to.
    /// </summary>
    public Position To { get; private set; }

    /// <summary>
    /// Specifies how the bounding rectangle is defined.
    /// </summary>
    public BoundingBoxType BoundingBoxType { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BoundingBox" /> class.
    /// </summary>
    /// <param name="boundingBoxType">Specifies how the bounding rectangle is defined.</param>
    /// <param name="from">The from.</param>
    /// <param name="to">The to.</param>
    public BoundingBox(BoundingBoxType boundingBoxType, Position from, Position to)
    {
        BoundingBoxType = boundingBoxType;
        From = from;
        To = to;
    }

    /// <summary>
    /// Converts the string representation of a bounding box to its <see cref="BoundingBox"/> equivalent.
    /// </summary>
    /// <param name="bboxType">Specifies how the bounding rectangle is defined.</param>
    /// <param name="coordinatesFormat">Coordinate format.</param>
    /// <param name="wkt">Well-known text.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <returns>A <see cref="BoundingBox"/> equivalent to the bounding box contained in <paramref name="wkt"/>.</returns>
    public static BoundingBox Parse(BoundingBoxType bboxType, CoordinatesFormat coordinatesFormat, string wkt)
    {
        var coordinates = ParseWktString(wkt);
        return BuildBoundingBox(bboxType, coordinatesFormat, coordinates);
    }

    /// <summary>
    /// Converts the string representation of a bounding box to its <see cref="BoundingBox"/> equivalent.
    /// A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="bboxType">Specifies how the bounding rectangle is defined.</param>
    /// <param name="coordinatesFormat">Coordinate format.</param>
    /// <param name="wkt">Well-known text.</param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="BoundingBox"/> value equivalent
    /// of the bounding box contained in <paramref name="wkt"/>, if the conversion succeeded, or <see langword="null"/> if the conversion
    /// failed. The conversion fails if the <paramref name="wkt"/> parameter is null or <see cref="string.Empty"/> or invalid format.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="wkt"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(BoundingBoxType bboxType, CoordinatesFormat coordinatesFormat, string wkt, out BoundingBox result)
    {
        result = null;

        if (!TryParseWktString(wkt, out double[] coordinates))
            return false;

        result = BuildBoundingBox(bboxType, coordinatesFormat, coordinates);
        return true;
    }

    public void ConvertBoundingBoxTypeTo(BoundingBoxType newType)
    {
        if (BoundingBoxType == newType)
            return;

        BoundingBoxType = newType;

        var from = From;
        From = new Position(To.Latitude, from.Longitude);
        To = new Position(from.Latitude, To.Longitude);
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "BBOX ({0}, {1}, {2}, {3})",
            From.Latitude, From.Longitude, To.Latitude, To.Longitude);
    }

    #region IEqualityComparer, IEquatable

    /// <summary>
    /// Determines whether the specified object instances are considered equal.
    /// </summary>
    public static bool operator ==(BoundingBox left, BoundingBox right)
    {
        return left.From == right.From
            && left.To == right.To;
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal.
    /// </summary>
    public static bool operator !=(BoundingBox left, BoundingBox right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the specified object instances are considered equal.
    /// </summary>
    public bool Equals(BoundingBox left, BoundingBox right)
    {
        return left == right;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    public bool Equals(BoundingBox other)
    {
        return this == other;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    public override bool Equals(object obj)
    {
        return this == (obj as BoundingBox);
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(From, To, BoundingBoxType);
    }

    /// <summary>
    /// Returns the hash code for the specified object.
    /// </summary>
    public int GetHashCode([DisallowNull] BoundingBox other)
    {
        return other.GetHashCode();
    }

    #endregion

    #region Private methods

    private static BoundingBox BuildBoundingBox(BoundingBoxType bboxType, CoordinatesFormat coordinatesFormat, double[] coordinates)
    {
        Position first = Position.Zero;
        Position second = Position.Zero;

        switch (coordinatesFormat)
        {
            case CoordinatesFormat.LatitudeLongitude:
                first = new Position(coordinates[0], coordinates[1]);
                second = new Position(coordinates[2], coordinates[3]);
                break;

            case CoordinatesFormat.LongitudeLatitude:
                first = new Position(coordinates[1], coordinates[0]);
                second = new Position(coordinates[3], coordinates[2]);
                break;
        }

        return new BoundingBox(bboxType, first, second);
    }

    private static bool TryParseWktString(string wkt, out double[] result)
    {
        result = Array.Empty<double>();

        if (string.IsNullOrEmpty(wkt))
            return false;

        var strValues = wkt.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (strValues.Length != 4)
            return false;

        var values = new double[4];

        for (int i = 0; i < strValues.Length; ++i)
        {
            if (double.TryParse(strValues[i], NumberStyles.Float, CultureInfo.InvariantCulture, out double n))
                values[i] = n;
            else
                return false;
        }

        result = values;
        return true;
    }

    private static double[] ParseWktString(string wkt)
    {
        if (string.IsNullOrEmpty(wkt))
            throw new ArgumentNullException(nameof(wkt), "String empty or null");

        var strValues = wkt.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (strValues.Length != 4)
            throw new FormatException("Invalid format of wkt string. Example: 37.283478,55.660739,37.936821,55.847952");

        var values = new double[4];

        for (int i = 0; i < strValues.Length; ++i)
            values[i] = double.Parse(strValues[i], NumberStyles.Float, CultureInfo.InvariantCulture);

        return values;
    }

    #endregion
}
