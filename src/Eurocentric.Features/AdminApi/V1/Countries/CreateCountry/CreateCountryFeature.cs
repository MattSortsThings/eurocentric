using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;

internal static class CreateCountryFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(request.ToCommand(), MapToCreatedAtRoute, cancellationToken);

    private static Command ToCommand(this CreateCountryRequest request) => new(request.CountryCode, request.CountryName);

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse response)
    {
        Guid countryId = response.Country.Id;

        return TypedResults.CreatedAtRoute(response,
            Endpoints.Countries.GetCountry,
            new Dictionary<string, object> { { nameof(countryId), countryId } });
    }

    private static CountryBuilder Apply(this CountryBuilder builder, Command command)
    {
        (string countryCode, string countryName) = command;

        return builder.WithCountryCode(CountryCode.FromValue(countryCode))
            .WithCountryName(CountryName.FromValue(countryName));
    }

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<CreateCountryResponse>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext, ICountryIdGenerator idGenerator) :
        ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await CountryAggregate.Create()
                .Apply(command)
                .Build(idGenerator.CreateSingle)
                .FailOnCountryCodeConflict(dbContext.Countries.AsNoTracking())
                .ThenDo(country => dbContext.Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(country => country.ToCountryDto())
                .Then(countryDto => new CreateCountryResponse(countryDto));
    }
}
