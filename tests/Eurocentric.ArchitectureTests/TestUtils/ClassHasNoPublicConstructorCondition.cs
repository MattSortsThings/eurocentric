using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassHasNoPublicConstructorCondition : ICondition<Class>
{
    public string Description => "have no public constructor";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            if (@class.GetConstructors().Any(member => member.Visibility == Visibility.Public))
            {
                yield return new ConditionResult(@class, false, "has a public constructor");
            }
            else
            {
                yield return new ConditionResult(@class, true);
            }
        }
    }

    public bool CheckEmpty() => true;
}
