using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                Debug.Log("[Success] Init Firebase");
            }
            else
            {
                Debug.LogError(string.Format("[Failed] Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void TestCrash()
    {
        throw new System.Exception("(ignore) this is a test crash");
    }
}
