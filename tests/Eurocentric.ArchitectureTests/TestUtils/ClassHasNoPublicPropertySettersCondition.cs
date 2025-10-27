using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassHasNoPublicPropertySettersCondition : ICondition<Class>
{
    public string Description => "have no public property setters";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            IEnumerable<MethodMember> filteredMethods = @class.GetMethodMembers().Where(IsPropertySetter);

            if (filteredMethods.Any(member => member.Visibility == Visibility.Public))
            {
                yield return new ConditionResult(@class, false, "has a public property setter");
            }
            else
            {
                yield return new ConditionResult(@class, true);
            }
        }
    }

    public bool CheckEmpty() => true;

    private static bool IsPropertySetter(MethodMember member) => member.Name.StartsWith("set_");
}
