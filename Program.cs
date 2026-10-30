using BlazorStatic;
using Website;
using Website.Components;
using Website.Models;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

// wwwroot 등 정적 자산을 BlazorStatic 출력물에 포함
builder.WebHost.UseStaticWebAssets();

builder.Services.AddBlazorStaticService(opt =>
{
    // sitemap.xml 생성 (Content/site.yml 의 site.url 과 같은 값을 넣어주세요)
    opt.ShouldGenerateSitemap = true;
    opt.SiteUrl = WebsiteKeys.SiteUrl;
})
.AddBlazorStaticContentService<ProjectFrontMatter>(opt =>
{
    // Content/Projects/*.md -> /projects/{파일명}
    opt.ContentPath = Path.Combine("Content", "Projects");
    opt.PageUrl = WebsiteKeys.ProjectsUrl;
    // 태그는 프로젝트 목록 페이지에서 필터로만 사용
    opt.Tags.AddTagPagesFromPosts = false;
});

// 프로필/링크/스킬 등 사이트 데이터 (Content/site.yml)
builder.Services.AddSingleton<SiteDataService>();

builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

// 개발 환경이 아니면 정적 파일(output 폴더) 생성 후 앱 종료 -> CI/CD 에 적합
app.UseBlazorStaticGenerator(shutdownApp: !app.Environment.IsDevelopment());

app.Run();

namespace Website
{
    public static class WebsiteKeys
    {
        public const string ProjectsUrl = "projects";

        // 배포 주소 (예: https://username.github.io)
        public const string SiteUrl = "https://example.com";
    }
}
