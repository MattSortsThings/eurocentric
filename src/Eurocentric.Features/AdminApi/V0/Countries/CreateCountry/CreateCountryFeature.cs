using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.V0.Entities;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;

internal static class CreateCountryFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest requestBody,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(requestBody.ToCommand(), MapToCreatedAtRoute, cancellationToken);

    private static Command ToCommand(this CreateCountryRequest requestBody) =>
        new(requestBody.CountryCode, requestBody.CountryName, requestBody.CountryType);

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse responseBody)
    {
        Guid countryId = responseBody.Country.Id;

        return TypedResults.CreatedAtRoute(responseBody,
            Endpoints.Countries.GetCountry,
            new RouteValueDictionary { { nameof(countryId), countryId } });
    }

    internal sealed record Command(string CountryCode, string CountryName, CountryType CountryType)
        : ICommand<CreateCountryResponse>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetFactoryFunction(command.CountryType)
                .Invoke(command.CountryCode, command.CountryName)
                .FailIf(country => dbContext.V0Countries.AsNoTracking()
                        .Any(existingCountry => existingCountry.CountryCode == country.CountryCode),
                    CountryCodeConflict(command.CountryCode))
                .ThenDo(country => dbContext.V0Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(country => new CreateCountryResponse(country.ToCountryDto()));

        private static Func<string, string, ErrorOr<Country>> GetFactoryFunction(CountryType countryType) =>
            countryType switch
            {
                CountryType.Real => Country.CreateReal,
                CountryType.Pseudo => Country.CreatePseudo,
                _ => throw new InvalidEnumArgumentException(nameof(countryType), (int)countryType, typeof(CountryType))
            };

        private static Error CountryCodeConflict(string countryCode) => Error.Conflict("Country code conflict",
            "A country already exists with the provided country code.",
            new Dictionary<string, object> { { nameof(countryCode), countryCode } });
    }
}
