using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

public interface IOptionalVotingMethodFiltering
{
    VotingMethodFilter? VotingMethod { get; }
}
