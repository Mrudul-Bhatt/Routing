using Routing.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.ConstraintMap.Add("months", typeof(RouteConstraints)));

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapGet
    //endpoints.MapPost
    //endpoints.MapControllers

    //Will hit all get, post , delete on this endpoint
    endpoints.Map("map1", async (context) =>
    {
        await context.Response.WriteAsync("map1");
    });

    //will hit only post request on this url
    endpoints.MapPost("map2", async (context) =>
    {
        await context.Response.WriteAsync("map2");
    });

    endpoints.Map("files/{filename}.{extension}", async (context) =>
    {
        var filename = context.Request.RouteValues["filename"];
        //route param names are case insensitive
        var extension = context.Request.RouteValues["EXTENSION"];

        await context.Response.WriteAsync($"filename: {filename}, extension: {extension}");
    });

    //route with default params
    endpoints.Map("employee/{name=john}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];

        await context.Response.WriteAsync($"name: {name}");
    });

    //route with optional params
    endpoints.Map("employee/{name:string?}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];

        if (name == null)
        {
            await context.Response.WriteAsync("name is null");
        }
        else
        {
            await context.Response.WriteAsync($"name: {name}");
        }
    });

    //route with multiple params and route constraints on id param to be of type int
    endpoints.Map("employee/{name}/{id:int:range(1,1000)}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];
        var id = context.Request.RouteValues["id"];

        await context.Response.WriteAsync($"name: {name}, id: {id}");
    });

    //route with multiple params and route constraints on id param to be of type int
    endpoints.Map("employee/{name:minlength(10)=harsha}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];
        var id = context.Request.RouteValues["id"];

        await context.Response.WriteAsync($"name: {name}, id: {id}");
    });

    //route with regex constraint on name param
    endpoints.Map("employee/{name:regex(wdqwqd)}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];
        var id = context.Request.RouteValues["id"];

        await context.Response.WriteAsync($"name: {name}, id: {id}");
    });

    endpoints.Map("employee/{month:months}", async (context) =>
    {
        var name = context.Request.RouteValues["name"];
        var id = context.Request.RouteValues["id"];

        await context.Response.WriteAsync($"name: {name}, id: {id}");
    });


});

app.Run();
