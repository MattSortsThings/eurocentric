using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public class ClassHasPrivateProtectedParameterlessConstructorCondition : ICondition<Class>
{
    public string Description => "have private protected parameterless constructor";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            if (
                @class.Constructors.Any(member =>
                    member.Visibility == Visibility.PrivateProtected && !member.Parameters.Any()
                )
            )
            {
                yield return new ConditionResult(@class, true);
            }
            else
            {
                yield return new ConditionResult(@class, false, "has no private protected parameterless constructor");
            }
        }
    }

    public bool CheckEmpty() => true;
}
