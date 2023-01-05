

public struct Role_Login_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Login_C2S; } }

    public int AccId; 
    public int TsTamp; 
    public string AccName; 
    public string Ticket; 
  

    public byte[] ToArray()
    {
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(AccId);
            ms.WriteInt(TsTamp);
            ms.WriteUTF8String(AccName);
            ms.WriteUTF8String(Ticket);
            return ms.ToArray();
        }
    }
    
}