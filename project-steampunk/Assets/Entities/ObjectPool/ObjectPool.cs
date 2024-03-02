using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Func<T> _objectCreation;
    private readonly Action<T> _objectActivation;
    private readonly Action<T> _objectDeactivation;

    private Queue<T> _pool;
    private List<T> _active;

    public ObjectPool(Func<T> objectCreation, Action<T> objectActivation, Action<T> objectDeactivation, int poolSize)
    {
        _objectCreation = objectCreation;
        _objectActivation = objectActivation;
        _objectDeactivation = objectDeactivation;

        for(int i = 0; i < poolSize; i++)
        {
            Return(objectCreation());
        }
    }

    public T Get()
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : _objectCreation();
        _objectActivation(obj);
        _active.Add(obj);

        return obj;
    }

    public void Return(T obj)
    {
        _objectDeactivation(obj);
        _pool.Enqueue(obj);
        _active.Remove(obj);
    }

    public void ReturnAll()
    {
        foreach(var obj in _active)
            Return(obj);
    }
}
