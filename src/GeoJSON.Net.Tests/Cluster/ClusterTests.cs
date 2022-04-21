using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Cluster;

public class ClusterTests : TestBase
{
    [Fact]
    public void Can_Deserialize_Circle_Cluster()
    {
        var json = GetExpectedJson();

        var cluster = JsonConvert.DeserializeObject<Net.Feature.Cluster>(json);

        Assert.NotNull(cluster);
        Assert.NotNull(cluster.Properties);
        Assert.True(cluster.Properties.Any());

        Assert.True(cluster.Properties.ContainsKey("Name"));
        Assert.Equal("Foo", cluster.Properties["Name"]);

        Assert.NotNull(cluster.Options);
        Assert.True(cluster.Options.Any());

        Assert.True(cluster.Options.ContainsKey("Foo"));
        Assert.Equal("Bar", cluster.Options["Foo"]);

        Assert.Equal("test-id", cluster.Id);

        Assert.Equal(42, cluster.Number);

        var expectedBbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            new Position(55.708352, 37.344074),
            new Position(55.801956, 37.670746));

        Assert.Equal(expectedBbox, cluster.BoundingBox);

        Assert.Equal(GeoJSONObjectType.Circle, cluster.Geometry.Type);

        var circleGeom = cluster.Geometry as Net.Geometry.Circle;

        Assert.Equal(new Position(55.771145, 37.623422), circleGeom.Coordinates);
        Assert.Equal(48, circleGeom.Radius);
    }

    [Fact]
    public void Can_Serialize_Circle_Cluster()
    {
        var geometry = new Circle(new Position(55.771145, 37.623422), 48);
        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var number = 42;
        var bbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            new Position(55.708352, 37.344074),
            new Position(55.801956, 37.670746));

        var id = "test-id";

        var cluster = new Net.Feature.Cluster(geometry, properties, options, number, bbox, id);
        var actualJson = JsonConvert.SerializeObject(cluster);

        var expectedJson = GetExpectedJson();

        JsonAssert.AreEqual(expectedJson, actualJson);
    }
}
