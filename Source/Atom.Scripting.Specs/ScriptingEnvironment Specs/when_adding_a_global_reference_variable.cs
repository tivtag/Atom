
namespace Atom.Scripting.ScriptingEnvironmentSpecs
{
    using Machine.Specifications;

    public class when_adding_a_global_reference_variable
    {
        class TestClass
        {
            public string Name { get; set; }
        }

        const string VariableName = "person";

        static ScriptingEnvironment env;
        static TestClass var;
        static IScriptingEnvironment returnedEnv;

        Establish context = () => {
            env = new ScriptingEnvironment();
            var = new TestClass() {
                Name = "Mew!"
            };
        };

        Because of = () =>
            returnedEnv = env.SetGlobal( VariableName, var );

        It should_contain_the_global_variable = () =>
            env.HasGlobal( VariableName ).ShouldBeTrue();

        It should_not_contain_another_global_variable = () =>
            env.HasGlobal( VariableName + "-no" ).ShouldBeFalse();

        It should_fluently_return_the_original_scripting_environment = () =>
            returnedEnv.ShouldEqual( env );
    }
}
