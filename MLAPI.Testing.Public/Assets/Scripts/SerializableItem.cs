using MLAPI.Serialization;

public class SerializableItem : INetworkSerializable
{
    public float Value;
    public void NetworkSerialize(NetworkSerializer serializer) => serializer.Serialize(ref Value);
}