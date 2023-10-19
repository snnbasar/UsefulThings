using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackManager<T> : MonoBehaviour
{
    public int maxStack;
    public int currentStack;
    public List<T> _stack = new List<T>();
    public List<T> stack
    {
        get => _stack; set
        {
            _stack = value;
            OnStackListChanged?.Invoke();
        }
    }

    public event Action OnStackListChanged;


    public abstract void OnStack_Awake();
    public void Stack_Awake()
    {
        OnStackListChanged += OnStackChanged;
    }



    public virtual void AddMeToStack(T t)
    {
        stack.Add(t);
        currentStack++;
    }


    public virtual void RemoveMeFromStack(T t)
    {
        stack.Remove(t);
        currentStack--;

    }


    public abstract void OnStackChanged();

    public bool IsStackFull() => currentStack >= maxStack;

}
