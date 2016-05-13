
namespace Atom.Scripting
{
    /// <summary>
    /// Represents a dynamic script that can be executed while the calling program is running.
    /// Example Script Language: IronRuby
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Executes this IScript.
        /// </summary>
        void Execute();

        /// <summary>
        /// Executes this IScript, returning the return value of the executed code.
        /// </summary>
        /// <returns>
        /// The value that was computed by the script.
        /// </returns>
        dynamic ExecuteQuery();

        /// <summary>
        /// Compiles this IScript under the given IScriptingEnvironment;
        /// allowing it to be executed.
        /// </summary>
        /// <param name="environment">
        /// The environment under which this script is compiled.
        /// </param>
        void Compile( IScriptingEnvironment environment );

        /// <summary>
        /// Gets the value of the variable <paramref name="name"/> stored in this IScript.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to receive.
        /// </param>
        /// <returns>
        /// The value of the variable.
        /// </returns>
        dynamic GetVariable( string name );

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
        IScript SetVariable( string name, object value );

        /// <summary>
        /// Gets a value indicating whether this IScript contains a variable with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the variable to query.
        /// </param>
        /// <returns>
        /// True if the variable exists; -or- otherwise false.
        /// </returns>
        bool HasVariable( string name );
    }
}
