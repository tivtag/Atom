
namespace Atom.Scripting.ScriptingEnvironmentSpecs
{
    using Machine.Specifications;
    using Microsoft.Scripting.Hosting;

    public class when_manipulating_a_global_variable_in_a_compiled_script
    {
        class TestClass
        {
            public int Counter { get; set; }
        }

        const string VariableName = "person";

        static ScriptingEnvironment env;
        static ScriptingEnvironment returnedEnv;
        static TestClass var;
        static CompiledCode script;

        Establish context = () =>
        {
            env = new ScriptingEnvironment();
            var = new TestClass() { };
            
            string ruby = "person.Counter = person.Counter + 1";
            script = env.CompileScript( ruby );

            env.AddGlobalVariable( VariableName, var );
        };

        Because of = () =>
            returnedEnv = env.ExecuteScript( script );

        It should_modify_the_outside_variable = () =>
            var.Counter.ShouldEqual( 1 );

        It should_fluently_return_the_original_scripting_environment = () =>
            returnedEnv.ShouldEqual( env );
    }
}
