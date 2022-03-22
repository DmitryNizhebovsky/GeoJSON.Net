using System;
using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.CoordinateReferenceSystem;

public class NamedCRSTests : TestBase
{
    [Fact]
    public void Has_Correct_Type()
    {
        var name = "EPSG:31370";
        var crs = new NamedCRS(name);

        Assert.Equal(CRSType.Name, crs.Type);
    }

    [Fact]
    public void Has_Name_Property_With_Name()
    {
        var name = "EPSG:31370";
        var crs = new NamedCRS(name);

        Assert.True(crs.Properties.ContainsKey("name"));
        Assert.Equal(name, crs.Properties["name"]);
    }

    [Fact]
    public void Can_Serialize()
    {
        var collection = new FeatureCollection() { CRS = new NamedCRS("EPSG:31370") };
        var actualJson = JsonConvert.SerializeObject(collection);

        JsonAssert.Contains("{\"properties\":{\"name\":\"EPSG:31370\"},\"type\":\"name\"}", actualJson);
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExpection_When_Name_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => { var collection = new FeatureCollection() { CRS = new NamedCRS(null) }; });
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExpection_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => { var collection = new FeatureCollection() { CRS = new NamedCRS(string.Empty) }; });
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        var name = "EPSG:31370";

        var left = new NamedCRS(name);
        var right = new NamedCRS(name);

        Assert.Equal(left, right);

        Assert.True(left == right);
        Assert.True(right == left);

        Assert.True(left.Equals(right));
        Assert.True(right.Equals(left));

        Assert.True(left.Equals(left));
        Assert.True(right.Equals(right));

        Assert.Equal(left.GetHashCode(), right.GetHashCode());

        name = "EPSG:25832";
        right = new NamedCRS(name);

        Assert.NotEqual(left, right);

        Assert.False(left == right);
        Assert.False(right == left);

        Assert.False(left.Equals(right));
        Assert.False(right.Equals(left));

        Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
    }
}
