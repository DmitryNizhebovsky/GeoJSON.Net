using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Feature;

public class FeatureTests : TestBase
{
    [Fact]
    public void Can_Deserialize_Point_Feature()
    {
        var json = GetExpectedJson();

        var feature = JsonConvert.DeserializeObject<Net.Feature.Feature>(json);

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
    }

    [Fact]
    public void Can_Serialize_LineString_Feature()
    {
        var coordinates = new[]
        {
            new List<IPosition>
            {
                new Position(52.370725881211314, 4.889259338378906),
                new Position(52.3711451105601, 4.895267486572266),
                new Position(52.36931095278263, 4.892091751098633),
                new Position(52.370725881211314, 4.889259338378906)
            },
            new List<IPosition>
            {
                new Position(52.370725881211314, 4.989259338378906),
                new Position(52.3711451105601, 4.995267486572266),
                new Position(52.36931095278263, 4.992091751098633),
                new Position(52.370725881211314, 4.989259338378906)
            }
        };

        var geometry = new LineString(coordinates[0]);
        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var id = "id-42";

        var actualJson = JsonConvert.SerializeObject(new Net.Feature.Feature(geometry, properties, options, id));

        var expectedJson = GetExpectedJson();

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_MultiLineString_Feature()
    {
        var geometry = new MultiLineString(new List<LineString>
        {
            new LineString(new List<IPosition>
            {
                new Position(52.370725881211314, 4.889259338378906),
                new Position(52.3711451105601, 4.895267486572266),
                new Position(52.36931095278263, 4.892091751098633),
                new Position(52.370725881211314, 4.889259338378906)
            }),
            new LineString(new List<IPosition>
            {
                new Position(52.370725881211314, 4.989259338378906),
                new Position(52.3711451105601, 4.995267486572266),
                new Position(52.36931095278263, 4.992091751098633),
                new Position(52.370725881211314, 4.989259338378906)
            })
        });

        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var id = "id-42";

        var expectedJson = GetExpectedJson();

        var actualJson = JsonConvert.SerializeObject(new Net.Feature.Feature(geometry, properties, options, id));

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_Point_Feature()
    {
        var geometry = new Point(new Position(1, 2));
        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var id = "id-42";

        var expectedJson = GetExpectedJson();

        var actualJson = JsonConvert.SerializeObject(new Net.Feature.Feature(geometry, properties, options, id));

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_Polygon_Feature()
    {
        var coordinates = new List<IPosition>
        {
            new Position(52.370725881211314, 4.889259338378906),
            new Position(52.3711451105601, 4.895267486572266),
            new Position(52.36931095278263, 4.892091751098633),
            new Position(52.370725881211314, 4.889259338378906)
        };

        var polygon = new Polygon(new List<LineString> { new LineString(coordinates) });
        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var id = "id-42";
        var feature = new Net.Feature.Feature(polygon, properties, options, id);

        var expectedJson = GetExpectedJson();
        var actualJson = JsonConvert.SerializeObject(feature);

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_MultiPolygon_Feature()
    {
        var multiPolygon = new MultiPolygon(new List<Polygon>
        {
            new Polygon(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(1, 1),
                    new Position(1, 0),
                    new Position(0, 0)
                })
            }),
            new Polygon(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(70, 70),
                    new Position(70, 71),
                    new Position(71, 71),
                    new Position(71, 70),
                    new Position(70, 70)
                }),
                new LineString(new List<IPosition>
                {
                    new Position(80, 80),
                    new Position(80, 81),
                    new Position(81, 81),
                    new Position(81, 80),
                    new Position(80, 80)
                })
            })
        });

        var properties = new Dictionary<string, object> { { "Name", "Foo" } };
        var options = new Dictionary<string, object> { { "Foo", "Bar" } };
        var id = "id-42";

        var feature = new Net.Feature.Feature(multiPolygon, properties, options, id);

        var expectedJson = GetExpectedJson();
        var actualJson = JsonConvert.SerializeObject(feature);

        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Can_Serialize_Dictionary_Subclass()
    {
        var geometry = new Point(new Position(10, 10));
        var properties =
            new TestFeaturePropertyDictionary()
            {
                 BooleanProperty = true,
                 DoubleProperty = 1.2345d,
                 EnumProperty = TestFeatureEnum.Value1,
                 IntProperty = -1,
                 StringProperty = "Hello, GeoJSON !"
            };

        var options =
            new TestFeaturePropertyDictionary()
            {
                BooleanProperty = false,
                DoubleProperty = 5.4321d,
                EnumProperty = TestFeatureEnum.Value2,
                IntProperty = 42,
                StringProperty = "Hello, YandexMap !"
            };

        var id = "id-42";

        var feature = new Net.Feature.Feature(geometry, properties, options, id);

        var expectedJson = GetExpectedJson();
        var actualJson = JsonConvert.SerializeObject(feature);

        Assert.False(string.IsNullOrEmpty(expectedJson));
        JsonAssert.AreEqual(expectedJson, actualJson);
    }

    [Fact]
    public void Ctor_Can_Add_Properties_Using_Object()
    {
        var geometry = new Point(new Position(10, 10));
        var properties = new TestFeatureProperty
        {
            BooleanProperty = true,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 1.2345d,
            EnumProperty = TestFeatureEnum.Value1,
            IntProperty = -1,
            StringProperty = "Hello, GeoJSON !"
        };

        var options = new TestFeatureProperty()
        {
            BooleanProperty = false,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 5.4321d,
            EnumProperty = TestFeatureEnum.Value2,
            IntProperty = 42,
            StringProperty = "Hello, YandexMap !"
        };

        var id = "id-42";

        var feature = new Net.Feature.Feature(geometry, properties, options, id);

        Assert.NotNull(feature.Properties);
        Assert.True(feature.Properties.Count > 1);
        Assert.Equal(6, feature.Properties.Count);

        Assert.NotNull(feature.Options);
        Assert.True(feature.Options.Count > 1);
        Assert.Equal(6, feature.Options.Count);
    }

    [Fact]
    public void Ctor_Can_Add_Properties_Using_Object_Inheriting_Dictionary()
    {
        var id = "id-42";
        var geometry = new Point(new Position(10, 10));
        var properties = new TestFeaturePropertyDictionary()
        {
            BooleanProperty = true,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 1.2345d,
            EnumProperty = TestFeatureEnum.Value1,
            IntProperty = -1,
            StringProperty = "Hello, GeoJSON !"
        };

        var options = new TestFeaturePropertyDictionary()
        {
            BooleanProperty = false,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 5.4321d,
            EnumProperty = TestFeatureEnum.Value2,
            IntProperty = 42,
            StringProperty = "Hello, YandexMap !"
        };

        var feature = new Net.Feature.Feature(geometry, properties, null, id);

        Assert.NotNull(feature.Properties);
        Assert.True(feature.Properties.Count > 1);
        Assert.Equal(6, feature.Properties.Count);
    }

    [Fact]
    public void Ctor_Creates_Properties_Collection_When_Passed_Null_Proper_Object()
    {
        var id = "id-42";
        var geometry = new Point(new Position(10, 10));
        var options = new TestFeatureProperty()
        {
            BooleanProperty = false,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 5.4321d,
            EnumProperty = TestFeatureEnum.Value2,
            IntProperty = 42,
            StringProperty = "Hello, YandexMap !"
        };

        var feature = new Net.Feature.Feature(geometry, null, options, id);

        Assert.NotNull(feature.Properties);
        Assert.Empty(feature.Properties);
    }

    [Fact]
    public void Ctor_Creates_Properties_Collection_When_Passed_Null_Options_Object()
    {
        var geometry = new Point(new Position(10, 10));
        var properties = new TestFeaturePropertyDictionary()
        {
            BooleanProperty = true,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 1.2345d,
            EnumProperty = TestFeatureEnum.Value1,
            IntProperty = -1,
            StringProperty = "Hello, GeoJSON !"
        };
        var id = "id-42";

        var feature = new Net.Feature.Feature(geometry, properties, null, id);

        Assert.NotNull(feature.Options);
        Assert.Empty(feature.Options);
    }

    [Fact]
    public void Feature_Equals_GetHashCode_Contract_Properties_Of_Objects()
    {
        var id = "11-22";
        // order of keys should not matter

        var leftProp = new TestFeatureProperty
        {
            StringProperty = "Hello, GeoJSON !",
            EnumProperty = TestFeatureEnum.Value1,
            IntProperty = -1,
            BooleanProperty = true,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 1.2345d
        };

        var leftOptions = new TestFeatureProperty()
        {
            BooleanProperty = false,
            DoubleProperty = 5.4321d,
            EnumProperty = TestFeatureEnum.Value2,
            IntProperty = 42,
            StringProperty = "Hello, YandexMap !",
            DateTimeProperty = DateTime.Now
        };

        var left = new Net.Feature.Feature(new Point(new Position(10, 10)), leftProp, leftOptions, id);

        var rightProp = new TestFeatureProperty
        {
            BooleanProperty = true,
            DateTimeProperty = DateTime.Now,
            DoubleProperty = 1.2345d,
            EnumProperty = TestFeatureEnum.Value1,
            IntProperty = -1,
            StringProperty = "Hello, GeoJSON !"
        };

        var rightOptions = new TestFeatureProperty()
        {
            DateTimeProperty = DateTime.Now,
            StringProperty = "Hello, YandexMap !",
            IntProperty = 42,
            EnumProperty = TestFeatureEnum.Value2,
            DoubleProperty = 5.4321d,
            BooleanProperty = false
        };

        var right = new Net.Feature.Feature(new Point(new Position(10, 10)), rightProp, rightOptions, id);

        Assert_Are_Equal(left, right);
    }

    [Fact]
    public void Feature_Equals_GetHashCode_Contract_Dictionary()
    {
        var leftDictionary = GetPropertiesInRandomOrder();
        var rightDictionary = GetPropertiesInRandomOrder();

        var geometry10 = new Position(10, 10);
        var geometry20 = new Position(20, 20);

        var left = new Net.Feature.Feature(new Point(
            geometry10),
            leftDictionary,
            rightDictionary,
            "abc");
        var right = new Net.Feature.Feature(new Point(
            geometry20),
            rightDictionary,
            leftDictionary,
            "abc");

        Assert_Are_Not_Equal(left, right); // different geometries

        left = new Net.Feature.Feature(new Point(
            geometry10),
            leftDictionary,
            rightDictionary,
            "abc");
        right = new Net.Feature.Feature(new Point(
            geometry10),
            rightDictionary,
            leftDictionary,
            "abc"); // identical geometries, different ids and or properties or not compared

        Assert_Are_Equal(left, right);
    }

    [Fact]
    public void Serialized_And_Deserialized_Feature_Equals_And_Share_HashCode()
    {
        var geometry = GetGeometry();

        var leftFeature = new Net.Feature.Feature(geometry, null, null, null);
        var leftJson = JsonConvert.SerializeObject(leftFeature);
        var left = JsonConvert.DeserializeObject<Net.Feature.Feature>(leftJson);

        var rightFeature = new Net.Feature.Feature(geometry, null, null, null);
        var rightJson = JsonConvert.SerializeObject(rightFeature);
        var right = JsonConvert.DeserializeObject<Net.Feature.Feature>(rightJson);

        Assert_Are_Equal(left, right);

        leftFeature = new Net.Feature.Feature(geometry, GetPropertiesInRandomOrder(), null, null);
        leftJson = JsonConvert.SerializeObject(leftFeature);
        left = JsonConvert.DeserializeObject<Net.Feature.Feature>(leftJson);

        rightFeature = new Net.Feature.Feature(geometry, GetPropertiesInRandomOrder(), null, null);
        rightJson = JsonConvert.SerializeObject(rightFeature);
        right = JsonConvert.DeserializeObject<Net.Feature.Feature>(rightJson);

        Assert_Are_Equal(left, right); // assert properties doesn't influence comparison and hashcode

        leftFeature = new Net.Feature.Feature(geometry, null, null, "abc_abc");
        leftJson = JsonConvert.SerializeObject(leftFeature);
        left = JsonConvert.DeserializeObject<Net.Feature.Feature>(leftJson);

        rightFeature = new Net.Feature.Feature(geometry, null, null, "xyz_XYZ");
        rightJson = JsonConvert.SerializeObject(rightFeature);
        right = JsonConvert.DeserializeObject<Net.Feature.Feature>(rightJson);

        Assert_Are_Equal(left, right); // assert id's doesn't influence comparison and hashcode

        leftFeature = new Net.Feature.Feature(geometry, GetPropertiesInRandomOrder(), GetPropertiesInRandomOrder(), "abc");
        leftJson = JsonConvert.SerializeObject(leftFeature);
        left = JsonConvert.DeserializeObject<Net.Feature.Feature>(leftJson);

        rightFeature = new Net.Feature.Feature(geometry, GetPropertiesInRandomOrder(), GetPropertiesInRandomOrder(), "abc");
        rightJson = JsonConvert.SerializeObject(rightFeature);
        right = JsonConvert.DeserializeObject<Net.Feature.Feature>(rightJson);

        Assert_Are_Equal(left, right); // assert id's + properties doesn't influence comparison and hashcode
    }

    [Fact]
    public void Feature_Equals_Null_Issue94()
    {
        bool equal1 = true;
        bool equal2 = true;

        var feature = new Net.Feature.Feature(new Point(new Position(12, 123)), null, null, null);

        var exception = Record.Exception(() =>
        {
            equal1 = feature.Equals(null);
            equal2 = feature == null;
        });

        Assert.Null(exception);

        Assert.False(equal1);
        Assert.False(equal2);
    }

    [Fact]
    public void Feature_Null_Instance_Equals_Null_Issue94()
    {
        var equal1 = true;

        Net.Feature.Feature feature = null;

        var exception = Record.Exception(() =>
        {
            equal1 = feature != null;
        });

        Assert.Null(exception);

        Assert.False(equal1);
    }

    [Fact]
    public void Feature_Equals_Itself_Issue94()
    {
        bool equal1 = false;
        bool equal2 = false;

        var feature = new Net.Feature.Feature(new Point(new Position(12, 123)), null, null, null);
        var exception = Record.Exception(() =>
        {
            #pragma warning disable CS1718 // Comparison made to same variable
            equal1 = feature == feature;
            #pragma warning restore CS1718 // Comparison made to same variable
            equal2 = feature.Equals(feature);
        });

        Assert.Null(exception);

        Assert.True(equal1);
        Assert.True(equal2);
    }

    [Fact]
    public void Feature_Equals_Geometry_Null_Issue115()
    {
        bool equal1 = false;
        bool equal2 = false;

        var feature1 = new Net.Feature.Feature(null, null, null, null);
        var feature2 = new Net.Feature.Feature(new Point(new Position(12, 123)), null, null, null);

        var exception = Record.Exception(() =>
        {
            equal1 = feature1 == feature2;
            equal2 = feature1.Equals(feature2);
        });

        Assert.Null(exception);

        Assert.False(equal1);
        Assert.False(equal2);
    }

    [Fact]
    public void Feature_Equals_Other_Geometry_Null_Issue115()
    {
        bool equal1 = false;
        bool equal2 = false;

        var feature1 = new Net.Feature.Feature(new Point(new Position(12, 123)), null, null, null);
        var feature2 = new Net.Feature.Feature(null, null, null, null);

        var exception = Record.Exception(() =>
        {
            equal1 = feature1 == feature2;
            equal2 = feature1.Equals(feature2);
        });

        Assert.Null(exception);

        Assert.False(equal1);
        Assert.False(equal2);
    }

    [Fact]
    public void Feature_Equals_All_Geometry_Null_Issue115()
    {
        bool equal1 = false;
        bool equal2 = false;

        var feature1 = new Net.Feature.Feature(null, null, null, null);
        var feature2 = new Net.Feature.Feature(null, null, null, null);

        var exception = Record.Exception(() =>
        {
            equal1 = feature1 == feature2;
            equal2 = feature1.Equals(feature2);
        });

        Assert.Null(exception);

        Assert.True(equal1);
        Assert.True(equal2);
    }

    private static IGeometryObject GetGeometry()
    {
        var coordinates = new List<LineString>
        {
            new LineString(new List<IPosition>
            {
                new Position(52.370725881211314, 4.889259338378906),
                new Position(52.3711451105601, 4.895267486572266),
                new Position(52.36931095278263, 4.892091751098633),
                new Position(52.370725881211314, 4.889259338378906)
            }),
            new LineString(new List<IPosition>
            {
                new Position(52.370725881211314, 4.989259338378906),
                new Position(52.3711451105601, 4.995267486572266),
                new Position(52.36931095278263, 4.992091751098633),
                new Position(52.370725881211314, 4.989259338378906)
            })
        };
        var multiLine = new MultiLineString(coordinates);
        return multiLine;
    }

    public static IDictionary<string, object> GetPropertiesInRandomOrder()
    {
        var properties = new Dictionary<string, object>()
        {
            { "DateTimeProperty",  DateTime.Now },
            { "IntProperty",  -1 },
            { "EnumProperty",  TestFeatureEnum.Value1 },
            { "BooleanProperty", true },
            { "DoubleProperty",  1.2345d },
            { "StringProperty",  "Hello, GeoJSON !" }
        };
        var randomlyOrdered = new Dictionary<string, object>();
        var randomlyOrderedKeys = properties.Keys.Select(k => Guid.NewGuid() + k).OrderBy(k => k).ToList();
        foreach (var key in randomlyOrderedKeys)
        {
            var theKey = key[36..];
            randomlyOrdered.Add(theKey, properties[theKey]);
        }
        return randomlyOrdered;
    }

    private static void Assert_Are_Equal(Net.Feature.Feature left, Net.Feature.Feature right)
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

    private static void Assert_Are_Not_Equal(Net.Feature.Feature left, Net.Feature.Feature right)
    {
        Assert.NotEqual(left, right);

        Assert.False(left.Equals(right));
        Assert.False(right.Equals(left));

        Assert.False(left == right);
        Assert.False(right == left);

        Assert.True(left != right);
        Assert.True(right != left);

        Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
    }
}
