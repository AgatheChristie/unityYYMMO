

public struct Role_Delete_Role_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Delete_Role_C2S; } }

    public int RoleId; 
    
    public byte[] ToArray()
    {
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(RoleId);
            return ms.ToArray();
        }
    }
    
}