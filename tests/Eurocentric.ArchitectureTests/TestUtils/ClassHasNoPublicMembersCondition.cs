using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassHasNoPublicMembersCondition : ICondition<Class>
{
    public string Description => "have no public members";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            if (@class.Members.Any(member => member.Visibility == Visibility.Public))
            {
                yield return new ConditionResult(@class, false, "has a public member");
            }
            else
            {
                yield return new ConditionResult(@class, true);
            }
        }
    }

    public bool CheckEmpty() => true;
}
