using Unity.Collections;
using Unity.Netcode;

namespace DefaultNamespace
{
    public struct FixedList : INetworkSerializable
    {
        public FixedList512Bytes<int> testList;
        public FixedList512Bytes<int> testList2;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref testList);
            serializer.SerializeValue(ref testList2);
        }
    }
}