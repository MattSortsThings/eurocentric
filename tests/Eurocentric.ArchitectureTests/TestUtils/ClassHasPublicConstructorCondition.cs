using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassHasPublicConstructorCondition : ICondition<Class>
{
    public string Description => "have a public constructor";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            if (@class.GetConstructors().All(member => member.Visibility != Visibility.Public))
            {
                yield return new ConditionResult(@class, false, "has no public constructor");
            }
            else
            {
                yield return new ConditionResult(@class, true);
            }
        }
    }

    public bool CheckEmpty() => true;
}
