using ErrorOr;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Rules.External;
using Eurocentric.Domain.Rules.External.DbCheckers;
using Eurocentric.Shared.AppPipeline;
using Country = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.AdminApi.V1.Countries.CreateCountry;

internal sealed class CreateCountryHandler(
    AppDbContext dbContext,
    TimeProvider timeProvider,
    ICountryDbChecker countryDbChecker) : CommandHandler<CreateCountryCommand, CreateCountryResult>
{
    public override async Task<ErrorOr<CreateCountryResult>> Handle(CreateCountryCommand command,
        CancellationToken cancellationToken) => await command.CountryType.ToBuilder()
        .WithCountryCode(command.CountryCode)
        .AndCountryName(command.CountryName)
        .Build(timeProvider.GetUtcNow())
        .EnforceExternalRules(countryDbChecker)
        .ThenDo(country => dbContext.Countries.Add(country))
        .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
        .Then(MapToCreateCountryResult);

    private static CreateCountryResult MapToCreateCountryResult(Country country) => new(country.ToModelCountry());
}
