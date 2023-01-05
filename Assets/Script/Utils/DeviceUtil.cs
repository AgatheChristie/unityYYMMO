using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class DeviceUtil 
{
    
    public static  string DeviceIdentifier
    {
        get { return SystemInfo.deviceUniqueIdentifier; }
    }

    public  static  string DeviceModel
    {
        get
        {
#if UNITY_IPHONE && !UNITY_EDITOR
             return Device.generation.ToString();
#else
            return SystemInfo.deviceModel;
#endif
        }
    }
}
