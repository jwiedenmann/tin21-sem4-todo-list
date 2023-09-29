using Pyco.Todo;
using Pyco.Todo.Core.Authorization;
using Pyco.Todo.Core.Exception;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

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

//#if DEBUG
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:8080")
    .WithOrigins("http://localhost:5000")
    .WithOrigins("http://localhost:80")
    .WithOrigins("http://localhost/")
    .WithOrigins("http://localhost")
    .AllowCredentials());
//#endif

//app.UseHttpsRedirection();
//app.UseDefaultFiles();
app.UseStaticFiles();
//app.UseCookiePolicy();

app.UseRouting();
// app.UseRateLimiter();
// app.UseRequestLocalization();

//app.UseAuthentication();
//app.UseAuthorization();
// app.UseSession();
// app.UseResponseCompression();
// app.UseResponseCaching();

// custom middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapRazorPages();
app.MapControllers();

app.Run();
