using Website.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Website.Services;

/// <summary>
/// Content/site.yml 을 읽어 제공합니다. 파일이 바뀌면 다음 요청에서 다시 읽습니다.
/// </summary>
public class SiteDataService(ILogger<SiteDataService> logger)
{
    static readonly string FilePath = Path.Combine("Content", "site.yml");

    readonly IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    readonly Lock gate = new();
    SiteData? cached;
    DateTime cachedWriteTime;

    public SiteData Data
    {
        get
        {
            lock (gate)
            {
                if (!File.Exists(FilePath))
                {
                    logger.LogWarning("{Path} 파일이 없습니다. 기본값을 사용합니다.", FilePath);
                    return cached ??= new SiteData();
                }

                var writeTime = File.GetLastWriteTimeUtc(FilePath);
                if (cached is null || writeTime != cachedWriteTime)
                {
                    cached = deserializer.Deserialize<SiteData>(File.ReadAllText(FilePath)) ?? new SiteData();
                    cachedWriteTime = writeTime;
                }
                return cached;
            }
        }
    }
}
