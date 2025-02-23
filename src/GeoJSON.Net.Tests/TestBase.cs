using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GeoJSON.Net.Tests;

public abstract class TestBase
{
    private static readonly Assembly ThisAssembly = typeof(TestBase).Assembly;
    private static readonly string AssemblyName = ThisAssembly.GetName().Name;

    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = ThisAssembly.Location;
            UriBuilder uri = new(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }

    protected string GetExpectedJson([CallerMemberName] string name = null)
    {
        var type = GetType().Name;
        var projectFolder = GetType().Namespace[(AssemblyName.Length + 1)..];
        var path = Path.Combine(AssemblyDirectory, @"./", projectFolder, type + "_" + name + ".json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("file not found at " + path);
        }

        return File.ReadAllText(path);
    }
}
