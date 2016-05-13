
namespace Atom.Scripting
{
    using System;
    using Microsoft.Scripting.Hosting;
    
    /// <summary>
    /// Reprents a simple scoped script.
    /// </summary>
    public class Script : IScript
    {
        /// <summary>
        /// Gets or sets the code to execute.
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this Script has an own local scope; -or- otherwise only uses the global scope.
        /// </summary>
        public bool Scoped 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this Script was compiled.
        /// </summary>
        public bool WasCompiled
        {
            get
            {
                return this.compiledCode != null;
            }
        }

        /// <summary>
        /// Compiles this Script under the given IScriptingEnvironment;
        /// allowing it to be executed.
        /// </summary>
        /// <param name="environment">
        /// The environment under which this script is compiled.
        /// </param>
        public void Compile( IScriptingEnvironment environment )
        {
            this.compiledCode = environment.CompileCode( this.Code ?? string.Empty );
            this.scope = environment.CreateScope( this.Scoped );
        }

        /// <summary>
        /// Executes this Script.
        /// </summary>
        public void Execute()
        {
            if( this.compiledCode != null )
            {
                this.compiledCode.Execute( this.scope );
            }
        }

        /// <summary>
        /// Executes this Script, returning the return value of the executed code.
        /// </summary>
        /// <returns>
        /// The value that was computed by the script.
        /// </returns>
        public dynamic ExecuteQuery()
        {
            if( this.compiledCode != null )
            {
                return this.compiledCode.Execute( this.scope );
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored in this Script.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to receive.
        /// </param>
        /// <returns>
        /// The value of the variable.
        /// </returns>
        public dynamic GetVariable( string name )
        {
            return this.scope.GetVariable( name );
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
        /// This Script for method chaining.
        /// </returns>
        public IScript SetVariable( string name, object value )
        {
            this.scope.SetVariable( name, value );
            return this;
        }

        /// <summary>
        /// Gets a value indicating whether this IScript contains a variable with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to query.
        /// </param>
        /// <returns>
        /// True if the variable exists; -or- otherwise false.
        /// </returns>
        public bool HasVariable( string name )
        {
            return this.scope.ContainsVariable( name );
        }

        private ScriptScope scope;
        private CompiledCode compiledCode;
    }
}
