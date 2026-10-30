namespace Website.Models;

/// <summary>
/// Content/site.yml 의 구조. 키는 camelCase 로 작성합니다.
/// </summary>
public class SiteData
{
    public SiteInfo Site { get; set; } = new();
    public Profile Profile { get; set; } = new();
    public List<SkillGroup> Skills { get; set; } = [];
    public List<TimelineItem> Timeline { get; set; } = [];
    public List<SocialLink> Links { get; set; } = [];
}

public class SiteInfo
{
    public string Title { get; set; } = "My Website";
    public string Description { get; set; } = "";
    public string Url { get; set; } = "";
    public string Footer { get; set; } = "";
}

public class Profile
{
    public string Name { get; set; } = "";
    public string Greeting { get; set; } = "Hello, I'm";

    /// <summary>히어로에 함께 표시되는 역할들.</summary>
    public List<string> Roles { get; set; } = [];

    public string Tagline { get; set; } = "";
    public string Avatar { get; set; } = "";
    public string Location { get; set; } = "";
    public string Email { get; set; } = "";

    /// <summary>About 섹션 문단들.</summary>
    public List<string> About { get; set; } = [];

    /// <summary>About 섹션 옆 작은 정보 카드들.</summary>
    public List<Fact> Facts { get; set; } = [];
}

public class Fact
{
    public string Label { get; set; } = "";
    public string Value { get; set; } = "";
}

public class SkillGroup
{
    public string Category { get; set; } = "";
    public List<string> Items { get; set; } = [];
}

public class TimelineItem
{
    public string Period { get; set; } = "";
    public string Title { get; set; } = "";
    public string Place { get; set; } = "";
    public string Description { get; set; } = "";
}

public class SocialLink
{
    public string Label { get; set; } = "";
    public string Url { get; set; } = "";

    /// <summary>github, linkedin, x, twitter, mail, instagram, youtube, blog, resume, steam, itch, gamepad, link 중 하나.</summary>
    public string Icon { get; set; } = "link";
}
