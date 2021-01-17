
namespace Atom.Diagnostics.Contracts
{
    using System;

    /// <summary>
    /// Marks a method as pure.
    /// </summary>
    /// <remarks>
    /// This class was added since we had to remove the now unsupported Microsoft Code Contracts.
    /// </remarks>
    public sealed class PureAttribute : Attribute
    {
    }
}
