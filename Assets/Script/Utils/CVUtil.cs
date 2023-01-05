using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CVUtil 
{
    public static long GetTimestamp()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
    }
}
