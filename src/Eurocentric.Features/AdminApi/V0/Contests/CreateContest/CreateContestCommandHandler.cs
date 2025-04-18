using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Contests.Models;
using Eurocentric.Features.Shared.Messaging;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

internal sealed class CreateContestCommandHandler : ICommandHandler<CreateContestCommand, CreateContestResponse>
{
    public Task<ErrorOr<CreateContestResponse>> OnHandle(CreateContestCommand command, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(command.Format))
        {
            throw new InvalidEnumArgumentException(nameof(command.Format), (int)command.Format, typeof(ContestFormat));
        }

        Contest contest = new(Guid.NewGuid(), command.ContestYear, command.CityName, command.Format, false);

        return Task.FromResult(ErrorOrFactory.From(new CreateContestResponse(contest)));
    }
}
