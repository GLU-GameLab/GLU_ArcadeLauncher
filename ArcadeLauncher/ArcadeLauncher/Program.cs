using ArcadeLauncher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<GamesData>();
builder.Services.AddHostedService<GameDownloadService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbservice = scope.ServiceProvider.GetService<GamesData>();
    //dbservice?.Database.EnsureDeleted();
    //var result = dbservice?.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints => endpoints.MapBlazorHub());

app.Run();
