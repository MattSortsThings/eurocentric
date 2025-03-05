using ErrorOr;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.DataAccess.InMemory;
using Eurocentric.Domain.Rules;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V1.Countries.CreateCountry;

internal sealed class CreateCountryHandler(InMemoryRepository repository, IUniqueCountryCodeRule uniqueCountryCodeRule)
    : CommandHandler<CreateCountryCommand, CreateCountryResult>
{
    public override async Task<ErrorOr<CreateCountryResult>> Handle(CreateCountryCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return command.CountryType.ToBuilder().Invoke()
            .WithCountryCode(command.CountryCode)
            .AndCountryName(command.CountryName)
            .Build(DateTimeOffset.UtcNow)
            .Then(uniqueCountryCodeRule.Validate)
            .ThenDo(country => repository.Countries.Add(country))
            .Then(country => new CreateCountryResult(country.ToModelCountry()));
    }
}
