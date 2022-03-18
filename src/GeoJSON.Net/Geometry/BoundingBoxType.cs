namespace GeoJSON.Net.Geometry;

/// <summary>
/// Types to describe a bounding rectangle.
/// </summary>
public enum BoundingBoxType
{
	/// <summary>
	/// The bounding rectangle described by the positions of the top left and bottom right corners.
	/// </summary>
	FromTopLeftBottomRight = 0,

	/// <summary>
	/// The bounding rectangle described by the positions of the bottom left and top right corners.
	/// </summary>
	FromBottomLeftTopRight = 1
}
