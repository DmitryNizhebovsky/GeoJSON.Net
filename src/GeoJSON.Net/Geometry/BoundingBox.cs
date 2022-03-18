using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace GeoJSON.Net.Geometry;

/// <summary>
/// A bounding rectangle described by the positions of two opposite corners top left and bottom right or bottom left and top right.
/// </summary>
public class BoundingBox : IEqualityComparer<BoundingBox>, IEquatable<BoundingBox>
{
    /// <summary>
    /// Bottom right or top right corner.
    /// </summary>
    public Position RightCorner { get; }

    /// <summary>
    /// Bottom left or top left corner.
    /// </summary>
    public Position LeftCorner { get; }

    /// <summary>
    /// Specifies how the bounding rectangle is defined.
    /// </summary>
    public BoundingBoxType BoundingBoxType { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rightCorner">Bottom right or top right corner.</param>
    /// <param name="leftCorner">Bottom left or top left corner.</param>
    public BoundingBox(BoundingBoxType boundingBoxType, Position rightCorner, Position leftCorner)
    {
        BoundingBoxType = boundingBoxType;
        RightCorner = rightCorner;
        LeftCorner = leftCorner;
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

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "BBOX ({0}, {1}, {2}, {3})",
            LeftCorner.Latitude, LeftCorner.Longitude, RightCorner.Latitude, RightCorner.Longitude);
    }

    #region IEqualityComparer, IEquatable

    /// <summary>
    /// Determines whether the specified object instances are considered equal.
    /// </summary>
    public static bool operator ==(BoundingBox left, BoundingBox right)
    {
        return left.LeftCorner == right.LeftCorner
            && left.RightCorner == right.RightCorner
            && left.BoundingBoxType == right.BoundingBoxType;
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
        var hash = 397 ^ LeftCorner.Latitude.GetHashCode();
        hash = (hash * 397) ^ LeftCorner.Longitude.GetHashCode();
        hash = (hash * 397) ^ LeftCorner.Altitude.GetValueOrDefault().GetHashCode();
        hash = (hash * 397) ^ RightCorner.Latitude.GetHashCode();
        hash = (hash * 397) ^ RightCorner.Longitude.GetHashCode();
        hash = (hash * 397) ^ RightCorner.Altitude.GetValueOrDefault().GetHashCode();
        return hash;
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
        var result = new BoundingBox(BoundingBoxType.FromBottomLeftTopRight, Position.Zero, Position.Zero);
        var firsts = Position.Zero;
        var seconds = Position.Zero;

        switch (coordinatesFormat)
        {
            case CoordinatesFormat.LatitudeLongitude:
                firsts = new Position(coordinates[0], coordinates[1]);
                seconds = new Position(coordinates[2], coordinates[3]);
                break;

            case CoordinatesFormat.LongitudeLatitude:
                firsts = new Position(coordinates[1], coordinates[0]);
                seconds = new Position(coordinates[3], coordinates[2]);
                break;
        }

        switch (bboxType)
        {
            case BoundingBoxType.FromTopLeftBottomRight:
                result = new BoundingBox(bboxType, firsts, seconds);
                break;

            case BoundingBoxType.FromBottomLeftTopRight:
                var topLeft = new Position(seconds.Latitude, firsts.Longitude);
                var bottomRight = new Position(firsts.Latitude, seconds.Longitude);

                result = new BoundingBox(bboxType, topLeft, bottomRight);
                break;
        }

        return result;
    }

    private static bool TryParseWktString(string wkt, out double[] result)
    {
        result = Array.Empty<double>();

        if (string.IsNullOrEmpty(wkt))
            return false;

        var strValues = wkt.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (strValues.Length != 4)
            return false;

        var values = strValues
            .Select(s => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double n) ? n : (double?)null)
            .Where(n => n.HasValue)
            .Select(n => n.Value)
            .ToArray();

        if (values.Length != 4)
            return false;

        result = values;
        return true;
    }

    private static double[] ParseWktString(string wkt)
    {
        if (string.IsNullOrEmpty(wkt))
            throw new ArgumentNullException("wkt string empty or null");

        var strValues = wkt.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (strValues.Length != 4)
            throw new FormatException("Invalid format of wkt string. Example: 37.283478,55.660739,37.936821,55.847952");

        var values = strValues
            .Select(s => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double n) ? n : (double?)null)
            .Where(n => n.HasValue)
            .Select(n => n.Value)
            .ToArray();

        if (values.Length != 4)
            throw new FormatException("Invalid format of wkt string. Example: 37.283478,55.660739,37.936821,55.847952");

        return values;
    }

    #endregion
}
