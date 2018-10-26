
namespace Atom.Diagnostics.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Contract
    {
        public static void Requires(bool condition, string message = null)
        {
            Requires<ArgumentNullException>(condition, message);
        }

        public static void Requires<TException>(bool condition, string message = null) where TException : Exception, new()
        {
            if (!condition)
            {
                //https://stackoverflow.com/questions/41397/asking-a-generic-method-to-throw-specific-exception-type-on-fail/41450#41450
                var e = default(TException);
                try
                {
                    message = message ?? "Unexpected Condition"; //TODO consider to pass condition as lambda expression
                    e = Activator.CreateInstance(typeof(TException), message) as TException;
                }
                catch (MissingMethodException)
                {
                    e = new TException();
                }
                throw e;
            }
        }

        public static bool ForAll<T>(IEnumerable<T> elementsToAdd, Func<T, bool> predicate)
        {
            return elementsToAdd.All( predicate );
        }

        public static void Invariant(bool invariant)
        {
            Requires<Exception>( invariant );
        }
    }
}
