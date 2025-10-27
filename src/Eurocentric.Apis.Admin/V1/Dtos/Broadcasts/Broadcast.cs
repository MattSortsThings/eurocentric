using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;

/// <summary>
///     Represents a broadcast.
/// </summary>
public sealed record Broadcast : ISchemaExampleProvider<Broadcast>
{
    /// <summary>
    ///     The broadcast's ID.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The date on which the broadcast is televised.
    /// </summary>
    public DateOnly BroadcastDate { get; init; }

    /// <summary>
    ///     The ID of the broadcast's parent contest.
    /// </summary>
    public Guid ParentContestId { get; init; }

    /// <summary>
    ///     The broadcast's contest stage in its parent contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     A boolean value indicating whether the broadcast has been completed.
    /// </summary>
    public bool Completed { get; init; }

    /// <summary>
    ///     An array of all the broadcast's competitors.
    /// </summary>
    public Competitor[] Competitors { get; init; } = [];

    /// <summary>
    ///     An array of all the broadcast's juries.
    /// </summary>
    public Jury[] Juries { get; init; } = [];

    /// <summary>
    ///     An array of all the broadcast's televotes.
    /// </summary>
    public Televote[] Televotes { get; init; } = [];

    public static Broadcast CreateExample()
    {
        return new Broadcast
        {
            Id = V1ExampleValues.BroadcastId,
            BroadcastDate = V1ExampleValues.BroadcastDate,
            ParentContestId = V1ExampleValues.ContestId,
            ContestStage = ContestStage.GrandFinal,
            Completed = false,
            Competitors = [Competitor.CreateExample()],
            Juries = [Jury.CreateExample()],
            Televotes = [Televote.CreateExample()],
        };
    }
}
