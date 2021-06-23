using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FreeNet;
using FreeNetUnity;

public interface IMessageReceiver
{
    void on_recv(CPacket msg);
}

public class CNetworkManager : CSingletonMonobehaviour<CNetworkManager>
{

    [SerializeField]
    private string server_ip;

    [SerializeField]
    private string server_port;

    CFreeNetUnityService freenet;
    public IMessageReceiver message_receiver;
    Queue<CPacket> sending_queue;

    string received_msg;

    void Awake()
    {
        // 네트워크 통신을 위해 CFreeNetUnityService객체를 추가합니다.
        this.freenet = gameObject.AddComponent<CFreeNetUnityService>();

        // 패킷 수신 델리게이트 설정.
        this.freenet.appcallback_on_message += this.on_message;

        // 상태 변화(접속, 끊김등)를 통보 받을 델리게이트 설정.
        this.freenet.appcallback_on_status_changed += this.on_status_changed;

        this.sending_queue = new Queue<CPacket>();
    }


    public void connect()
    {
        // 이전에 보내지 못한 패킷은 모두 버린다.
        this.sending_queue.Clear();

        if (!this.freenet.is_connected())
        {
            this.freenet.connect(this.server_ip, int.Parse(this.server_port));
        }
    }

    public void disconnect()
    {
        if (is_connected())
        {
            this.freenet.disconnect();
            return;
        }
    }

    /// <summary>
    /// 네트워크 상태 변경시 호출될 콜백 매소드.
    /// </summary>
    void on_status_changed(NETWORK_EVENT status)
    {
        switch (status)
        {
            // 접속 성공.
            case NETWORK_EVENT.connected:
                {
                    CLogManager.log("on connected");
                    this.received_msg += "on connected\n";

                    //GameObject.Find("MainTitle").GetComponent<CMainTitle>().on_connected();
                    gameObject.GetComponent<CMainTitle>().on_connected();
                }
                break;

            // 연결 끊김.
            case NETWORK_EVENT.disconnected:
                CLogManager.log("disconnected");
                this.received_msg += "disconnected\n";
                break;
        }
    }
    void on_message(CPacket msg)
    {
        this.message_receiver.on_recv(msg);
    }

    public void send(CPacket msg)
    {
        this.sending_queue.Enqueue(msg);
    }


    void Update()
    {
        if (!this.freenet.is_connected())
        {
            return;
        }

        while (this.sending_queue.Count > 0)
        {
            CPacket msg = this.sending_queue.Dequeue();
            this.freenet.send(msg);
        }
    }


    public bool is_connected()
    {
        if (this.freenet == null)
        {
            return false;
        }

        return this.freenet.is_connected();
    }
}
