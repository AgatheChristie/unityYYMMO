

public struct Role_Get_Role_List_C2SProto : IProto
{
    public ushort ProtoCode { get { return ProtoCodeDef.Role_Get_Role_List_C2S; } }

   
  

    public byte[] ToArray()
    {
        using (MMO_QMemoryStream ms = new MMO_QMemoryStream())
        {
            ms.WriteUShort(ProtoCode);
         
            return ms.ToArray();
        }
    }
    
}