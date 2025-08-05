using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;

internal static class CreateCountryFeature
{
    internal static async Task<IResult> ExecuteAsync(CreateCountryRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(requestBody.ToCommand(), MapToCreatedAtRoute, cancellationToken);

    private static CountryBuilder Apply(this CountryBuilder builder, Command command)
    {
        var (countryCode, countryName) = command;

        return builder.WithCountryCode(CountryCode.FromValue(countryCode))
            .WithCountryName(CountryName.FromValue(countryName));
    }

    private static Command ToCommand(this CreateCountryRequest requestBody) =>
        new(requestBody.CountryCode, requestBody.CountryName);

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointNames.Routes.Countries.GetCountry,
            new RouteValueDictionary { { "countryId", response.Country.Id } });

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<CreateCountryResponse>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext, ICountryIdProvider idProvider)
        : ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await Country.Create()
                .Apply(command)
                .Build(idProvider.CreateSingle)
                .FailOnCountryCodeConflict(dbContext.Countries.AsNoTracking().AsSplitQuery())
                .ThenDo(country => dbContext.Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(country => country.ToCountryDto())
                .Then(country => new CreateCountryResponse(country));
    }
}
