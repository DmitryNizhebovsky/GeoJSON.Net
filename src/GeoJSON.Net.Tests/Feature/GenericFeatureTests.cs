using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Feature;

public class GenericFeatureTests : TestBase
{
    [Fact]
    public void Can_Deserialize_Point_Feature()
    {
        var json = GetExpectedJson();

        var feature = JsonConvert.DeserializeObject<Feature<Point>>(json);

        Assert.NotNull(feature);
        Assert.NotNull(feature.Properties);
        Assert.True(feature.Properties.Any());

        Assert.True(feature.Properties.ContainsKey("name"));
        Assert.Equal("Dinagat Islands", feature.Properties["name"]);

        Assert.NotNull(feature.Options);
        Assert.True(feature.Options.Any());

        Assert.True(feature.Options.ContainsKey("fill"));
        Assert.Equal(true, feature.Options["fill"]);

        Assert.Equal("test-id", feature.Id);

        Assert.Equal(GeoJSONObjectType.Point, feature.Geometry.Type);
        Assert.Equal(125.6, feature.Geometry.Coordinates.Longitude);
        Assert.Equal(10.1, feature.Geometry.Coordinates.Latitude);
    }

    [Fact]
    public void Can_Deserialize_LineString_Feature()
    {
        var json = GetExpectedJson();

        var feature = JsonConvert.DeserializeObject<Feature<LineString>>(json);

        Assert.NotNull(feature);
        Assert.NotNull(feature.Properties);
        Assert.True(feature.Properties.Any());

        Assert.True(feature.Properties.ContainsKey("name"));
        Assert.Equal("Dinagat Islands", feature.Properties["name"]);

        Assert.NotNull(feature.Options);
        Assert.True(feature.Options.Any());

        Assert.True(feature.Options.ContainsKey("fill"));
        Assert.Equal(true, feature.Options["fill"]);

        Assert.Equal("test-id", feature.Id);

        Assert.Equal(GeoJSONObjectType.LineString, feature.Geometry.Type);

        Assert.Equal(4, feature.Geometry.Coordinates.Count);
    }

    [Fact]
    public void Feature_Generic_Equals_Null_Issure94()
    {
        bool equal1 = true;
        bool equal2 = true;

        var point = new Point(new Position(34, 123));
        var properties = new Dictionary<string, string>
        {
            {"test1", "test1val"},
            {"test2", "test2val"}
        };
        var options = new Dictionary<string, string>
        {
            {"option1", "option1val"},
            {"option2", "option2val"}
        };

        var feature = new Feature<Point, Dictionary<string, string>, Dictionary<string, string>>(point, properties, options, "testid");

        var exception = Record.Exception(() =>
        {
            equal1 = feature == null;
            equal2 = feature.Equals(null);
        });

        Assert.Null(exception);

        Assert.False(equal1);
        Assert.False(equal2);
    }

    private class TypedFeatureProps
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public double Value { get; set; }
    }

    private class TypedFeatureOptions
    {
        [JsonProperty("fill")]
        public bool Fill { get; set; }
        [JsonProperty("fillColor")]
        public string FillColor { get; set; }
    }

    [Fact]
    public void Can_Deserialize_Typed_Point_Feature()
    {
        var json = GetExpectedJson();
        var feature = JsonConvert.DeserializeObject<Feature<Point, TypedFeatureProps, TypedFeatureOptions>>(json);

        Assert.NotNull(feature);

        Assert.NotNull(feature.Properties);
        Assert.Equal("Dinagat Islands", feature.Properties.Name);
        Assert.Equal(4.2, feature.Properties.Value);

        Assert.NotNull(feature.Options);
        Assert.True(feature.Options.Fill);
        Assert.Equal("#FF00FF", feature.Options.FillColor);

        Assert.Equal("test-id", feature.Id);

        Assert.Equal(GeoJSONObjectType.Point, feature.Geometry.Type);
    }

    [Fact]
    public void Can_Serialize_Typed_Point_Feature()
    {
        var geometry = new Point(new Position(1, 2));
        var props = new TypedFeatureProps
        {
            Name = "no name here",
            Value = 1.337
        };
        var options = new TypedFeatureOptions
        {
            Fill = true,
            FillColor = "#FF00FF"
        };

        var feature = new Feature<Point, TypedFeatureProps, TypedFeatureOptions>(geometry, props, options, "no id there");

        var expectedJson = GetExpectedJson();
        var actualJson = JsonConvert.SerializeObject(feature);

        JsonAssert.AreEqual(expectedJson, actualJson);
    }
}
