using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Geometry;

public class MultiPolygonTests : TestBase
{
    [Fact]
    public void Can_Deserialize()
    {
        var json = GetExpectedJson();

        var expectMultiPolygon = GetMultiPolygon();

        var actualMultiPolygon = JsonConvert.DeserializeObject<MultiPolygon>(json);

        Assert.Equal(expectMultiPolygon, actualMultiPolygon);
    }

    private static MultiPolygon GetMultiPolygon(double offset = 0.0)
    {
        var multiPolygon = new MultiPolygon(new List<Polygon>
        {
            new Polygon(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.959676831105995 + offset, -2.6797102391514338 + offset),
                    new Position(52.9608756693609 + offset, -2.6769029474483279 + offset),
                    new Position(52.908449372833715 + offset, -2.6079763270327119 + offset),
                    new Position(52.891287242948195 + offset, -2.5815104708998668 + offset),
                    new Position(52.875476700983896 + offset, -2.5851645010668989 + offset),
                    new Position(52.882954723868622 + offset, -2.6050779098387191 + offset),
                    new Position(52.875255907042678 + offset, -2.6373482332006359 + offset),
                    new Position(52.878791122091066 + offset, -2.6932445076063951 + offset),
                    new Position(52.89564268523565 + offset, -2.6931334629377890 + offset),
                    new Position(52.930592009390175 + offset, -2.6548779332193022 + offset),
                    new Position(52.959676831105995 + offset, -2.6797102391514338 + offset)
                })
            }),
            new Polygon(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.89610842810761 + offset,-2.69628632041613 + offset),
                    new Position(52.8894641454077 + offset,-2.75901233808515 + offset),
                    new Position(52.89938894657412 + offset,-2.7663172788742449 + offset),
                    new Position(52.90253773227807 + offset,-2.804554822840895 + offset),
                    new Position(52.929801009654575 + offset,-2.83848602260174 + offset),
                    new Position(52.94013913205788 + offset,-2.838979264607087 + offset),
                    new Position(52.937353122653533 + offset,-2.7978187468478741 + offset),
                    new Position(52.920394929466184 + offset,-2.772273870352612 + offset),
                    new Position(52.926572918779222 + offset,-2.6996509024137052 + offset),
                    new Position(52.89610842810761 + offset, -2.69628632041613 + offset)
                })
            })
        });
        return multiPolygon;
    }

    [Fact]
    public void Can_Serialize()
    {
        // Arrang
        var polygon1 = new Polygon(new List<LineString>
        {
            new LineString(new List<IPosition>
            {
                new Position(0, 0), 
                new Position(0, 1), 
                new Position(1, 1), 
                new Position(1, 0), 
                new Position(0, 0)
            })
        });

        var polygon2 = new Polygon(new List<LineString>
        {
            new LineString(new List<IPosition>
            {
                new Position(60, 60), 
                new Position(60, 61), 
                new Position(61, 61), 
                new Position(61, 60), 
                new Position(60, 60)
            }), 
            new LineString(new List<IPosition>
            {
                new Position(70, 70), 
                new Position(71, 70), 
                new Position(71, 71), 
                new Position(70, 71), 
                new Position(70, 70)
            })
        });

        var multiPolygon = new MultiPolygon(new List<Polygon> { polygon1, polygon2 });
        var expectedJson = GetExpectedJson();

        // Act
        var actualJson = JsonConvert.SerializeObject(multiPolygon);

        // Assert
        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        double offset = 0d;

        var left = GetMultiPolygon(offset);
        var right = GetMultiPolygon(offset);

        Assert.Equal(left, right);
        Assert.Equal(right, left);

        Assert.True(left.Equals(right));
        Assert.True(left.Equals(left));
        Assert.True(right.Equals(left));
        Assert.True(right.Equals(right));

        Assert.True(left == right);
        Assert.True(right == left);

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
