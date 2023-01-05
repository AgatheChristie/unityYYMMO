//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-01-24 23:08:33
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Linq;
using System.Text;


/// <summary>
/// 数据转换(byte short int long float decimal bool string)
/// </summary>

public class MMO_QMemoryStream : MemoryStream
{
    public MMO_QMemoryStream()
    {

    }

    public MMO_QMemoryStream(byte[] buffer) : base(buffer)
    {

    }

  

    #region UShort
    /// <summary>
    /// 从流中读取一个ushort数据
    /// </summary>
    /// <returns></returns>
    public ushort ReadUShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);
        byte[] arr2 = (BitConverter.IsLittleEndian ? arr.Reverse().ToArray() : arr);
        return BitConverter.ToUInt16(arr2, 0);
    }

    /// <summary>
    /// 把一个ushort数据写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteUShort(ushort value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        byte[] arr2 = (BitConverter.IsLittleEndian ? arr.Reverse().ToArray() : arr);
        base.Write(arr2, 0, arr2.Length);
    }
    #endregion
    
    #region Int
    /// <summary>
    /// 从流中读取一个int数据
    /// </summary>
    /// <returns></returns>
    public int ReadInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        byte[] arr2 = (BitConverter.IsLittleEndian ? arr.Reverse().ToArray() : arr);
        return BitConverter.ToInt32(arr2, 0);
    }

    /// <summary>
    /// 把一个int数据写入流
    /// </summary>
    /// <param name="value"></param>
    public void WriteInt(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        byte[] arr2 = (BitConverter.IsLittleEndian ? arr.Reverse().ToArray() : arr);
        base.Write(arr2, 0, arr2.Length);
    }
    #endregion

    
    #region UTF8String
    /// <summary>
    /// 从流中读取一个sting数组
    /// </summary>
    /// <returns></returns>
    public string ReadUTF8String()
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];
        base.Read(arr, 0, len);
        return Encoding.UTF8.GetString(arr);
    }

    /// <summary>
    /// 把一个string数据写入流
    /// </summary>
    /// <param name="str"></param>
    public void WriteUTF8String(string str)
    {
        byte[] arr = Encoding.UTF8.GetBytes(str);
        if (arr.Length > 65535)
        {
            throw new InvalidCastException("字符串超出范围");
        }
        WriteUShort((ushort)arr.Length);
        base.Write(arr, 0, arr.Length);
    }
    #endregion
    
    
   
}