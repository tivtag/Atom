
namespace Atom.Scripting
{
    using System.IO;
    using Microsoft.Scripting.Hosting;

    /// <summary>
    /// Represents the root environment and scope under which scripts are compiled.
    /// </summary>
    public interface IScriptingEnvironment
    {  
        /// <summary>
        /// Compiles and executes the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="codeStream">
        /// The code to execute.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        IScriptingEnvironment Execute( Stream codeStream );

        /// <summary>
        /// Compiles and executes the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="code">
        /// The code to execute.
        /// </param>
        /// <returns>
        /// This IScriptingEnvironment for method chaining.
        /// </returns>
        IScriptingEnvironment Execute( string code );

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
        IScriptingEnvironment Execute( string code, ScriptScope scope );

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored in this IScriptingEnvironment.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to receive.
        /// </param>
        /// <returns>
        /// The value of the variable.
        /// </returns>
        dynamic GetGlobal( string name );
               
        /// <summary>
        /// Gets a value indicating whether this IScriptingEnvironment contains a variable with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to query.
        /// </param>
        /// <returns>
        /// True if the variable exists; -or- otherwise false.
        /// </returns>
        bool HasGlobal( string name );

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
        IScriptingEnvironment SetGlobal( string name, object value );

        /// <summary>
        /// Compiles the given code under this IScriptingEnvironment.
        /// </summary>
        /// <param name="code">
        /// The code to compile.
        /// </param>
        /// <returns>
        /// The compiled code.
        /// </returns>
        CompiledCode CompileCode( string code );

        /// <summary>
        /// Gets or creates a scope under which scripts can run.
        /// </summary>
        /// <param name="local">
        /// If true a new local scope, that is based on the global scope of this IScriptingEnvironment, is created.
        /// </param>
        /// <returns>
        /// A new local scope; -or- the global scope.
        /// </returns>
        ScriptScope CreateScope( bool local );
    }
}
