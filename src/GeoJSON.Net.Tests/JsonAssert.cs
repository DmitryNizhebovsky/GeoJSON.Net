using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoJSON.Net.Tests
{
    /// <summary>
    ///     Assertions for json strings
    /// </summary>
    public static class JsonAssert
    {
        /// <summary>
        ///     Asserts that the json strings are equal.
        /// </summary>
        /// <remarks>
        ///     Parses each json string into a <see cref="JObject" />, sorts the properties of each
        ///     and then serializes each back to a json string for comparison.
        /// </remarks>
        /// <param name="expectJson">The expect json.</param>
        /// <param name="actualJson">The actual json.</param>
        public static void AreEqual(string expectJson, string actualJson)
        {
            Assert.Equal(
                JObject.Parse(expectJson).SortProperties().ToString(),
                JObject.Parse(actualJson).SortProperties().ToString());
        }

        /// <summary>
        ///     Asserts that the json strings are equal.
        /// </summary>
        /// <remarks>
        ///     Parses each json string into a <see cref="JArray" />
        ///     and then serializes each back to a json string for comparison.
        /// </remarks>
        /// <param name="expectJson">The expect json.</param>
        /// <param name="actualJson">The actual json.</param>
        public static void AreEqualArray(string expectJson, string actualJson)
        {
            Assert.Equal(
                JArray.Parse(expectJson).ToString(),
                JArray.Parse(actualJson).ToString());
        }

        /// <summary>
        ///     Asserts that <paramref name="actualJson" /> contains <paramref name="expectedJson" />
        /// </summary>
        /// <param name="expectedJson">The expected json.</param>
        /// <param name="actualJson">The actual json.</param>
        public static void Contains(string expectedJson, string actualJson)
        {
            Assert.Contains(expectedJson, actualJson);
        }

        /// <summary>
        ///     Sorts the properties of a JObject
        /// </summary>
        /// <param name="jObject">The json object whhose properties to sort</param>
        /// <returns>A new instance of a <see cref="JObject" /> with sorted properties</returns>
        private static JObject SortProperties(this JObject jObject)
        {
            var result = new JObject();

            foreach (var property in jObject.Properties().OrderBy(p => p.Name))
            {
                var value = property.Value as JObject;

                if (value != null)
                {
                    value = value.SortProperties();
                    result.Add(property.Name, value);
                }
                else
                {
                    result.Add(property.Name, property.Value);
                }
            }

            return result;
        }
    }
}