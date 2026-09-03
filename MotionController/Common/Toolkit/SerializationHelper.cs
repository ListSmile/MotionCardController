using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// 提供对象与本地文件之间的 JSON/XML 序列化与反序列化帮助方法。
/// </summary>
public static class SerializationHelper
{
    // ---------- JSON ----------

    /// <summary>
    /// 将对象以 JSON 格式同步保存到指定文件（默认 UTF-8 无 BOM，缩进格式化）。
    /// </summary>
    public static void SaveToJson<T>(T obj, string filePath, JsonSerializerOptions? options = null)
    {
        EnsureDirectory(filePath);
        options ??= GetDefaultJsonOptions();

        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    /// <summary>
    /// 从 JSON 文件同步加载并反序列化为指定类型。
    /// </summary>
    public static T LoadFromJson<T>(string filePath, JsonSerializerOptions? options = null)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"文件不存在：{filePath}");

        string json = File.ReadAllText(filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<T>(json, options)
               ?? throw new InvalidOperationException($"反序列化失败，返回 null。");
    }

    /// <summary>
    /// 将对象以 JSON 格式异步保存到指定文件（推荐用于 UI 应用）。
    /// </summary>
    public static async Task SaveToJsonAsync<T>(T obj, string filePath, JsonSerializerOptions? options = null)
    {
        EnsureDirectory(filePath);
        options ??= GetDefaultJsonOptions();

        await using FileStream fs = File.Create(filePath);
        await JsonSerializer.SerializeAsync(fs, obj, options);
        await fs.FlushAsync();
    }

    /// <summary>
    /// 从 JSON 文件异步加载并反序列化为指定类型。
    /// </summary>
    public static async Task<T> LoadFromJsonAsync<T>(string filePath, JsonSerializerOptions? options = null)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"文件不存在：{filePath}");

        await using FileStream fs = File.OpenRead(filePath);
        T? result = await JsonSerializer.DeserializeAsync<T>(fs, options);
        return result ?? throw new InvalidOperationException($"反序列化失败，返回 null。");
    }

    // ---------- XML ----------

    /// <summary>
    /// 将对象以 XML 格式同步保存到指定文件（UTF-8 无 BOM，缩进格式化）。
    /// </summary>
    public static void SaveToXml<T>(T obj, string filePath, XmlSerializerNamespaces? namespaces = null)
    {
        EnsureDirectory(filePath);

        var serializer = new XmlSerializer(typeof(T));
        using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(fs, new UTF8Encoding(false)); // 无 BOM
        namespaces ??= new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        serializer.Serialize(writer, obj, namespaces);
    }

    /// <summary>
    /// 从 XML 文件同步加载并反序列化为指定类型。
    /// </summary>
    public static T LoadFromXml<T>(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"文件不存在：{filePath}");

        var serializer = new XmlSerializer(typeof(T));
        using var fs = File.OpenRead(filePath);
        using var reader = new StreamReader(fs, Encoding.UTF8);
        object? result = serializer.Deserialize(reader);
        return result is T t ? t : throw new InvalidOperationException($"反序列化失败，返回 null。");
    }

    /// <summary>
    /// 将对象以 XML 格式异步保存到指定文件（内部使用 Task.Run 包装同步操作）。
    /// </summary>
    public static async Task SaveToXmlAsync<T>(T obj, string filePath, XmlSerializerNamespaces? namespaces = null)
    {
        // XmlSerializer 没有原生异步方法，用 Task.Run 包装，避免阻塞 UI 线程
        await Task.Run(() => SaveToXml(obj, filePath, namespaces));
    }

    /// <summary>
    /// 从 XML 文件异步加载并反序列化为指定类型（内部使用 Task.Run 包装同步操作）。
    /// </summary>
    public static async Task<T> LoadFromXmlAsync<T>(string filePath)
    {
        return await Task.Run(() => LoadFromXml<T>(filePath));
    }

    // ---------- 辅助方法 ----------

    /// <summary>
    /// 确保目标文件所在的目录存在。
    /// </summary>
    private static void EnsureDirectory(string filePath)
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
    }

    /// <summary>
    /// 获取默认的 JSON 序列化选项（缩进、中文不转义）。
    /// </summary>
    private static JsonSerializerOptions GetDefaultJsonOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            // 可根据需要添加其他全局配置，如忽略循环引用等
        };
    }
}