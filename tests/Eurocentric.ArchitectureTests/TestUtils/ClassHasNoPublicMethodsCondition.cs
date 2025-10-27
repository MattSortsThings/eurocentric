using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace Eurocentric.ArchitectureTests.TestUtils;

public sealed class ClassHasNoPublicMethodsCondition : ICondition<Class>
{
    public string Description => "have no public methods";

    public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
    {
        foreach (Class @class in objects)
        {
            IEnumerable<MethodMember> filteredMethods = @class
                .GetMethodMembers()
                .Where(IsNotConstructor)
                .Where(IsNotPropertyGetter)
                .Where(IsNotPropertySetter);

            if (filteredMethods.Any(member => member.Visibility == Visibility.Public))
            {
                yield return new ConditionResult(@class, false, "has a public method");
            }
            else
            {
                yield return new ConditionResult(@class, true);
            }
        }
    }

    public bool CheckEmpty() => true;

    private static bool IsNotConstructor(MethodMember member) => !member.Name.StartsWith(".ctor");

    private static bool IsNotPropertyGetter(MethodMember member) => !member.Name.StartsWith("get_");

    private static bool IsNotPropertySetter(MethodMember member) => !member.Name.StartsWith("set_");
}
