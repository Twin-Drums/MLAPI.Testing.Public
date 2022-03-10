using Unity.Collections;
using Unity.Netcode;

public class IdName : INetworkSerializable
{
    public int Id;
    public FixedString512Bytes Name;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Name);
    }
}