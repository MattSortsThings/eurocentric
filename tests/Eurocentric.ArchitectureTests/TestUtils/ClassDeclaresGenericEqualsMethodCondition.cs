using System.Runtime.CompilerServices;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassDeclaresGenericEqualsMethodCondition : ICondition<Class>
{
    public string Description => "declare generic Equals method";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            IMember expectedMethod = @class.Members.Single(member => member.Name.Equals($"Equals({@class.FullName})"));

            if (IsCompilerGenerated(expectedMethod))
            {
                yield return new ConditionResult(@class, false, "does not declare generic Equals method");
            }

            yield return new ConditionResult(@class, true);
        }
    }

    public bool CheckEmpty() => true;

    private static bool IsCompilerGenerated(IMember member) =>
        member.AttributeInstances.Any(attribute => attribute.MatchesType(typeof(CompilerGeneratedAttribute)));
}
