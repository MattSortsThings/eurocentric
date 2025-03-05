using ErrorOr;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Rules;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V1.Countries.CreateCountry;

internal sealed class CreateCountryHandler(
    AppDbContext dbContext,
    TimeProvider timeProvider,
    IUniqueCountryCodeRule uniqueCountryCodeRule) : CommandHandler<CreateCountryCommand, CreateCountryResult>
{
    public override async Task<ErrorOr<CreateCountryResult>> Handle(CreateCountryCommand command,
        CancellationToken cancellationToken) =>
        await command.CountryType.ToBuilder().Invoke()
            .WithCountryCode(command.CountryCode)
            .AndCountryName(command.CountryName)
            .Build(timeProvider.GetUtcNow())
            .Then(uniqueCountryCodeRule.Validate)
            .ThenDo(country => dbContext.Countries.Add(country))
            .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
            .Then(country => new CreateCountryResult(country.ToModelCountry()));
}
