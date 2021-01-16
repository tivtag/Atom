
namespace Atom.Scripting.ScriptingEnvironmentSpecs
{
    using Machine.Specifications;

    public class when_created
    {
        static ScriptingEnvironment env;

        Establish context = () => { };

        Because of = () =>
            env = new ScriptingEnvironment();

        It should_not_contain_a_global_variable = () =>
            env.HasGlobal( "Any" ).ShouldBeFalse();
    }
}
