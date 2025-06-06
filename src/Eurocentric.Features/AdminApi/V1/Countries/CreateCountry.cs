using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record CreateCountryResponse(CountryDto Country) : IExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample()
    {
        CountryDto country = CountryDto.CreateExample() with { ParticipatingContests = [] };

        return new CreateCountryResponse(country);
    }
}

public sealed record CreateCountryRequest : IExampleProvider<CreateCountryRequest>
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() => new() { CountryCode = "AT", CountryName = "Austria" };
}

internal static class CreateCountry
{
    internal static IEndpointRouteBuilder MapCreateCountry(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("countries", HandleAsync)
            .WithName(EndpointIds.Countries.CreateCountry)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country.")
            .HasApiVersion(1, 0)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .WithTags(EndpointTags.Countries);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateCountryResponse>>> HandleAsync(
        CreateCountryRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(CreateCountryRequest requestBody) =>
        ErrorOrFactory.From(new Command(requestBody.CountryCode, requestBody.CountryName));

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointIds.Countries.GetCountry,
            new RouteValueDictionary { ["countryId"] = response.Country.Id });

    private static CountryBuilder SpecifyFrom(this CountryBuilder builder, Command command) =>
        builder.WithCountryCode(CountryCode.FromValue(command.CountryCode))
            .WithCountryName(CountryName.FromValue(command.CountryName));

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<CreateCountryResponse>;

    internal sealed class Handler(AppDbContext dbContext, ICountryIdGenerator idGenerator) :
        ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await Country.Create()
                .SpecifyFrom(command)
                .Build(idGenerator)
                .FailOnCountryCodeConflict(dbContext.Countries.AsNoTracking().AsSplitQuery())
                .ThenDo(country => dbContext.Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(country => new CreateCountryResponse(country.ToCountryDto()));
    }
}
