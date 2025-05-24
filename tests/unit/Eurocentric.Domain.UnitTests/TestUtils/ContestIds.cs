using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.TestUtils;

public static class ContestIds
{
    public static ContestId GetOne() => ContestId.FromValue(Guid.Parse("49a6718b-e2ee-48fd-9282-498de9bec930"));

    public static (ContestId, ContestId) GetTwo() =>
        (ContestId.FromValue(Guid.Parse("000b3c66-fe78-415f-810e-b9f20c1ac0cb")),
            ContestId.FromValue(Guid.Parse("8fcdecb9-72db-4ec5-ab2a-984c6b3eb2cf")));
}
