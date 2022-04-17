using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Feature;

public class FeatureCollectionTests : TestBase
{
    [Fact]
    public void Ctor_Throws_ArgumentNullException_When_Features_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var featureCollection = new FeatureCollection(null);
        });
    }

    [Fact]
    public void Can_Deserialize()
    {
        string json = GetExpectedJson();

        var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(json);

        Assert.NotNull(featureCollection.Features);
        Assert.Equal(5, featureCollection.Features.Count);
        Assert.Equal(2, featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Point));
        Assert.Equal(1, featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.MultiPolygon));
        Assert.Equal(1, featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Polygon));
        Assert.Equal(1, featureCollection.Features.Count(x => x.Geometry.Type == GeoJSONObjectType.Circle));
    }

    [Fact]
    public void FeatureCollectionSerialization()
    {
        var model = new FeatureCollection();
        for (var i = 0; i < 10; i++)
        {
            var id = "id" + i;

            var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

            var props = new Dictionary<string, object>
            {
                { "test1", "1" },
                { "test2", 2 }
            };

            var options = new Dictionary<string, object>
            {
                { "option1", "1" },
                { "option2", 2 }
            };

            var feature = new Net.Feature.Feature(geom, props, options, id);
            model.Features.Add(feature);
        }

        var actualJson = JsonConvert.SerializeObject(model);

        Assert.NotNull(actualJson);

        Assert.False(string.IsNullOrEmpty(actualJson));
    }
    
    [Fact]
    public void FeatureCollection_Equals_GetHashCode_Contract()
    {
        var left = GetFeatureCollection();
        var right = GetFeatureCollection();

        Assert_Are_Equal(left, right);
    }

    [Fact]
    public void Serialized_And_Deserialized_FeatureCollection_Equals_And_Share_HashCode()
    {
        var leftFc = GetFeatureCollection();
        var leftJson = JsonConvert.SerializeObject(leftFc);
        var left = JsonConvert.DeserializeObject<FeatureCollection>(leftJson);

        var rightFc = GetFeatureCollection();
        var rightJson = JsonConvert.SerializeObject(rightFc);
        var right = JsonConvert.DeserializeObject<FeatureCollection>(rightJson);

        Assert_Are_Equal(left, right);
    }

    /*
    [Fact]
    public void FeatureCollection_Test_IndexOf()
    {
        var model = new FeatureCollection();
        var expectedIds = new List<string>();
        var expectedIndexes = new List<int>();

        for (var i = 0; i < 10; i++)
        {
            var id = "id" + i;

            expectedIds.Add(id);
            expectedIndexes.Add(i);

            var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

            var props = FeatureTests.GetPropertiesInRandomOrder();
            var options = FeatureTests.GetPropertiesInRandomOrder();

            var feature = new Net.Feature.Feature(geom, props, options, id);
            model.Features.Add(feature);
        }

        for (var i = 0; i < 10; i++)
        {
            var actualFeature = model.Features[i];
            var actualId = actualFeature.Id;
            var actualIndex = model.Features.IndexOf(actualFeature);

            var expectedId = expectedIds[i];
            var expectedIndex = expectedIndexes[i];

            Assert.Equal(expectedId, actualId);
            Assert.Equal(expectedIndex, actualIndex);
        }
    }
    */

    private static FeatureCollection GetFeatureCollection()
    {
        var model = new FeatureCollection();
        for (var i = 0; i < 5; i++)
        {
            model.Features.Add(CreateFeature());
        }

        for (var i = 0; i < 5; i++)
        {
            model.Features.Add(CreateCluster());
        }
        return model;
    }

    private static Net.Feature.Feature CreateFeature()
    {
        var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

        var props = FeatureTests.GetPropertiesInRandomOrder();
        var options = FeatureTests.GetPropertiesInRandomOrder();
        var id = Guid.NewGuid().ToString();

        return new Net.Feature.Feature(geom, props, options, id);
    }

    private static Cluster CreateCluster()
    {
        var geom = new LineString(new[]
            {
                new Position(51.010, -1.034),
                new Position(51.010, -0.034)
            });

        var props = FeatureTests.GetPropertiesInRandomOrder();
        var options = FeatureTests.GetPropertiesInRandomOrder();
        var number = 42;
        var bbox = new BoundingBox(BoundingBoxType.FromBottomLeftTopRight, new Position(1, 2), new Position(3, 4));
        var id = Guid.NewGuid().ToString();

        return new Cluster(geom, props, options, number, bbox, id);
    }

    private static void Assert_Are_Equal(FeatureCollection left, FeatureCollection right)
    {
        Assert.Equal(left, right);

        Assert.True(left.Equals(right));
        Assert.True(right.Equals(left));

        Assert.True(left.Equals(left));
        Assert.True(right.Equals(right));

        Assert.True(left == right);
        Assert.True(right == left);

        Assert.False(left != right);
        Assert.False(right != left);

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
