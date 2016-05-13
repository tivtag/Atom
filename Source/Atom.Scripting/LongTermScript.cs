
namespace Atom.Scripting
{
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// Represents a script that is executing for a longer time duration or multiple times.
    /// </summary>
    public class LongTermScript : IScript
    {
        /// <summary>
        /// Gets or sets the code that is executed on the first run of the script.
        /// </summary>
        public string InitCode 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the code that is executed every time when the script is executed.
        /// </summary>
        public string ExecCode 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Compiles this IScript under the given IScriptingEnvironment;
        /// allowing it to be executed.
        /// </summary>
        /// <param name="environment">
        /// The environment under which this script is compiled.
        /// </param>
        public void Compile( IScriptingEnvironment environment )
        {
            this.code = environment.CompileCode( this.ExecCode ?? string.Empty );
            this.scope = environment.CreateScope( local: true );
            this.environment = environment;
        }

        /// <summary>
        /// Executes this IScript.
        /// </summary>
        public void Execute()
        {
            if( !this.hasRun )
            {
                this.OnFirstRun();
                this.hasRun = true;
            }

             this.code.Execute( this.scope );
        }

        /// <summary>
        /// Executes this IScript, returning the return value of the executed code.
        /// </summary>
        /// <returns>
        /// The value that was computed by the script.
        /// </returns>
        public dynamic ExecuteQuery()
        {
            if( !this.hasRun )
            {
                this.OnFirstRun();
                this.hasRun = true;
            }

            return this.code.Execute( this.scope );
        }

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored in this IScript.
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
        /// This IScript for method chaining.
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

        private void OnFirstRun()
        {
            if( this.InitCode != null )
            {
                this.environment.Execute( this.InitCode, this.scope );
            }
        }

        private bool hasRun;
        private CompiledCode code;
        private ScriptScope scope;
        private IScriptingEnvironment environment;
    }
}
