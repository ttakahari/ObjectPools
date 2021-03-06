﻿using System;
using System.Collections.Concurrent;

namespace ObjectPool.Net
{
    public class ObjectPoolClient<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly Func<T> _valueFactory;

        public int PoolCount => _queue.Count;

        public ObjectPoolClient(Func<T> valueFactory)
        {
            _queue        = new ConcurrentQueue<T>();
            _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
        }

        public T Get()
        {
            if (!_queue.TryDequeue(out var value))
            {
                value = _valueFactory.Invoke();
            }

            return value;
        }

        public void Set(T value)
        {
            _queue.Enqueue(value);
        }

        public void Clear()
        {
            while (_queue.TryDequeue(out var _)) { }
        }
    }
}
