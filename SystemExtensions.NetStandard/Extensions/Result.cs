namespace System.Extensions
{
    public class Result<TSuccess, TFailure>
    {
        private readonly object value;

        public Result(TSuccess successValue)
        {
            value = successValue;
        }
        public Result(TFailure failureValue)
        {
            value = failureValue;
        }

        public bool TryExtractSuccess(out TSuccess successValue)
        {

            if (value is TSuccess success)
            {
                successValue = success;
                return true;
            }
            else
            {
                successValue = default;
                return false;
            }
        }
        public bool TryExtractFailure(out TFailure failureValue)
        {

            if (value is TFailure failure)
            {
                failureValue = failure;
                return true;
            }
            else
            {
                failureValue = default;
                return false;
            }
        }
        public Result<TSuccess, TFailure> Do(Action onSuccess, Action onFailure)
        {
            if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

            if (onFailure is null) throw new ArgumentNullException(nameof(onFailure));

            if (value is TSuccess)
            {
                onSuccess.Invoke();
            }
            else if (value is TFailure)
            {
                onFailure.Invoke();
            }
            return this;
        }
        public Result<TSuccess, TFailure> DoAny(Action onSuccess = null, Action onFailure = null)
        {
            if (value is TSuccess)
            {
                onSuccess?.Invoke();
            }
            else if (value is TFailure)
            {
                onFailure?.Invoke();
            }
            return this;
        }
        public Result<TSuccess, TFailure> Do(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
        {
            if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

            if (onFailure is null) throw new ArgumentNullException(nameof(onFailure));

            if (value is TSuccess success)
            {
                onSuccess.Invoke(success);
            }
            else if (value is TFailure failure)
            {
                onFailure.Invoke(failure);
            }
            return this;
        }
        public Result<TSuccess, TFailure> DoAny(Action<TSuccess> onSuccess = null, Action<TFailure> onFailure = null)
        {
            if (value is TSuccess success)
            {
                onSuccess?.Invoke(success);
            }
            else if (value is TFailure failure)
            {
                onFailure?.Invoke(failure);
            }
            return this;
        }
        public T Switch<T>(Func<TSuccess, T> onSuccess, Func<TFailure, T> onFailure)
        {
            if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

            if (onFailure is null) throw new ArgumentNullException(nameof(onFailure));

            if (value is TSuccess success)
            {
                return onSuccess.Invoke(success);
            }
            else if (value is TFailure failure)
            {
                return onFailure.Invoke(failure);
            }
            throw new InvalidOperationException($"{nameof(value)} must be of type {typeof(TSuccess)} or {typeof(TFailure)} in order to switch to {typeof(T)}");
        }
        public T SwitchAny<T>(Func<TSuccess, T> onSuccess = null, Func<TFailure, T> onFailure = null)
        {
            if (value is TSuccess success)
            {
                return onSuccess is null ? default : onSuccess.Invoke(success);
            }
            else if (value is TFailure failure)
            {
                return onFailure is null ? default : onFailure.Invoke(failure);
            }
            throw new InvalidOperationException($"{nameof(value)} must be of type {typeof(TSuccess)} or {typeof(TFailure)} in order to switch to {typeof(T)}");
        }
        public Result<V, K> Switch<V, K>(Func<TSuccess, V> onSuccess, Func<TFailure, K> onFailure)
        {
            if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

            if (onFailure is null) throw new ArgumentNullException(nameof(onFailure));

            if (value is TSuccess success)
            {
                return Result<V, K>.Success(onSuccess.Invoke(success));
            }
            else if (value is TFailure failure)
            {
                return Result<V, K>.Failure(onFailure.Invoke(failure));
            }
            throw new InvalidOperationException($"{nameof(value)} must be of type {typeof(TSuccess)} or {typeof(TFailure)} in order to switch to Result of type {typeof(V)} or {typeof(K)}");
        }
        public Result<V, K> SwitchAny<V, K>(Func<TSuccess, V> onSuccess, Func<TFailure, K> onFailure)
        {
            if (value is TSuccess success)
            {
                return Result<V, K>.Success(onSuccess is null ? default : onSuccess.Invoke(success));
            }
            else if (value is TFailure failure)
            {
                return Result<V, K>.Failure(onFailure is null ? default : onFailure.Invoke(failure));
            }
            throw new InvalidOperationException($"{nameof(value)} must be of type {typeof(TSuccess)} or {typeof(TFailure)} in order to switch to Result of type {typeof(V)} or {typeof(K)}");
        }
        public Optional<TSuccess> ToOptional()
        {
            if (value is TSuccess)
            {
                return Optional.FromValue(value.Cast<TSuccess>());
            }

            return Optional.None<TSuccess>();
        }

        public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
        {
            return Success(success);
        }

        public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
        {
            return Failure(failure);
        }

        public static Result<TSuccess, TFailure> Success(TSuccess value)
        {
            return new Result<TSuccess, TFailure>(value);
        }
        public static Result<TSuccess, TFailure> Failure(TFailure value)
        {
            return new Result<TSuccess, TFailure>(value);
        }
    }
}
