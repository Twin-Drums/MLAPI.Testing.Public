using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using UnityEngine;
public class Server : MonoBehaviour
{
    private Connector connector;
    [SerializeField] private int CustomMessageAmount = 5;
    [SerializeField] private int CustomMessageArrayLength = 5;


    private void Awake() => connector = GetComponent<Connector>();

    private void Start()
    {
        connector.ExecuteWhenNetworkHasStarted(Initialize); 
        InvokeRepeating($"AddToFixedList", 3f, 3f);
        
    }

    private void Initialize()
    {
        for (int i = 0; i < CustomMessageAmount; i++)
        {
            AddUnmanagedContainer();
        } 
        CustomMessageType test = new CustomMessageType
        {
            someString = "Live long and prosper.",
            someList = new List<int> { 4, 5, 6, 3, 5, 6, 7, 4, 5, 6, 76, 4, 345, 34, 5, 345, 3, 45, 345 }
        };
        connector.SendCustomMessage(test);

        IdName[] arr = new IdName[CustomMessageArrayLength];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new IdName
            {
                Id = i,
                Name = "test.id." + i
            };
        }

        connector.SendPayloadWithArray(arr);
    }
    
    public void AddToFixedList()
    {
        FixedList512Bytes<int> fixedList512Bytes = connector.FixedListVariable.testList;
        fixedList512Bytes.Add(5);
        connector.FixedListVariable = new FixedList
        {
            testList = fixedList512Bytes
        };
    }
    private void AddUnmanagedContainer()
    {
        connector.Value01.Add(new UnmanagedTestContainer
        {
            Value01 = Random.Range(0,100).ToString(),
            Value02 = Random.Range(0,100).ToString(),
            Value03 = Random.Range(0,100).ToString()
        });
    }
}
