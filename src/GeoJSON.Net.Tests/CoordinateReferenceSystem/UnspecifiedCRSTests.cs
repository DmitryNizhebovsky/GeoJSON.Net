using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.CoordinateReferenceSystem;

public class UnspecifiedCRSTests : TestBase
{
    [Fact]
    public void Has_Correct_Type()
    {
        var crs = new UnspecifiedCRS();

        Assert.Equal(CRSType.Unspecified, crs.Type);
    }

    [Fact]
    public void Can_Serialize_To_Null()
    {
        var collection = new FeatureCollection { CRS = new UnspecifiedCRS() };
        var expectedJson = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
        var actualJson = JsonConvert.SerializeObject(collection);
        
        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Deserialize_From_Null()
    {
        var json = "{\"type\":\"FeatureCollection\",\"crs\":null,\"features\":[] }";
        var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(json);

        Assert.IsType<UnspecifiedCRS>(featureCollection.CRS);
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        var left = new UnspecifiedCRS();
        var right = new UnspecifiedCRS();

        Assert.Equal(left, right);

        Assert.True(left == right);
        Assert.True(right == left);

        Assert.True(left.Equals(right));
        Assert.True(right.Equals(left));

        Assert.True(left.Equals(left));
        Assert.True(right.Equals(right));

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
