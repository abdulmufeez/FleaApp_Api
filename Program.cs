using fleaApi.Extensions;
using FleaApp_Api.Extensions;
using FleaApp_Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

await app.UseAutoMigrateAsync();

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.RoutePrefix = "swagger";
});

app.UseMiddleware<ApiExceptionMiddleware>();

app.UseHttpsRedirection();

var externalUrl = builder.Configuration.GetValue<string>("ExternalApplicationUrl");
//assigning policy for Cross Origin Response
app.UseCors(policy => policy
    .AllowAnyHeader().AllowAnyMethod()
    .AllowCredentials().WithOrigins(externalUrl));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapFallbackToController("Index","Fallback"); 

app.Run();
