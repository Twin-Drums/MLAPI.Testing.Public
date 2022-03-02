using System.Collections.Generic;
using Unity.Netcode;

namespace DefaultNamespace
{
    public class CustomMessageType : INetworkSerializable
    {
        public string someString;
        public List<int> someList;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref someString);
            SerializationHelper.Serialize(ref someList, serializer);
        }
    }
}