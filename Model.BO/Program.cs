using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Model.BO.Data.Model;
using Model.BO.Data.Model.DemoDb;
using Model.BO.Data.Security;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ModelDbContext>(options =>
    options.UseSqlite("Data Source=demo.db"));

//builder.Services.AddDbContext<ModelDbContext>((serviceProvider, options) =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnectionString"));
//});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();


    Dictionary<string, List<string>> accessGroups = AccessRights.getAccessRightGroups();
    foreach (var group in accessGroups)
    {
        var allowedRoles = group.Value ?? [];

        if (builder.Configuration.GetValue<bool>("BypassAccessRights"))
        {
            options.AddPolicy("Require" + group.Key, policy => policy.RequireAssertion(context =>
            {
                    return true;
            }));
        }
        else
        {
            options.AddPolicy("Require" + group.Key, policy => policy.RequireRole((allowedRoles.Count != 0) ? allowedRoles : [""]));
        }

    }

});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = NegotiateDefaults.AuthenticationScheme;
        options.DefaultScheme = "Windows";
        options.DefaultChallengeScheme = NegotiateDefaults.AuthenticationScheme;
    })
   .AddNegotiate();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/UserData/Accounts", "");
    });



var app = builder.Build();

app.UsePathBase("/BO");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ModelDbContext>();
    DemoDb.GenerateFakeDb(context);
}

app.Run();


