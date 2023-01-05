

public struct Role_Create_Role_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Create_Role_C2S; } }

    public byte Realm; 
    public byte Career; 
    public byte Sex;
    public string NickName;
    
    public byte[] ToArray()
    {
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteByte(Realm);
            ms.WriteByte(Career);
            ms.WriteByte(Sex);
            ms.WriteUTF8String(NickName);
            return ms.ToArray();
        }
    }
    
}