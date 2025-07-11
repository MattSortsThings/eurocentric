using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Country = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Contracts.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record CreateCountryRequest : IExampleProvider<CreateCountryRequest>
{
    /// <summary>
    ///     The country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    ///     The country's short UK English name.
    /// </summary>
    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() => new() { CountryCode = "AT", CountryName = "Austria" };
}

public sealed record CreateCountryResponse(CountryDto Country) : IExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(CountryDto.CreateExample() with { ParticipatingContestIds = [] });
}

internal static class CreateCountry
{
    internal static IEndpointRouteBuilder MapCreateCountry(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapPost("countries", ExecuteAsync)
            .WithName(EndpointConstants.Names.Countries.CreateCountry)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country.")
            .WithTags(EndpointConstants.Tags.Countries)
            .HasApiVersion(1, 0)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateCountryResponse>>> ExecuteAsync(
        [FromBody] CreateCountryRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(CreateCountryRequest requestBody) =>
        ErrorOrFactory.From(new Command(requestBody.CountryCode, requestBody.CountryName));

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointConstants.Names.Countries.GetCountry,
            new RouteValueDictionary { { "countryId", response.Country.Id } });

    private static CountryBuilder Apply(this CountryBuilder builder, Command command) =>
        builder.WithCountryCode(CountryCode.FromValue(command.CountryCode))
            .WithCountryName(CountryName.FromValue(command.CountryName));

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<CreateCountryResponse>;

    internal sealed class Handler(AppDbContext dbContext, ICountryIdGenerator idGenerator) :
        ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            Func<Country, CountryDto> mapper = Projections.ProjectToCountryDto.Compile();

            return await Country.Create()
                .Apply(command)
                .Build(idGenerator.CreateSingle)
                .FailOnCountryCodeConflict(dbContext.Countries.AsNoTracking())
                .ThenDo(country => dbContext.Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(country => new CreateCountryResponse(mapper(country)));
        }
    }
}
