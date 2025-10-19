using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Queries.Rankings.Common;

public interface IOptionalVotingMethodFiltering
{
    VotingMethodFilter? VotingMethod { get; }
}
