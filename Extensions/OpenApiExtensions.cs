namespace JustAnApi.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "JustAnApi";
                document.Info.Version = "v1";
                document.Info.Description = "A small sample API that returns randomly generated weather forecasts.";
                return Task.CompletedTask;
            });
        });

        return services;
    }
}
