using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using CompetitorEntity = Eurocentric.Domain.Aggregates.Broadcasts.Competitor;
using JuryEntity = Eurocentric.Domain.Aggregates.Broadcasts.Jury;
using TelevoteEntity = Eurocentric.Domain.Aggregates.Broadcasts.Televote;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using ApiContestStage = Eurocentric.Features.AdminApi.V1.Common.Enums.ContestStage;
using BroadcastDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using CompetitorDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Competitor;
using VoterDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Voter;
using AwardDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Award;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Broadcasts;

public static class ApiDriverExtensions
{
    public static async Task<BroadcastDto> CreateSingleBroadcastAsync(this IApiDriver driver,
        Guid[] competingCountryIds = null!,
        Guid contestId = default,
        ApiContestStage contestStage = default,
        DateOnly broadcastDate = default)
    {
        CountryId[] countryIds = competingCountryIds.Select(CountryId.FromValue).ToArray();
        List<CompetitorEntity> competitors = countryIds.Select((id, index) => new CompetitorEntity(id, index + 1)).ToList();
        List<JuryEntity> juries = countryIds.Select(id => new JuryEntity(id)).ToList();
        List<TelevoteEntity> televotes = countryIds.Select(id => new TelevoteEntity(id)).ToList();
        ContestId parentContestId = ContestId.FromValue(contestId);
        DomainContestStage stage = Enum.Parse<DomainContestStage>(contestStage.ToString());
        BroadcastDate date = BroadcastDate.FromValue(broadcastDate).Value;
        BroadcastId id = BroadcastId.FromValue(Guid.NewGuid());

        BroadcastAggregate agg = new(id, date, parentContestId, stage, competitors, juries, televotes);

        await driver.BackDoor.ExecuteScopedAsync(PersistAsync(agg));

        return agg.ToBroadcastDto();
    }

    public static async Task DeleteSingleBroadcastAsync(this IApiDriver driver, Guid broadcastId)
    {
        BroadcastId broadcastIdToDelete = BroadcastId.FromValue(broadcastId);
        await driver.BackDoor.ExecuteScopedAsync(DeleteAsync(broadcastIdToDelete));
    }

    private static Func<IServiceProvider, Task> PersistAsync(BroadcastAggregate agg)
    {
        BroadcastAggregate broadcastToPersist = agg;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Broadcasts.Add(broadcastToPersist);
            await dbContext.SaveChangesAsync();
        };
    }

    private static Func<IServiceProvider, Task> DeleteAsync(BroadcastId broadcastId)
    {
        BroadcastId broadcastIdToDelete = broadcastId;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Broadcasts.Where(broadcast => broadcast.Id == broadcastIdToDelete).ExecuteDeleteAsync();
        };
    }

    private static BroadcastDto ToBroadcastDto(this BroadcastAggregate broadcast) => new()
    {
        Id = broadcast.Id.Value,
        BroadcastDate = broadcast.BroadcastDate.Value,
        ParentContestId = broadcast.ParentContestId.Value,
        ContestStage = (ApiContestStage)(int)broadcast.ContestStage,
        Completed = broadcast.Completed,
        Competitors = broadcast.Competitors.Select(c => new CompetitorDto
        {
            CompetingCountryId = c.CompetingCountryId.Value,
            RunningOrderPosition = c.RunningOrderPosition,
            FinishingPosition = c.FinishingPosition,
            JuryAwards =
                c.JuryAwards.Select(a =>
                    new AwardDto { VotingCountryId = a.VotingCountryId.Value, PointsValue = (int)a.PointsValue }).ToArray(),
            TelevoteAwards =
                c.TelevoteAwards.Select(a =>
                    new AwardDto { VotingCountryId = a.VotingCountryId.Value, PointsValue = (int)a.PointsValue }).ToArray()
        }).ToArray(),
        Juries = broadcast.Juries.Select(v => new VoterDto
        {
            VotingCountryId = v.VotingCountryId.Value, PointsAwarded = v.PointsAwarded
        }).ToArray(),
        Televotes = broadcast.Televotes.Select(v => new VoterDto
        {
            VotingCountryId = v.VotingCountryId.Value, PointsAwarded = v.PointsAwarded
        }).ToArray()
    };
}
