using System.Linq;
using System.Threading;

namespace System.Collections.Generic;
public sealed class CircularBuffer<T> : IList<T>
{
    private readonly T[] buffer;
    private int head;
    private int tail;

    public int Count { get; private set; }
    public bool IsReadOnly { get; } = false;

    public T this[int index]
    {
        get
        {
            if (index > this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.buffer[(this.head + index) % this.buffer.Length];
        }
        set
        {
            if (index > this.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.buffer[(this.head + index) % this.buffer.Length] = value;
        }
    }

    public CircularBuffer(int capacity)
    {
        if (capacity < 1) throw new ArgumentException($"{nameof(capacity)} cannot be smaller than 1");

        this.buffer = new T[capacity];
        this.head = 0;
        this.tail = 0;
    }

    public CircularBuffer(T[] buffer, int head, int tail)
    {
        if (buffer.Length == 0) throw new ArgumentException($"{nameof(buffer)} cannot be of length 0");

        this.buffer = buffer;
        this.head = head;
        this.tail = tail;
        this.Count = this.tail < this.head ?
            this.tail + this.buffer.Length - this.head + 1 :
            this.tail - this.head + 1;
    }

    public CircularBuffer(T[] buffer)
    {
        if (buffer.Length == 0) throw new ArgumentException($"{nameof(buffer)} cannot be of length 0");

        this.buffer = buffer;
        this.head = 0;
        this.tail = this.buffer.Length - 1;
        this.Count = this.buffer.Length;
    }

    public void Add(T item)
    {
        if (this.Count == this.buffer.Length)
        {
            this.buffer[this.tail] = item;
            this.tail = (this.tail + 1) % this.buffer.Length;
            this.head = (this.head + 1) % this.buffer.Length;
        }
        else
        {
            this.buffer[this.tail] = item;
            this.tail = (this.tail + 1) % this.buffer.Length;
            this.Count++;
        }
    }

    public void Clear()
    {
        this.head = 0;
        this.tail = 0;
        this.Count = 0;
    }

    public void DeepClear()
    {
        this.Clear();
        Array.Clear(this.buffer, 0, this.buffer.Length);
    }

    public bool Contains(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        foreach(var itItem in this)
        {
            if (comparer.Equals(itItem, item))
            {
                return true;
            }
        }

        return false;
    }

    public bool Remove(T item)
    {
        var index = this.head;
        var comparer = EqualityComparer<T>.Default;
        for (var i = 0; i < this.Count; i++)
        {
            if (comparer.Equals(this.buffer[index % this.buffer.Length], item))
            {
                int nextIndex;
                for (var j = 0; j < this.Count - i - 1; j++)
                {
                    nextIndex = (index + 1) % this.buffer.Length;
                    this.buffer[index] = this.buffer[nextIndex];
                    index = nextIndex;
                }

                this.Count--;
                this.tail = this.tail == 0 ? this.buffer.Length - 1 : this.tail - 1;
                return true;
            }

            index = (index + 1) % this.buffer.Length;
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        foreach(var item in this)
        {
            array[arrayIndex++] = item;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var index = this.head;
        for (var i = 0; i < this.Count; i++)
        {
            yield return this.buffer[index];
            index = (index + 1) % this.buffer.Length;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        var comparer = EqualityComparer<T>.Default;
        for (var i = 0; i < this.Count; i++)
        {
            if (comparer.Equals(this[i], item))
            {
                return i;
            }
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > this.Count) throw new ArgumentOutOfRangeException(nameof(index));
        
        if (this.Count == this.buffer.Length) throw new InvalidOperationException("Cannot insert into a full buffer.");

        if (index == this.Count)
        {
            this.Add(item);
            return;
        }

        for (var i = this.Count; i > index; i--)
        {
            this.buffer[(this.head + i) % this.buffer.Length] = this.buffer[(this.head + i - 1) % this.buffer.Length];
        }

        this.buffer[(this.head + index) % this.buffer.Length] = item;
        this.tail = (this.tail + 1) % this.buffer.Length;
        this.Count++;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= this.Count) throw new ArgumentOutOfRangeException(nameof(index));

        for (var i = index; i < this.Count - 1; i++)
        {
            this.buffer[(this.head + i) % this.buffer.Length] = this.buffer[(this.head + i + 1) % this.buffer.Length];
        }

        this.tail = (this.tail - 1 + this.buffer.Length) % this.buffer.Length;
        this.Count--;
    }
}