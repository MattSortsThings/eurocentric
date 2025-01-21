using ErrorOr;
using Eurocentric.Domain.Entities.Contests;
using MediatR;

namespace Eurocentric.AdminApi.V1.Contests.CreateContest;

internal sealed class CreateContestCommandHandler : IRequestHandler<CreateContestCommand, ErrorOr<Contest>>
{
    public async Task<ErrorOr<Contest>> Handle(CreateContestCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var (year, city, rules) = command;

        return rules == VotingRules.Undefined
            ? Error.Failure("Contests.IllegalVotingRules", "Cannot create a contest with Undefined voting rules.")
            : new Contest(Guid.CreateVersion7(), year, city, rules);
    }
}
