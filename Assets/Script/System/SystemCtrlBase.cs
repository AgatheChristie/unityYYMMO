using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCtrlBase<T> : IDisposable where T : new()
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

    protected void ShowMessage(string title, string message, MessageViewType type = MessageViewType.Ok,
        Action okAction = null,
        Action cancelAction = null)
    {
        MessageCtrl.Instance.Show(title, message, type, okAction, cancelAction);
    }


    protected void AddEventListener(string key, DispatcherBase<UIDispatcher, object[], string>.OnActionHandler handler)
    {
        UIDispatcher.Instance.AddEventListener(key, handler);
    }
    
    protected void RemoveEventListener(string key, DispatcherBase<UIDispatcher, object[], string>.OnActionHandler handler)
    {
        UIDispatcher.Instance.RemoveEventListener(key, handler);
    }

    protected void Log(object message)
    {
        AppDebug.Log(message);
    }
    
    protected void LogError(object message)
    {
        AppDebug.LogError(message);
    }

}