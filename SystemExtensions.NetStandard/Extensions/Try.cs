using System.Collections.Generic;

namespace System.Extensions
{
    public static class Try
    {
        public static Try<TResult> Action<TResult>(Func<TResult> act)
        {
            return new Try<TResult>(act);
        }
    }
    public class Try<TResult>
    {
        private readonly Func<TResult> action;
        private readonly List<Func<Exception, TResult>> catchActions = new List<Func<Exception, TResult>>();
        internal Try(Func<TResult> action)
        {
            this.action = action;
        }
        public static Try<TResult> Action(Func<TResult> act)
        {
            return new Try<TResult>(act);
        }

        public Try<TResult> Catch<TException>(Func<TException, TResult> act) where TException : Exception
        {
            this.catchActions.Add((Func<Exception, TResult>)act);
            return this;
        }

        public TResult Run()
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                foreach (var catchAction in this.catchActions)
                {
                    return catchAction(ex);
                }
                throw;
            }
        }

        public TResult Finally(Action act)
        {
            if (act is null) throw new ArgumentNullException(nameof(act));

            try
            {
                return this.action();
            }
            catch (Exception ex)
            {
                foreach (var catchAction in this.catchActions)
                {
                    return catchAction(ex);
                }
                throw;
            }
            finally
            {
                act();
            }
        }
    }
}
