using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatcherBase<T, P, X> : IDisposable
    where T : new()
    where P : class
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }

    public virtual void Dispose()
    {
    }

 //==============================================================
    public delegate void OnActionHandler(P p);

    public Dictionary<X, List<OnActionHandler>> dic =
        new Dictionary<X, List<OnActionHandler>>();

    public void AddEventListener(X key, OnActionHandler handler)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(handler);
        }
        else
        {
            List<OnActionHandler> lstHandler = new List<OnActionHandler>();
            lstHandler.Add(handler);
            dic[key] = lstHandler;
        }
    }


    public void RemoveEventListener(X key, OnActionHandler handler)
    {
        if (dic.ContainsKey(key))
        {
            List<OnActionHandler> lstHandler = dic[key];
            lstHandler.Remove(handler);
            if (lstHandler.Count == 0)
            {
                dic.Remove(key);
            }
        }
    }

    public void Dispatch(X key, P p)
    {
        if (dic.ContainsKey(key))
        {
            List<OnActionHandler> lstHandler = dic[key];
            if (lstHandler != null && lstHandler.Count > 0)
            {
                for (int i = 0; i < lstHandler.Count; i++)
                {
                    if (lstHandler[i] != null)
                    {
                        lstHandler[i](p);
                    }
                }
            }
        }
    }

    public void Dispatch(X key)
    {
        Dispatch(key, null);
    }

}