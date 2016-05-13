
namespace Atom.Scripting
{
    using System.IO;
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// Implements an <see cref="IScriptingEnvironment"/> based on IronRuby.
    /// </summary>
    public class ScriptingEnvironment : IScriptingEnvironment
    {
        /// <summary>
        /// Initializes a new instance of the ScriptingEnvironment class.
        /// </summary>
        public ScriptingEnvironment()
        {
            this.engine = IronRuby.Ruby.CreateEngine();
            this.globalScope = this.engine.CreateScope();
        }

        /// <summary>
        /// Sets the value of the variable <paramref name="name"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to set.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        public IScriptingEnvironment SetGlobal( string name, object value )
        {
            this.globalScope.SetVariable( name, value );
            return this;
        }

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored in this IScriptingEnvironment.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to receive.
        /// </param>
        /// <returns>
        /// The value of the variable.
        /// </returns>
        public dynamic GetGlobal( string name )
        {
            return this.globalScope.GetVariable( name );
        }

        /// <summary>
        /// Gets a value indicating whether this IScriptingEnvironment contains a variable with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to query.
        /// </param>
        /// <returns>
        /// True if the variable exists; -or- otherwise false.
        /// </returns>
        public bool HasGlobal( string name )
        {
            return this.globalScope.ContainsVariable( name );
        }

        /// <summary>
        /// Compiles and executes the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        public IScriptingEnvironment Execute( string code )
        {
            this.engine.Execute( code, this.globalScope );
            return this;
        }

        /// <summary>
        /// Compiles and executes the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <param name="scope">
        /// The variable scope under which the script is executed.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        public IScriptingEnvironment Execute( string code, ScriptScope scope )
        {
            this.engine.Execute( code, scope );
            return this;
        }

        /// <summary>
        /// Compiles and executes the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="codeStream">
        /// The code to execute.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        public IScriptingEnvironment Execute( Stream codeStream )
        {
            var sr = new StreamReader( codeStream );
            string code = sr.ReadToEnd();
            this.Execute( code );
            return this;
        }

        /// <summary>
        /// Compiles the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="code">
        /// The code to compile.
        /// </param>
        /// <returns>
        /// The compiled code.
        /// </returns>
        public CompiledCode CompileCode( string code )
        {
            return this.engine.CreateScriptSourceFromString( code )
                .Compile();
        }

        /// <summary>
        /// Gets or creates a scope under which scripts can run.
        /// </summary>
        /// <param name="local">
        /// If true a new local scope, that is based on the global scope of this IScriptingEnvironment, is created.
        /// </param>
        /// <returns>
        /// A new local scope; -or- the global scope.
        /// </returns>
        public ScriptScope CreateScope( bool local )
        {
            return local ? this.engine.CreateScope( this.globalScope ) : this.globalScope;
        }
        
        /// <summary>
        /// Creates and pre-compiles a new Script that will execute the given <paramref name="code"/>.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="scoped">
        /// If true a new local scope, that is based on the global scope of this IScriptingEnvironment, is created;
        /// -or- otherwise if false, the global scope is used.
        /// </param>
        /// <returns>
        /// The newly created Script.
        /// </returns>
        public IScript CreateScript( string code, bool scoped = false )
        {
            var script = new Script() {
                Code = code,
                Scoped = scoped
            };

            script.Compile( this );
            return script;
        }

        private readonly ScriptScope globalScope;
        private readonly ScriptEngine engine;
    }
}
