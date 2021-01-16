
namespace Atom.Scripting.ScriptingEnvironmentSpecs
{
    using Machine.Specifications;

    public class when_manipulating_a_global_variable_in_a_script
    {
        class TestClass
        {
            public string Name { get; set; }
        }

        const string VariableName = "person";

        static ScriptingEnvironment env;
        static IScriptingEnvironment returnedEnv;
        static TestClass var;

        Establish context = () => {
            env = new ScriptingEnvironment();
            var = new TestClass() {
                Name = "Meow"
            };

            env.SetGlobal( VariableName, var );
        };

        Because of = () => {
            string ruby = "person.Name = 'Woof'";
            returnedEnv = env.Execute( ruby );
        };

        It should_modify_the_outside_variable = () =>
            var.Name.ShouldEqual( "Woof" );
        
        It should_fluently_return_the_original_scripting_environment = () =>
            returnedEnv.ShouldEqual( env );
    }
}
