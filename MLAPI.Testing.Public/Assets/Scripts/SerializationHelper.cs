using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace DefaultNamespace
{
    public static class SerializationHelper
    {
        #region uint list serializer

        public static void Serialize<T>(List<uint> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(list, serializer, WriteUnmanaged, ReadUnmanaged<uint>);
        public static void Serialize<T>(ref List<uint> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(ref list, serializer, WriteUnmanaged, ReadUnmanaged<uint>);

        #endregion

        #region int list serializer

        public static void Serialize<T>(List<int> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(list, serializer, WriteUnmanaged, ReadUnmanaged<int>);
        public static void Serialize<T>(ref List<int> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(ref list, serializer, WriteUnmanaged, ReadUnmanaged<int>);

        #endregion

        #region long list serializer

        public static void Serialize<T>(List<long> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(list, serializer, WriteUnmanaged, ReadUnmanaged<long>);
        public static void Serialize<T>(ref List<long> list, BufferSerializer<T> serializer) where T : IReaderWriter => Serialize(ref list, serializer, WriteUnmanaged, ReadUnmanaged<long>);

        #endregion

        #region string list serializer

        public static void Serialize<T>(List<string> list, BufferSerializer<T> serializer) where T : IReaderWriter =>
            Serialize(list, serializer, WriteString, ReadString);

        #endregion


        #region INetworkSerializable list serializer

        public static void Serialize<T, U>(List<T> list, BufferSerializer<U> serializer) where T : INetworkSerializable, new() where U : IReaderWriter
        {
            NetworkSerialize(list, serializer);
        }


        public static void Serialize<T, U>(ref List<T> list, BufferSerializer<U> serializer) where U : IReaderWriter where T : INetworkSerializable, new()
        {
            NetworkSerialize(ref list, serializer);
        }

        #endregion


        #region Network Serialization

        public static void NetworkSerialize<T, U>(ref List<T> list,
                                                  BufferSerializer<U> serializer) where U : IReaderWriter where T : INetworkSerializable, new()
        {
            if (PrepareListForSerialization(ref list, serializer))
                return;

            NetworkSerialize(list, serializer);
        }

        public static void NetworkSerialize<T, U>(List<T> list, BufferSerializer<U> serializer) where U : IReaderWriter where T : INetworkSerializable, new()
        {

            int count = list.Count;
            serializer.SerializeValue(ref count);

            for (int i = 0; i < count; i++)
            {
                if (!serializer.IsReader)
                {
                    list[i].NetworkSerialize(serializer);

                }
                else
                {
                    var item = new T();
                    item.NetworkSerialize(serializer);
                    list.Add(item);
                }
            }
        }

        #endregion

        #region Read & Write Serialization

        public static void Serialize<T, U>(ref List<T> list,
                                           BufferSerializer<U> serializer,
                                           Action<T, FastBufferWriter> writeAction,
                                           Func<FastBufferReader, T> readAction) where U : IReaderWriter
        {
            if (PrepareListForSerialization(ref list, serializer))
                return;

            Serialize(list, serializer, writeAction, readAction);
        }

        private static void Serialize<T, U>(List<T> list,
                                            BufferSerializer<U> serializer,
                                            Action<T, FastBufferWriter> writeAction,
                                            Func<FastBufferReader, T> readAction) where U : IReaderWriter
        {
            int count = list.Count;
            serializer.SerializeValue(ref count);

            for (int i = 0; i < count; i++)
            {
                if (!serializer.IsReader)
                {
                    writeAction(list[i], serializer.GetFastBufferWriter());
                }
                else
                {
                    list.Add(readAction(serializer.GetFastBufferReader()));
                }
            }
        }
        #endregion

        private static bool PrepareListForSerialization<T, U>(ref List<T> list, BufferSerializer<U> serializer) where U : IReaderWriter
        {

            bool isNull = list == null;
            serializer.SerializeValue(ref isNull);

            if (isNull)
            {
                if (serializer.IsReader)
                    list = null;
                return true;
            }

            if (list == null)
                list = new List<T>();

            if (serializer.IsReader)
                list.Clear();
            return false;
        }


        #region Generic Reader & Writer

        private static void WriteUnmanaged<T>(T item, FastBufferWriter writer) where T : unmanaged => writer.WriteValueSafe(item);

        private static T ReadUnmanaged<T>(FastBufferReader reader) where T : unmanaged
        {
            reader.ReadValueSafe(out T item);
            return item;
        }

        private static void WriteString(string value, FastBufferWriter writer)
        {
            char[] payload = value.ToCharArray();
            writer.WriteValueSafe(payload);
        }
        private static string ReadString(FastBufferReader reader)
        {
            reader.ReadValueSafe(out char[] item);
            return item.ToString();
        }

        #endregion
    }
}
