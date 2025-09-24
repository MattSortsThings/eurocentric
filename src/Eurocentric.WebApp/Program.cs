WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapGet(
        "/placeholders",
        () => TypedResults.Ok("You don't have to tell me twice! But during the Stone Age...")
    )
    .AllowAnonymous();

app.Run();
