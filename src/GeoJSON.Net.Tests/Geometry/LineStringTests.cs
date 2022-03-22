using System;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Geometry;

public class LineStringTests : TestBase
{
    [Fact]
    public void Is_Closed()
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314, 4.889259338378906),
            new Position(52.3711451105601, 4.895267486572266),
            new Position(52.36931095278263, 4.892091751098633),
            new Position(52.370725881211314, 4.889259338378906)
        };

        var lineString = new LineString(coordinates);

        Assert.True(lineString.IsClosed());
    }

    [Fact]
    public void Is_Not_Closed()
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314, 4.889259338378906),
            new Position(52.3711451105601, 4.895267486572266),
            new Position(52.36931095278263, 4.892091751098633),
            new Position(52.370725881211592, 4.889259338378955)
        };

        var lineString = new LineString(coordinates);

        Assert.False(lineString.IsClosed());
    }
    

    [Fact]
    public void Can_Serialize()
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314, 4.889259338378906),
            new Position(52.3711451105601, 4.895267486572266),
            new Position(52.36931095278263, 4.892091751098633),
            new Position(52.370725881211314, 4.889259338378906)
        };

        var lineString = new LineString(coordinates);

        var actualJson = JsonConvert.SerializeObject(lineString);

        JsonAssert.AreEqual(GetExpectedJson(), actualJson);
    }

    [Fact]
    public void Can_Deserialize()
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314, 4.889259338378906),
            new Position(52.3711451105601, 4.895267486572266),
            new Position(52.36931095278263, 4.892091751098633),
            new Position(52.370725881211314, 4.889259338378906)
        };

        var expectedLineString = new LineString(coordinates);

        var json = GetExpectedJson();
        var actualLineString = JsonConvert.DeserializeObject<LineString>(json);

        Assert.Equal(expectedLineString, actualLineString);

        Assert.Equal(4, actualLineString.Coordinates.Count);
        Assert.Equal(expectedLineString.Coordinates[0].Latitude, actualLineString.Coordinates[0].Latitude);
        Assert.Equal(expectedLineString.Coordinates[0].Longitude, actualLineString.Coordinates[0].Longitude);
    }

    [Fact]
    public void Constructor_No_Coordinates_Throws_Exception()
    {
        var coordinates = new List<IPosition>();
        Assert.Throws<ArgumentOutOfRangeException>(() => new LineString(coordinates));
    }

    [Fact]
    public void Constructor_Null_Coordinates_Throws_Exception()
    {
        Assert.Throws<ArgumentNullException>(() => new LineString((IEnumerable<IPosition>)null));
    }

    private static LineString GetLineString(double offset = 0.0)
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314 + offset, 4.889259338378906 + offset),
            new Position(52.3711451105601 + offset, 4.895267486572266 + offset),
            new Position(52.36931095278263 + offset, 4.892091751098633 + offset),
            new Position(52.370725881211314 + offset, 4.889259338378906 + offset)
        };
        var lineString = new LineString(coordinates);
        return lineString;
    }

    [Fact]
    public void Equals_GetHashCode_Contract()
    {
        var rnd = new Random();
        var offset = rnd.NextDouble() * 60;
        if (rnd.NextDouble() < 0.5)
        {
            offset *= -1;
        }

        var left = GetLineString(offset);
        var right = GetLineString(offset);

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
