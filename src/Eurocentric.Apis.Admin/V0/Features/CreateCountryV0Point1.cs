using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Contracts.Mapping;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Admin.V0.Features;

internal static class CreateCountryV0Point1
{
    internal static IEndpointRouteBuilder MapCreateCountryV0Point1(
        this IEndpointRouteBuilder builder
    )
    {
        builder
            .MapPost("v0.1/countries", ExecuteAsync)
            .WithName("AdminApi.V0.Countries.CreateCountryV0Point1")
            .WithTags(V0Group.Countries.Tag)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        ErrorOr<Result> errorsOrResult = await bus.Send(
            request.ToCommand(),
            cancellationToken: cancellationToken
        );

        return MapToCreatedAtRoute(errorsOrResult.Value);
    }

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(in Result result)
    {
        CreateCountryResponse response = new(result.Country.ToCountryDto());

        Guid countryId = response.Country.Id;

        return TypedResults.CreatedAtRoute(
            response,
            "AdminApi.V0.Countries.GetCountryV0Point1",
            new RouteValueDictionary { { nameof(countryId), countryId } }
        );
    }

    private static Command ToCommand(this CreateCountryRequest request) =>
        new(request.CountryCode, request.CountryName);

    internal readonly record struct Result(Country Country);

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<Result>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(
            Command command,
            CancellationToken cancellationToken
        )
        {
            (string countryCode, string countryName) = command;

            if (countryCode.Length != 2 || !countryCode.All(char.IsAsciiLetterUpper))
            {
                return IllegalCountryCodeValue(countryCode);
            }

            if (
                dbContext
                    .Countries.AsNoTracking()
                    .Any(country => country.CountryCode == countryCode)
            )
            {
                return CountryCodeConflict(countryCode);
            }

            Country country = new()
            {
                Id = Guid.NewGuid(),
                CountryCode = countryCode,
                CountryName = countryName,
                ContestRoles = [],
            };

            dbContext.Countries.Add(country);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new Result(country);
        }

        private static Error IllegalCountryCodeValue(string countryCode)
        {
            return Error.Failure(
                "Illegal country code value",
                "Country code value must be a string of 2 upper-case letters.",
                new Dictionary<string, object> { { nameof(countryCode), countryCode } }
            );
        }

        private static Error CountryCodeConflict(string countryCode)
        {
            return Error.Conflict(
                "Country code conflict",
                "A country already exists with the provided country code.",
                new Dictionary<string, object> { { nameof(countryCode), countryCode } }
            );
        }
    }
}
