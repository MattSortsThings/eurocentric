using System.ComponentModel;
using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Dtos;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Apis.Admin.V0.Versioning;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Country = Eurocentric.Domain.V0Entities.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Country;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public static class CreateCountry
{
    internal static IEndpointRouteBuilder MapCreateCountry(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/countries", ExecuteAsync)
            .WithName(V0Group.Countries.Endpoints.CreateCountry)
            .IntroducedInV0Point1()
            .WithTags(V0Group.Countries.Tag)
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromBody] Request request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(request, cancellationToken: cancellationToken);

        return MapToCreatedAtRoute(errorsOrResponse.Value);
    }

    private static CreatedAtRoute<Response> MapToCreatedAtRoute(Response response)
    {
        Guid countryId = response.Country.Id;

        return TypedResults.CreatedAtRoute(response,
            V0Group.Countries.Endpoints.GetCountry,
            new RouteValueDictionary { { nameof(countryId), countryId } });
    }

    public sealed record Request : ICommand<Response>
    {
        public required string CountryCode { get; init; }

        public required string CountryName { get; init; }

        public required CountryType CountryType { get; init; }
    }

    public sealed record Response(CountryDto Country);

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Request, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Request command, CancellationToken cancellationToken) =>
            await GetFactoryFunction(command.CountryType)
                .Invoke(command.CountryCode, command.CountryName)
                .FailIf(CountryCodeNotUnique, CountryCodeConflict(command.CountryCode))
                .ThenDo(country => dbContext.V0Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(MapToResponse);

        private bool CountryCodeNotUnique(Country country) =>
            dbContext.V0Countries.Any(existingCountry => existingCountry.CountryCode == country.CountryCode);

        private static Func<string, string, ErrorOr<Country>> GetFactoryFunction(CountryType countryType) => countryType switch
        {
            CountryType.Real => Country.CreateReal,
            CountryType.Pseudo => Country.CreatePseudo,
            _ => throw new InvalidEnumArgumentException(nameof(countryType), (int)countryType, typeof(CountryType))
        };

        private static Error CountryCodeConflict(string countryCode) => Error.Conflict("Country code conflict",
            "A country exists with the provided country code.",
            new Dictionary<string, object> { { nameof(countryCode), countryCode } });

        private static Response MapToResponse(Country country)
        {
            Func<Country, CountryDto> mapper = OutboundMapping.CountryToCountryDto.Compile();

            return new Response(mapper(country));
        }
    }
}
