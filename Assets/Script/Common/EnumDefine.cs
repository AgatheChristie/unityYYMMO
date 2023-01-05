using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SceneType
{
    LogOn,
    SelectRole,
    City
}

public enum WindowUIType
{
    None,
    LogOn,
    Reg,
    RoleInfo,
    GameServerEnter,
    GameServerSelect
}

public enum MessageViewType
{
    Ok,
    OkAndCancel
}
public enum WindowUIContainerType
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
    Center
}

public enum WindowShowStyle
{
    Normal,
    CenterToBig,
    FromTop,
    FromDown,
    FromLeft,
    FromRight
}