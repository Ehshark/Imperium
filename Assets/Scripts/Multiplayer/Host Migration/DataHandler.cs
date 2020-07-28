using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; } = null;

    private RaiseEventOptions raiseEventOptions;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonData)
    {
        
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, raiseEventOptions, SendOptions.SendReliable);
    }

    // Convert an object to a byte array
    public byte[] ObjectToByteArray(System.Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // Convert a byte array to an Object
    public System.Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }
}
