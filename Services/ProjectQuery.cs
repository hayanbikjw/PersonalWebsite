using BlazorStatic;
using Website.Models;

namespace Website.Services;

public static class ProjectQuery
{
    /// <summary>날짜(published) 기준 최신순.</summary>
    public static IEnumerable<Post<ProjectFrontMatter>> Sorted(this IEnumerable<Post<ProjectFrontMatter>> posts) =>
        posts.OrderByDescending(p => p.FrontMatter.Published);

    /// <summary>개발 기간. period 가 없으면 published 의 연월.</summary>
    public static string PeriodText(this ProjectFrontMatter fm) =>
        string.IsNullOrWhiteSpace(fm.Period) ? fm.Published.ToString("yyyy.MM") : fm.Period;

    /// <summary>제작자 표기. 팀이면 "팀명 · 역할", 아니면 "이름 · 1인 개발".</summary>
    public static string CreditText(this ProjectFrontMatter fm, string owner)
    {
        var solo = string.IsNullOrWhiteSpace(fm.Team);
        var role = string.IsNullOrWhiteSpace(fm.Role) ? (solo ? "1인 개발" : "") : fm.Role;
        var maker = solo ? owner : fm.Team;
        return role.Length == 0 ? maker : $"{maker} · {role}";
    }
}
