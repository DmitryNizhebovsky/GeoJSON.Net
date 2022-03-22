using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Geometry;

public class PointTests : TestBase
{
    [Fact]
    public void Can_Serialize_With_Lat_Lon()
    {
        var point = new Point(new Position(53.2455662, 90.65464646));
        
        var expectedJson = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var actualJson = JsonConvert.SerializeObject(point);
        
        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_With_Lat_Lon_Alt()
    {
        var point = new Point(new Position(53.2455662, 90.65464646));

        var expectedJson = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var actualJson = JsonConvert.SerializeObject(point);

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Deserialize_With_Lat_Lon_Alt()
    {
        var json = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var expectedPoint = new Point(new Position(53.2455662, 90.65464646));

        var actualPoint = JsonConvert.DeserializeObject<Point>(json);

        Assert.NotNull(actualPoint);
        Assert.NotNull(actualPoint.Coordinates);
        Assert.Equal(53.2455662, actualPoint.Coordinates.Latitude);
        Assert.Equal(90.65464646, actualPoint.Coordinates.Longitude);
        Assert.Equal(expectedPoint, actualPoint);
    }

    [Fact]
    public void Can_Deserialize_With_Lat_Lon()
    {
        var json = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var expectedPoint = new Point(new Position(53.2455662, 90.65464646));

        var actualPoint = JsonConvert.DeserializeObject<Point>(json);

        Assert.NotNull(actualPoint);
        Assert.NotNull(actualPoint.Coordinates);
        Assert.Equal(53.2455662, actualPoint.Coordinates.Latitude);
        Assert.Equal(90.65464646, actualPoint.Coordinates.Longitude);
        Assert.Equal(expectedPoint, actualPoint);
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        var json = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var expectedPoint = new Point(new Position(53.2455662, 90.65464646));

        var actualPoint = JsonConvert.DeserializeObject<Point>(json);

        Assert.Equal(expectedPoint, actualPoint);
        Assert.True(expectedPoint.Equals(actualPoint));
        Assert.True(actualPoint.Equals(expectedPoint));

        Assert.Equal(expectedPoint.GetHashCode(), actualPoint.GetHashCode());
    }

    [Fact]
    public void Can_Serialize_With_Lat_Lon_Alt_DefaultValueHandling_Ignore()
    {
        var point = new Point(new Position(53.2455662, 90.65464646));

        var expectedJson = "{\"coordinates\":[90.65464646,53.2455662],\"type\":\"Point\"}";

        var actualJson = JsonConvert.SerializeObject(point, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

        JsonAssert.AreEqual(expectedJson, actualJson);
    }
}
