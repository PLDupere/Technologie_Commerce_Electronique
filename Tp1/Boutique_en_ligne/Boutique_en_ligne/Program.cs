var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
var app = builder.Build();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "Inscription", 
        template: "Inscription/Inscription", 
        defaults: new { controller = "Inscription", action = "Inscription" } 
    );


    routes.MapRoute(
        name: "Authentification", 
        template: "Authentification/Authentification", 
        defaults: new { controller = "Authentification", action = "Authentification" } 
    );

    routes.MapRoute(
        name: "Default",
        template: "{controller=Home}/{action=Index}");
});


app.UseFileServer();

app.Run();
