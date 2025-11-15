using System.Reflection;
using Gorge.Core;
using Raylib_cs;

namespace Gorge.Services;

/// <summary>
/// Service to assist with getting resources from the assembly
/// </summary>
public class ResourceService : BaseService
{
    Assembly assembly = Assembly.GetExecutingAssembly();
    private string? assemblyName;
    public override void Start()
    {
        base.Start();
        assemblyName = assembly.GetName().Name?.Replace("-", "_"); ;

        Log.Info($"Assembly v{assembly.GetName().Version}");
    }

    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Fetches an EmbeddedResource
    /// </summary>
    /// <param name="resourcePath">Resource path NOT including namespace or .assets.</param>
    /// <returns>Byte array of resource</returns>
    public byte[] GetResource(string resourcePath)
    {
        using var rs = assembly.GetManifestResourceStream(assemblyName + ".assets." + resourcePath);
        if (rs == null)
        {
            throw Log.Error("Resource not found " + resourcePath);
        }
        using (var ms = new MemoryStream())
        {
            rs.CopyTo(ms);
            return ms.ToArray();
        }
    }

    public Image GetImage(string resourcePath)
    {
        byte[] data = GetResource(resourcePath);
        var ext = Path.GetExtension(resourcePath);
        return Raylib.LoadImageFromMemory(ext, data); ;
    }

    public Texture2D GetTexture(string resourcePath)
    {
        var img = GetImage(resourcePath);
        return Raylib.LoadTextureFromImage(img);
    }

    public unsafe Shader GetShader(string vsPath, string fsPath)
    {
        var vs = GetResource(vsPath);
        var fs = GetResource(fsPath);

        fixed (byte* pVs = vs)
        fixed (byte* pFs = fs)
        {
            return Raylib.LoadShaderFromMemory((sbyte*)pVs, (sbyte*)pFs);
        }
    }

    public override void Stop()
    {
        base.Stop();
    }
}