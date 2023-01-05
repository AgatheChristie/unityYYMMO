

public struct Role_Myself_Info_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Myself_Info_C2S; } }

 
    
    public byte[] ToArray()
    {
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
          
            return ms.ToArray();
        }
    }
    
}