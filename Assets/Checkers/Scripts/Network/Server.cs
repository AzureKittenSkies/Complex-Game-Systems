using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Checkers
{
    public class Server : MonoBehaviour
{
    public int port = 6321;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        // perform a 'try-catch' block for any errors
        try
        {
            // create a new TcpListener for any IP addresses
            server = new TcpListener(IPAddress.Any, port);
            // start the server
            server.Start();
            // start the listening method
            StartListening();
            // flag the server as 'started'
            serverStarted = true;
        }
        catch (Exception e)
        {
            // if an error is detected, display the message
            Debug.Log("Socket error: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // is the server not started?
        if (!serverStarted)
        {
            return;
        }

        // loop through entire list 
        foreach (ServerClient client in clients)
        {
            // is the client still connected?
            if (IsConnected(client.tcp))
            {
                // get the client's network stream
                NetworkStream stream = client.tcp.GetStream();
                // check if data is available on the stream
                if (stream.DataAvailable)
                {
                    // setup a reader for the stream
                    StreamReader reader = new StreamReader(stream, true);
                    // read the data from teh reader of the stream
                    string data = reader.ReadLine();
                    // if there is any data
                    if (data != null)
                    {
                        // give the client and it's data to the method
                        OnIncomingData(client, data);
                    }
                }
            }

            else // the client disconnected
            {
                // close the client's tcp protocol (disconnect client)
                client.tcp.Close();
                // add to the list of disconnected
                disconnectList.Add(client);
                continue;
            }
        }

        // loop through all the disconnected clients
        for (int i = 0; i < disconnectList.Count; i++)
        {
            // tell our player somebody has disconnected
            clients.Remove(disconnectList[i]);
        }

        // clear the disconnected list for another time
        disconnectList.Clear();

    }

    // runs command to continue listening for a tcp client to connect
    private void StartListening()
    {
        Server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    // callback method for listening for tcp clients
    private void AcceptTcpClient(IAsyncResult result)
    {
        // get the listener
        TcpListener listener = (TcpListener)result.AsyncState;

        string allUsers = "";
        // loop through all currently connected clients
        foreach (ServerClient i in clients)
        {
            // append with client name
            allUsers += i.clientName + '|';
        }

        // get the connected client from the listener
        ServerClient connectedClient = new ServerClient(listener.EndAcceptTcpClient(result));
        // add newly connected client to the list
        clients.Add(connectedClient);
        // continue listening for more clients
        StartListening();
        // broadcast to all clients that there is a newly connected client
        Broadcast("SWHO|" + allUsers, connectedClient);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null & c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    // broadcast data to a list of incomingClients
    private void Broadcast(string data, List<ServerClient> incomingClients)
    {
        // loop through all incoming clients from broadcast
        foreach (ServerClient client in incomingClients)
        {
            // try sending the data to the client
            try
            {
                // get a writer specifically for the current client
                StreamWriter writer = new StreamWriter(client.tcp.GetStream());
                // send the data to client with writer
                writer.WriteLine(data);
                // flush the writer when done
                writer.Flush();
            }
            catch (Exception e) // the data couldn't be sent
            {
                // print the message error
                Debug.Log("Write error : " + e.Message);
            }
        }
    }

    // broadcast data to a single client exists for simplicity (less syntax)
    private void Broadcast(string data, ServerClient incomingClient)
    {
        // create a list containing the individual client
        List<ServerClient> client = new List<ServerClient> { incomingClient };
        // broadcast to that list of one client
        Broadcast(data, client);
    }

    private void OnIncomingData(ServerClient client, string data)
    {
        //Client Commands:
        //     CWHO - Client who connected
        //     CMOV - Movement data from client
        //     CMSG - Message from the client
        //     
        //     Server Commands:
        //     SCNN - Server new connection
        //     SMOV - Server movement broadcast
        //     SMSG - Server message broadcast

        Debug.Log("Server: " + data);

        // split the data with in-line '|'
        string[] aData = data.Split('|');

        // switch the header of the packet
        switch (aData[0])
        {
            // client connected. Syntax - "CWHO | clientName | isHost"
            case "CWHO":
                // get the client's name
                client.clientName = aData[1];
                // check if the client is a host
                client.isHost = (aData[2] == "0") ? false : true;
                // broadcast the new cilent to all other clients
                Broadcast("SCNN|" + client.clientName, clients);
                break;

            default:
                break;
        }
    }




}
}