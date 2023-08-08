using Pyco.Todo;
using Pyco.Todo.Core.Authorization;
using Pyco.Todo.Core.Exception;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

#if DEBUG
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_myAllowSpecificOrigins",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
#endif

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
//app.UseCookiePolicy();

app.UseRouting();
// app.UseRateLimiter();
// app.UseRequestLocalization();
// app.UseCors();

#if DEBUG
app.UseCors("_myAllowSpecificOrigins");
#endif

app.UseAuthentication();
app.UseAuthorization();
// app.UseSession();
// app.UseResponseCompression();
// app.UseResponseCaching();

// custom middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapRazorPages();
app.MapControllers();

//app.MapGet("/", () => "blub");

app.Run();
