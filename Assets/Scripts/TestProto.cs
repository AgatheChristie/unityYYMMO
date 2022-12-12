using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TestProto : IProto
{
    public ushort ProtoCode { get { return 12001; } }
    public int Id;
    public string Name;
    public int Type;
    public int Price;

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteInt(Id);
            ms.WriteUTF8String(Name);
            ms.WriteInt(Type);
            ms.WriteInt(Price);
            return ms.ToArray();
        }

       
    }
    public static TestProto GetProto(byte[] buffer)
    {

        TestProto proto = new TestProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.Id = ms.ReadInt();
            proto.Name = ms.ReadUTF8String();
            proto.Type = ms.ReadInt();
            proto.Price = ms.ReadInt();
        }
        return proto;
       
    }

    
}
