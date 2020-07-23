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

    private const byte PLAYER_REJOIN_EVENT = 0;
    private const byte HOST_LEFT_EVENT = 1;
    private const byte CLIENT_LEFT_EVENT = 2;

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
        byte code = photonData.Code;

        if (code == HOST_LEFT_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            Manager Client = (Manager)ByteArrayToObject((byte[])data[0]);
            //MinionDataPhoton[] minions = (MinionDataPhoton[])ByteArrayToObject((byte[])data[1]);

            MigrationManager.Instance.Host = MigrationManager.Instance.Client;
            MigrationManager.Instance.Client = Client;
        }
        else if (code == PLAYER_REJOIN_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            Manager Client = (Manager)ByteArrayToObject((byte[])data[0]);

            MigrationManager.Instance.Client = Client;
            MigrationManager.Instance.buttonText.text = Client.val.ToString();
        }
        else if (code == CLIENT_LEFT_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            Manager Client = (Manager)ByteArrayToObject((byte[])data[0]);

            MigrationManager.Instance.Client = Client;
        }
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
