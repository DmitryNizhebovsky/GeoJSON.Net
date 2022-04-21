using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.BoundingBox;

public sealed class BoundingBoxTests : TestBase
{
    [Fact]
    public void Parse_BoundingBox_From_WKT_String_FromTopLeftBottomRight_LatitudeLongitude()
    {
        var wkt = "55.832915,37.346306,55.708352,37.915878";
        Net.Geometry.BoundingBox bbox = null;

        var exception = Record.Exception(() =>
        {
            bbox = Net.Geometry.BoundingBox.Parse(BoundingBoxType.FromTopLeftBottomRight, CoordinatesFormat.LatitudeLongitude, wkt);
        });

        Assert.Null(exception);

        Assert.Equal(BoundingBoxType.FromTopLeftBottomRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.832915, 37.346306), bbox.From);
        Assert.Equal(new Position(55.708352, 37.915878), bbox.To);
    }

    [Fact]
    public void Parse_BoundingBox_From_WKT_String_FromTopLeftBottomRight_LongitudeLatitude()
    {
        var wkt = "37.346306,55.832915,37.915878,55.708352";
        Net.Geometry.BoundingBox bbox = null;

        var exception = Record.Exception(() =>
        {
            bbox = Net.Geometry.BoundingBox.Parse(BoundingBoxType.FromTopLeftBottomRight, CoordinatesFormat.LongitudeLatitude, wkt);
        });

        Assert.Null(exception);

        Assert.Equal(BoundingBoxType.FromTopLeftBottomRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.832915, 37.346306), bbox.From);
        Assert.Equal(new Position(55.708352, 37.915878), bbox.To);
    }

    [Fact]
    public void Parse_BoundingBox_From_WKT_String_FromBottomLeftTopRight_LatitudeLongitude()
    {
        var wkt = "55.708352,37.346306,55.832915,37.915878";
        Net.Geometry.BoundingBox bbox = null;

        var exception = Record.Exception(() =>
        {
            bbox = Net.Geometry.BoundingBox.Parse(BoundingBoxType.FromBottomLeftTopRight, CoordinatesFormat.LatitudeLongitude, wkt);
        });

        Assert.Null(exception);

        Assert.Equal(BoundingBoxType.FromBottomLeftTopRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.708352, 37.346306), bbox.From);
        Assert.Equal(new Position(55.832915, 37.915878), bbox.To);
    }

    [Fact]
    public void Parse_BoundingBox_From_WKT_String_FromBottomLeftTopRight_LongitudeLatitude()
    {
        var wkt = "37.346306,55.708352,37.915878,55.832915";
        Net.Geometry.BoundingBox bbox = null;

        var exception = Record.Exception(() =>
        {
            bbox = Net.Geometry.BoundingBox.Parse(BoundingBoxType.FromBottomLeftTopRight, CoordinatesFormat.LongitudeLatitude, wkt);
        });

        Assert.Null(exception);

        Assert.Equal(BoundingBoxType.FromBottomLeftTopRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.708352, 37.346306), bbox.From);
        Assert.Equal(new Position(55.832915, 37.915878), bbox.To);
    }

    [Fact]
    public void TryParse_BoundingBox_From_WKT_String_FromTopLeftBottomRight_LatitudeLongitude()
    {
        var wkt = "55.832915,37.346306,55.708352,37.915878";

        var parseOk = Net.Geometry.BoundingBox.TryParse(
            BoundingBoxType.FromTopLeftBottomRight,
            CoordinatesFormat.LatitudeLongitude,
            wkt,
            out var bbox);

        Assert.True(parseOk);

        Assert.Equal(BoundingBoxType.FromTopLeftBottomRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.832915, 37.346306), bbox.From);
        Assert.Equal(new Position(55.708352, 37.915878), bbox.To);
    }

    [Fact]
    public void TryParse_BoundingBox_From_WKT_String_FromTopLeftBottomRight_LongitudeLatitude()
    {
        var wkt = "37.346306,55.832915,37.915878,55.708352";

        var parseOk = Net.Geometry.BoundingBox.TryParse(
            BoundingBoxType.FromTopLeftBottomRight,
            CoordinatesFormat.LongitudeLatitude,
            wkt,
            out var bbox);

        Assert.True(parseOk);

        Assert.Equal(BoundingBoxType.FromTopLeftBottomRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.832915, 37.346306), bbox.From);
        Assert.Equal(new Position(55.708352, 37.915878), bbox.To);
    }

    [Fact]
    public void TryParse_BoundingBox_From_WKT_String_FromBottomLeftTopRight_LatitudeLongitude()
    {
        var wkt = "55.708352,37.346306,55.832915,37.915878";

        var parseOk = Net.Geometry.BoundingBox.TryParse(
            BoundingBoxType.FromBottomLeftTopRight,
            CoordinatesFormat.LatitudeLongitude,
            wkt,
            out var bbox);

        Assert.True(parseOk);

        Assert.Equal(BoundingBoxType.FromBottomLeftTopRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.708352, 37.346306), bbox.From);
        Assert.Equal(new Position(55.832915, 37.915878), bbox.To);
    }

    [Fact]
    public void TryParse_BoundingBox_From_WKT_String_FromBottomLeftTopRight_LongitudeLatitude()
    {
        var wkt = "37.346306,55.708352,37.915878,55.832915";

        var parseOk = Net.Geometry.BoundingBox.TryParse(
            BoundingBoxType.FromBottomLeftTopRight,
            CoordinatesFormat.LongitudeLatitude,
            wkt,
            out var bbox);

        Assert.True(parseOk);

        Assert.Equal(BoundingBoxType.FromBottomLeftTopRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.708352, 37.346306), bbox.From);
        Assert.Equal(new Position(55.832915, 37.915878), bbox.To);
    }

    [Fact]
    public void Can_Deserialize()
    {
        var json = GetExpectedJson();

        var bbox = JsonConvert.DeserializeObject<Net.Geometry.BoundingBox>(json);

        Assert.NotNull(bbox);

        Assert.Equal(BoundingBoxType.FromBottomLeftTopRight, bbox.BoundingBoxType);
        Assert.Equal(new Position(55.708352, 37.346306), bbox.From);
        Assert.Equal(new Position(55.832915, 37.915878), bbox.To);
    }

    [Fact]
    public void Can_Serialize()
    {
        var bbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            new Position(55.708352, 37.346306),
            new Position(55.832915, 37.915878));

        var actualJson = JsonConvert.SerializeObject(bbox);

        var expectedJson = GetExpectedJson();

        JsonAssert.AreEqualArray(expectedJson, actualJson);
    }

    [Fact]
    public void Convert_the_type_of_BoundingBox_BottomLeftTopRight_to_TopLeftBottomRight()
    {
        var bbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            new Position(55.708352, 37.346306),
            new Position(55.832915, 37.915878));

        var expectedBbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromTopLeftBottomRight,
            new Position(55.832915, 37.346306),
            new Position(55.708352, 37.915878));

        bbox.ConvertBoundingBoxTypeTo(BoundingBoxType.FromTopLeftBottomRight);

        Assert.Equal(expectedBbox, bbox);
    }

    [Fact]
    public void Convert_the_type_of_BoundingBox_TopLeftBottomRight_to_BottomLeftTopRight()
    {
        var bbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromTopLeftBottomRight,
            new Position(55.832915, 37.346306),
            new Position(55.708352, 37.915878));

        var expectedBbox = new Net.Geometry.BoundingBox(
            BoundingBoxType.FromBottomLeftTopRight,
            new Position(55.708352, 37.346306),
            new Position(55.832915, 37.915878));

        bbox.ConvertBoundingBoxTypeTo(BoundingBoxType.FromBottomLeftTopRight);

        Assert.Equal(expectedBbox, bbox);
    }
}
