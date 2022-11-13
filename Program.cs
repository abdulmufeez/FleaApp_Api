using fleaApi.Extensions;
using FleaApp_Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

await app.UseAutoMigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts => 
    {
        opts.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

var externalUrl = builder.Configuration.GetValue<string>("ExternalApplicationUrl");
//assigning policy for Cross Origin Response
// app.UseCors(policy => policy
//     .AllowAnyHeader().AllowAnyMethod()
//     .AllowCredentials().WithOrigins(externalUrl));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
