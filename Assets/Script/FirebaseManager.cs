using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Messaging;
using Firebase.Crashlytics;
using Firebase;

public class FirebaseManager : MonoBehaviour
{
    FirebaseApp app;
    public void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessabeReceived;
                Debug.Log("[Success] Init Firebase");
            }
            else
            {
                Debug.LogError(string.Format("[Failed] Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    void OnTokenReceived(object sender, TokenReceivedEventArgs arg)
    {
        if (arg != null)
        {
            Debug.Log(string.Format("[Firebase] Token = {0}", arg.Token));
        }
        else
        {
            Debug.Log(string.Format("[Firebase] Token = null"));
        }
    }

    private void OnMessabeReceived(object sender, MessageReceivedEventArgs arg)
    {
        if (arg != null && arg.Message != null && arg.Message.Notification != null)
        {
            Debug.Log(string.Format("[Firebase] Message From = {0}, Title = {1}, Body = {2}", 
                arg.Message.From, arg.Message.Notification.Title, arg.Message.Notification.Body));
        }
    }

    public void TestCrash()
    {
        throw new System.Exception("(ignore) this is a test crash");
    }
}
