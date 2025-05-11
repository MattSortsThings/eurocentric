using Eurocentric.Domain.Enums;

namespace Eurocentric.Infrastructure.FakeRepositories;

public sealed class FakeVoterPointsDataRepository
{
    public IEnumerable<FakeVoterPointsDatum> GetTelevoteData() =>
    [
        new(2023, ContestStage.SemiFinal1, "AT", "Austria", "BE", 12, 12),
        new(2023, ContestStage.SemiFinal1, "AT", "Austria", "CZ", 10, 12),
        new(2023, ContestStage.SemiFinal1, "BE", "Belgium", "AT", 12, 12),
        new(2023, ContestStage.SemiFinal1, "BE", "Belgium", "CZ", 10, 12),
        new(2023, ContestStage.SemiFinal1, "CZ", "Czechia", "AT", 12, 12),
        new(2023, ContestStage.SemiFinal1, "CZ", "Czechia", "BE", 10, 12),
        new(2023, ContestStage.SemiFinal1, "DE", "Germany", "AT", 12, 12),
        new(2023, ContestStage.SemiFinal1, "DE", "Germany", "BE", 10, 12),
        new(2023, ContestStage.SemiFinal1, "DE", "Germany", "CZ", 0, 12),
        new(2023, ContestStage.SemiFinal2, "ES", "Spain", "FI", 12, 12),
        new(2023, ContestStage.SemiFinal2, "ES", "Spain", "GB", 10, 12),
        new(2023, ContestStage.SemiFinal2, "ES", "Spain", "HR", 0, 12),
        new(2023, ContestStage.SemiFinal2, "FI", "Finland", "GB", 12, 12),
        new(2023, ContestStage.SemiFinal2, "FI", "Finland", "HR", 10, 12),
        new(2023, ContestStage.SemiFinal2, "GB", "United Kingdom", "FI", 12, 12),
        new(2023, ContestStage.SemiFinal2, "GB", "United Kingdom", "HR", 10, 12),
        new(2023, ContestStage.SemiFinal2, "HR", "Croatia", "FI", 10, 12),
        new(2023, ContestStage.SemiFinal2, "HR", "Croatia", "GB", 12, 12),
        new(2024, ContestStage.SemiFinal1, "AT", "Austria", "BE", 10, 12),
        new(2024, ContestStage.SemiFinal1, "AT", "Austria", "CZ", 12, 12),
        new(2024, ContestStage.SemiFinal1, "BE", "Belgium", "AT", 10, 12),
        new(2024, ContestStage.SemiFinal1, "BE", "Belgium", "CZ", 12, 12),
        new(2024, ContestStage.SemiFinal1, "CZ", "Czechia", "AT", 10, 12),
        new(2024, ContestStage.SemiFinal1, "CZ", "Czechia", "BE", 12, 12),
        new(2024, ContestStage.SemiFinal1, "DE", "Germany", "AT", 10, 12),
        new(2024, ContestStage.SemiFinal1, "DE", "Germany", "BE", 12, 12),
        new(2024, ContestStage.SemiFinal1, "DE", "Germany", "CZ", 0, 12),
        new(2024, ContestStage.SemiFinal2, "ES", "Spain", "FI", 12, 12),
        new(2024, ContestStage.SemiFinal2, "ES", "Spain", "GB", 10, 12),
        new(2024, ContestStage.SemiFinal2, "ES", "Spain", "HR", 0, 12),
        new(2024, ContestStage.SemiFinal2, "FI", "Finland", "GB", 12, 12),
        new(2024, ContestStage.SemiFinal2, "FI", "Finland", "HR", 10, 12),
        new(2024, ContestStage.SemiFinal2, "GB", "United Kingdom", "FI", 10, 12),
        new(2024, ContestStage.SemiFinal2, "GB", "United Kingdom", "HR", 12, 12),
        new(2024, ContestStage.SemiFinal2, "HR", "Croatia", "FI", 0, 12),
        new(2024, ContestStage.SemiFinal2, "HR", "Croatia", "GB", 0, 12)
    ];

    public IEnumerable<FakeVoterPointsDatum> GetJuryData() => [];
}
