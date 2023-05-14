namespace System.Extensions;

public static class Optional
{
    public static Optional<T> None<T>()
    {
        return new Optional<T>.None();
    }

    public static Optional<T> FromValue<T>(T? value)
    {
        if (value == null)
        {
            return new Optional<T>.None();
        }

        return new Optional<T>.Some(value);
    }
}

public abstract class Optional<T> : IEquatable<Optional<T>>
{
    public Optional(T value)
    {
        this.Value = value;
    }

    private T Value { get; }

    public T? ExtractValue()
    {
        if (this is None)
        {
            return default;
        }
        else
        {
            return this.Value;
        }
    }
    public Optional<T> Do(Action<T>? onSome, Action? onNone)
    {
        if (onSome is null)
        {
            throw new ArgumentNullException(nameof(onSome));
        }

        if (onNone is null)
        {
            throw new ArgumentNullException(nameof(onNone));
        }

        if (this is Some)
        {
            onSome.Invoke(this.Value);
        }
        else
        {
            onNone.Invoke();
        }

        return this;
    }
    public Optional<T> DoAny(Action<T>? onSome = default, Action? onNone = default)
    {
        if (this is Some)
        {
            onSome?.Invoke(this.Value);
        }
        else
        {
            onNone?.Invoke();
        }

        return this;
    }
    public Optional<V> Switch<V>(Func<T, V>? onSome, Func<V>? onNone)
    {
        if (onSome is null)
        {
            throw new ArgumentNullException($"{nameof(onSome)}");
        }

        if (onNone is null)
        {
            throw new ArgumentNullException($"{nameof(onNone)}");
        }

        if (this is Some)
        {
            return Optional.FromValue(onSome.Invoke(this.Value));
        }
        else
        {
            return Optional.FromValue(onNone.Invoke());
        }
    }
    public Optional<V> SwitchAny<V>(Func<T, V>? onSome = null, Func<V>? onNone = null)
    {
        if (this is Some)
        {
            return onSome != null ? Optional.FromValue(onSome.Invoke(this.Value)) : Optional.FromValue(default(V));
        }
        else
        {
            return onNone != null ? Optional.FromValue(onNone.Invoke()) : Optional.FromValue(default(V));
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is Optional<T>)
        {
            return this.Equals(obj);
        }

        return base.Equals(obj);
    }

    public bool Equals(Optional<T>? other)
    {
        if (this is Some && other is Some)
        {
            return this.As<Some>() is Some some ? some.Equals(other.As<Some>()) : false;
        }

        return false;
    }

    public static implicit operator Optional<T>(T? value)
    {
        return Optional.FromValue(value);
    }

    public override int GetHashCode()
    {
        return this.Value == null ? base.GetHashCode() : this.Value.GetHashCode();
    }

    internal class Some : Optional<T>, IEquatable<Some>
    {
        public Some(T value) : base(value)
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is Some)
            {
                return this.Equals(obj.As<Some>());
            }

            return base.Equals(obj);
        }

        public bool Equals(Some? other)
        {
            if (other is None)
            {
                return false;
            }

            if (other is not null)
            {
                return this.Value is not null && this.Value.Equals(other.Value);
            }

            return false;
        }

        public static bool operator ==(Some? left, Some right)
        {
            return left?.Equals(right) == true;
        }

        public static bool operator !=(Some? left, Some right)
        {
            return left?.Equals(right) != true;
        }

        public override int GetHashCode()
        {
            return this.Value is object ? this.Value.GetHashCode() : this.As<object>() is object obj ? obj.GetHashCode() : 0;
        }
    }

    internal class None : Optional<T>
    {
        public None() : base(default!)
        {
        }
    }
}
