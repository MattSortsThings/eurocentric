using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.TestUtils;

public static class BroadcastIds
{
    public static BroadcastId GetOne() => BroadcastId.FromValue(Guid.Parse("127f4df7-4730-4013-a4c0-e05cb98c40c3"));

    public static (BroadcastId, BroadcastId) GetTwo() =>
        (BroadcastId.FromValue(Guid.Parse("42fc3d69-901e-4b0f-89eb-a5a727a0c169")),
            BroadcastId.FromValue(Guid.Parse("f2546df3-9b73-42ae-8ee0-aaf288f5f131")));

    public static (BroadcastId, BroadcastId, BroadcastId) GetThree() =>
        (BroadcastId.FromValue(Guid.Parse("9d2308e5-2e35-4fe4-9a8a-a818e24f875c")),
            BroadcastId.FromValue(Guid.Parse("e8cdbeba-6ae2-43d5-9244-8e6cb7b921b3")),
            BroadcastId.FromValue(Guid.Parse("0f6d24c2-3db7-4496-9018-a88e5708e5e5")));
}
