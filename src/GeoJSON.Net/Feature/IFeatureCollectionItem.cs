using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Feature;

public interface IFeatureCollectionItem<TGeometry> where TGeometry : IGeometryObject
{
    public string Id { get; }

    public TGeometry Geometry { get; }
}
