
namespace Atom.Scripting
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a container of multiple executable <seealso cref="IScript"/>s.
    /// </summary>
    public interface IScriptContainer : ICollection<IScript>
    {
        /// <summary>
        /// Executes all <seealso cref="IScript"/> in this container.
        /// </summary>
        void ExecuteAll();
    }
}
