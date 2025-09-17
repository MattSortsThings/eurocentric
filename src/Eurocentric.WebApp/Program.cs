WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/placeholder", () => TypedResults.Ok("You don't have to tell me twice! But during the Stone Age..."))
    .WithName("GetPlaceholder")
    .WithSummary("Get a placeholder message")
    .AllowAnonymous();

app.Run();
