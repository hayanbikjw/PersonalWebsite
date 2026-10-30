using BlazorStatic;

namespace Website.Models;

/// <summary>
/// Content/Projects/*.md 의 YAML front matter. 키는 camelCase 로 작성합니다.
/// </summary>
public class ProjectFrontMatter : IFrontMatter, IFrontMatterWithTags
{
    public string Title { get; set; } = "";

    /// <summary>카드와 상세 페이지 상단에 표시되는 한 줄 소개.</summary>
    public string Lead { get; set; } = "";

    /// <summary>프로젝트 정렬 기준 날짜 (최신순).</summary>
    public DateTime Published { get; set; } = DateTime.Now;

    /// <summary>개발 기간 (예: "2025.06 - 2025.07"). 비우면 published 의 연월을 표시.</summary>
    public string Period { get; set; } = "";

    /// <summary>팀명. 비우면 1인 개발로 표시.</summary>
    public string Team { get; set; } = "";

    /// <summary>맡은 역할 (예: "프로그래머").</summary>
    public string Role { get; set; } = "";

    /// <summary>예: "진행 중", "완료", "유지보수".</summary>
    public string Status { get; set; } = "";

    /// <summary>썸네일 이미지 경로 (wwwroot 기준, 예: images/projects/foo.png). 비우면 color 배경으로 대체.</summary>
    public string Thumbnail { get; set; } = "";

    /// <summary>카드 강조 색 (CSS 색상값).</summary>
    public string Color { get; set; } = "";

    public string Github { get; set; } = "";

    /// <summary>Steam, itch.io 등 플랫폼 링크. 항목마다 상세 페이지에 버튼이 하나씩 생깁니다.</summary>
    public List<SocialLink> Links { get; set; } = [];

    /// <summary>true 면 홈 화면 "Featured Projects" 에 노출.</summary>
    public bool Featured { get; set; }

    public bool IsDraft { get; set; }

    public List<string> Tags { get; set; } = [];

    public AdditionalInfo? AdditionalInfo => new() { LastMod = Published };
}
