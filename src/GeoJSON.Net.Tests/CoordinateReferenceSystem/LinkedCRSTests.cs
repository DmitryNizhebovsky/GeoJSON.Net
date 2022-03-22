using System;
using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.CoordinateReferenceSystem;

public class LinkedCRSTests : TestBase
{
    private const string Href = "http://localhost";

    [Fact]
    public void Has_Correct_Type()
    {
        var crs = new LinkedCRS(Href);
        Assert.Equal(CRSType.Link, crs.Type);
    }

    [Fact]
    public void Has_Href_Property_With_Href()
    {
        var crs = new LinkedCRS(Href);

        Assert.True(crs.Properties.ContainsKey("href"));
        Assert.Equal(Href, crs.Properties["href"]);
    }

    [Fact]
    public void Has_Type_Property()
    {
        const string type = "ogcwkt";
        var crs = new LinkedCRS(Href, type);

        Assert.True(crs.Properties.ContainsKey("type"));
        Assert.Equal(type, crs.Properties["type"]);
    }

    [Fact]
    public void Can_Serialize()
    {
        var collection = new Point(new Position(1, 2)) { CRS = new LinkedCRS(Href) };
        var actualJson = JsonConvert.SerializeObject(collection);

        JsonAssert.Contains("{\"properties\":{\"href\":\"http://localhost\"},\"type\":\"link\"}", actualJson);
    }

    [Fact]
    public void Can_Deserialize_CRS_issue_101()
    {
        const string pointJson = "{\"type\":\"Point\",\"coordinates\":[2.0,1.0],\"crs\":{\"properties\":{\"href\":\"http://localhost\"},\"type\":\"link\"}}";
        var pointWithCRS = JsonConvert.DeserializeObject<Point>(pointJson);
        var linkCRS = pointWithCRS.CRS as LinkedCRS;

        Assert.NotNull(linkCRS);
        Assert.Equal(CRSType.Link, linkCRS.Type);
        Assert.Equal(Href, linkCRS.Properties["href"]);
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExpection_When_Href_String_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => { var crs = new LinkedCRS((string)null); });
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExpection_When_Href_Uri_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => { var crs = new LinkedCRS((Uri)null); });
    }

    [Fact]
    public void Ctor_Throws_ArgumentExpection_When_Href_Is_Not_Dereferencable_Uri()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

        var argumentExpection = Assert.Throws<ArgumentException>(() => { var crs = new LinkedCRS("http://not-a-valid-<>-url"); });
        Assert.Equal("must be a dereferenceable URI (Parameter 'href')", argumentExpection.Message);
    }

    [Fact]
    public void Ctor_Does_Not_Throw_When_Href_Is_Dereferencable_Uri()
    {
        var exception = Record.Exception(() => { var crs = new LinkedCRS("data.crs"); });
        Assert.Null(exception);
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExpection_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => { var crs = new LinkedCRS(string.Empty); });
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        var left = new LinkedCRS(Href);
        var right = new LinkedCRS(Href);

        Assert.Equal(left, right);

        Assert.True(left == right);
        Assert.True(right == left);

        Assert.True(left.Equals(right));
        Assert.True(right.Equals(left));

        Assert.True(left.Equals(left));
        Assert.True(right.Equals(right));

        Assert.Equal(left.GetHashCode(), right.GetHashCode());

        right = new LinkedCRS(Href + "?query=null");

        Assert.NotEqual(left, right);

        Assert.False(left == right);
        Assert.False(right == left);

        Assert.False(left.Equals(right));
        Assert.False(right.Equals(left));

        Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
    }
}
