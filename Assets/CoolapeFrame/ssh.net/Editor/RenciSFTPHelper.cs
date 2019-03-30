using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Authentication;
using Renci.SshNet.Messages.Connection;
using Renci.SshNet.Messages.Transport;
using Renci.SshNet;
using System.IO;
using Coolape;

public class RenciSFTPHelper
{
    public SftpClient client;
    public RenciSFTPHelper(string host, string user, string password)
    {
        if (string.IsNullOrEmpty(host)
           || string.IsNullOrEmpty(user)
           || string.IsNullOrEmpty(password))
        {
            Debug.LogError("some params is empty!!" + "host==" + host + ",user ==" + user + ",password==" + password);
        }
        client = new SftpClient(host, user, password);
    }
    public bool connect()
    {
        //var connectionInfo = new ConnectionInfo(host,
        //"guest",
        //new PasswordAuthenticationMethod("guest", "pwd")
        //,new PrivateKeyAuthenticationMethod("rsa.key") 
        //);
        try
        {
            if (!client.IsConnected)
            {
                client.Connect();
            }
            return client.IsConnected;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public void disConnect()
    {
        client.Disconnect();
        client.Dispose();
        client = null;
    }

    public bool mkdir(string path)
    {
        try
        {
            if (!client.Exists(path))
            {
                client.CreateDirectory(path);
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public bool put(string localPath, string remotePath, Callback finishCallback)
    {
        try
        {
            FileStream fs = new FileStream(localPath, FileMode.Open, FileAccess.Read);
            client.UploadFile(fs, remotePath, true, (len) =>
            {
                Utl.doCallback(finishCallback, true, len);
            });
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Utl.doCallback(finishCallback, false);
            return false;
        }
    }

    public bool putDir(string localDir, string remoteDir, Callback onProgressCallback, Callback onFinishCallback)
    {
        bool ret = false;
        if (!Directory.Exists(localDir))
        {
            Debug.LogError("There is no directory exist!");
            Utl.doCallback(onFinishCallback, false);
            return false;
        }
        mkdir(remoteDir);
        string[] files = Directory.GetFiles(localDir);
        string file = "";
        string[] dirs = Directory.GetDirectories(localDir);
        if (files != null)
        {
            int finishCount = 0;
            for (int i = 0; i < files.Length; i++)
            {
                file = files[i];
                Debug.Log(file);
                string remoteFile = Path.Combine(remoteDir, Path.GetFileName(file));
                ret = put(file, remoteFile,
                          (Callback)((objs) =>
                          {
                              finishCount++;
                              Utl.doCallback(onProgressCallback, (float)finishCount / files.Length);
                          })
                         );
                if (!ret)
                {
                    Utl.doCallback(onFinishCallback, false);
                    return false;
                }
            }
        }

        if (dirs != null)
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                //              Debug.Log (PStr.b ().a (remotePath).a ("/").a (Path.GetFileName (dirs [i])).e ());
                ret = putDir(dirs[i], PStr.b().a(remoteDir).a("/").a(Path.GetFileName(dirs[i])).e(), onProgressCallback, null);
                if (!ret)
                {
                    Utl.doCallback(onFinishCallback, false);
                    return false;
                }
            }
        }
        Utl.doCallback(onFinishCallback, true);
        return ret;
    }

    public bool get(string remotePath, string localPath, Callback finishCallback)
    {
        try
        {
            FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate, FileAccess.Write);
            client.DownloadFile(remotePath, fs, (len) =>
            {
                Utl.doCallback(finishCallback, true, len);
            });
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Utl.doCallback(finishCallback, false);
            return false;
        }
    }

}
