using System;
using System.Collections.Generic;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Xunit;

namespace GeoJSON.Net.Tests.Geometry;

public class GeometryTests : TestBase
{
    public static IEnumerable<object[]> Geometries
    {
        get
        {
            var point = new Point(new Position(1, 2));

            var multiPoint = new MultiPoint(new List<Point>
            {
                new Point(new Position(52.379790828551016, 5.3173828125)),
                new Point(new Position(52.36721467920585, 5.456085205078125)),
                new Point(new Position(52.303440474272755, 5.386047363281249))
            });

            var lineString = new LineString(new List<IPosition>
            {
                new Position(52.379790828551016, 5.3173828125),
                new Position(52.36721467920585, 5.456085205078125),
                new Position(52.303440474272755, 5.386047363281249)
            });

            var multiLineString = new MultiLineString(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3173828125),
                    new Position(52.36721467920585, 5.456085205078125),
                    new Position(52.303440474272755, 5.386047363281249)
                }),
                new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3273828125),
                    new Position(52.36721467920585, 5.486085205078125),
                    new Position(52.303440474272755, 5.426047363281249)
                })
            });

            var polygon = new Polygon(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3173828125),
                    new Position(52.36721467920585, 5.456085205078125),
                    new Position(52.303440474272755, 5.386047363281249),
                    new Position(52.379790828551016, 5.3173828125)
                })
            });

            var multiPolygon = new MultiPolygon(new List<Polygon>
            {
                new Polygon(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(52.959676831105995, -2.6797102391514338),
                        new Position(52.9608756693609, -2.6769029474483279),
                        new Position(52.908449372833715, -2.6079763270327119),
                        new Position(52.891287242948195, -2.5815104708998668),
                        new Position(52.875476700983896, -2.5851645010668989),
                        new Position(52.882954723868622, -2.6050779098387191),
                        new Position(52.875255907042678, -2.6373482332006359),
                        new Position(52.878791122091066, -2.6932445076063951),
                        new Position(52.89564268523565, -2.6931334629377890),
                        new Position(52.930592009390175, -2.6548779332193022),
                        new Position(52.959676831105995, -2.6797102391514338)
                    })
                }),
                new Polygon(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(52.89610842810761, -2.69628632041613),
                        new Position(52.8894641454077, -2.75901233808515),
                        new Position(52.89938894657412, -2.7663172788742449),
                        new Position(52.90253773227807, -2.804554822840895),
                        new Position(52.929801009654575, -2.83848602260174),
                        new Position(52.94013913205788, -2.838979264607087),
                        new Position(52.937353122653533, -2.7978187468478741),
                        new Position(52.920394929466184, -2.772273870352612),
                        new Position(52.926572918779222, -2.6996509024137052),
                        new Position(52.89610842810761, -2.69628632041613)
                    })
                })
            });

            yield return new object[] { point };
            yield return new object[] { multiPoint };
            yield return new object[] { lineString };
            yield return new object[] { multiLineString };
            yield return new object[] { polygon };
            yield return new object[] { multiPolygon };
            yield return new object[] {
                new GeometryCollection(new List<IGeometryObject>
                {
                    point,
                    multiPoint,
                    lineString,
                    multiLineString,
                    polygon,
                    multiPolygon
                })
            };
        }
    }

    [Theory]
    [MemberData(nameof(Geometries))]
    public void Can_Serialize_And_Deserialize_Geometry(IGeometryObject geometry)
    {
        var json = JsonConvert.SerializeObject(geometry);

        var deserializedGeometry = JsonConvert.DeserializeObject<IGeometryObject>(json, new GeometryConverter());

        Assert.Equal(geometry, deserializedGeometry);
    }

    [Theory]
    [MemberData(nameof(Geometries))]
    public void Serialization_Observes_Indenting_Setting_Of_Serializer(IGeometryObject geometry)
    {
        var json = JsonConvert.SerializeObject(geometry, Formatting.Indented);
        Assert.Contains(Environment.NewLine, json);
    }

    [Theory]
    [MemberData(nameof(Geometries))]
    public void Serialization_Observes_No_Indenting_Setting_Of_Serializer(IGeometryObject geometry)
    {
        var json = JsonConvert.SerializeObject(geometry, Formatting.None);
        Assert.DoesNotContain(Environment.NewLine, json);
        Assert.False(json.Contains(' '));
    }

    [Theory]
    [MemberData(nameof(Geometries))]
    public void Can_Serialize_And_Deserialize_Geometry_As_Object_Property(IGeometryObject geometry)
    {
        var classWithGeometry = new ClassWithGeometryProperty(geometry);

        var json = JsonConvert.SerializeObject(classWithGeometry);

        var deserializedClassWithGeometry = JsonConvert.DeserializeObject<ClassWithGeometryProperty>(json);

        Assert.Equal(classWithGeometry, deserializedClassWithGeometry);
    }

    [Theory]
    [MemberData(nameof(Geometries))]
    public void Serialized_And_Deserialized_Equals_And_Share_HashCode(IGeometryObject geometry)
    {
        var classWithGeometry = new ClassWithGeometryProperty(geometry);

        var json = JsonConvert.SerializeObject(classWithGeometry);

        var deserializedClassWithGeometry = JsonConvert.DeserializeObject<ClassWithGeometryProperty>(json);

        var actual = classWithGeometry;
        var expected = deserializedClassWithGeometry;

        Assert.True(actual.Equals(expected));
        Assert.True(actual.Equals(actual));

        Assert.True(expected.Equals(actual));
        Assert.True(expected.Equals(expected));

        Assert.True(classWithGeometry == deserializedClassWithGeometry);
        Assert.True(deserializedClassWithGeometry == classWithGeometry);

        Assert.Equal(actual.GetHashCode(), expected.GetHashCode());
    }

    internal class ClassWithGeometryProperty
    {
        public ClassWithGeometryProperty(IGeometryObject geometry)
        {
            Geometry = geometry;
        }

        [JsonConverter(typeof(GeometryConverter))]
        public IGeometryObject Geometry { get; set; }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ClassWithGeometryProperty)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return Geometry.GetHashCode();
        }

        public static bool operator ==(ClassWithGeometryProperty left, ClassWithGeometryProperty right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClassWithGeometryProperty left, ClassWithGeometryProperty right)
        {
            return !Equals(left, right);
        }

        private bool Equals(ClassWithGeometryProperty other)
        {
            return Geometry.Equals(other.Geometry);
        }
    }
}
