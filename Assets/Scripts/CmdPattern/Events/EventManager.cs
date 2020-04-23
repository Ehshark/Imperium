using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    //public access to instance
    public static EventManager Instance { get; private set; } = null;

    //Array of listeners (all objects registered for events)
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Function to add listener to array of listeners
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {
        //Check existing event type key. If exists, add to list
        if (Listeners.TryGetValue(Event_Type, out List<IListener> ListenList))
        {
            //List exists, so add new item
            ListenList.Add(Listener);
            return;
        }

        //Otherwise create new list as dictionary key
        ListenList = new List<IListener>
        {
            Listener
        };
        Listeners.Add(Event_Type, ListenList);
    }

    public void PostNotification(EVENT_TYPE Event_Type)
    {
        //Notify all listeners of an event

        //If no event exists, then exit
        if (!Listeners.TryGetValue(Event_Type, out List<IListener> ListenList))
            return;

        //Entry exists. Now notify appropriate listeners
        for (int i = 0; i < ListenList.Count; i++)
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type);
    }

    //Remove event from dictionary, including all listeners
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        //Remove entry from dictionary
        Listeners.Remove(Event_Type);
    }

    //Remove all redundant entries from the Dictionary
    public void RemoveRedundancies()
    {
        //Create new dictionary
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener>>();

        //Cycle through all dictionary entries
        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            //Cycle all listeners, remove null
            for (int i = Item.Value.Count - 1; i >= 0; i--)
                //if null then remove item
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);

            //If items remain in list, then add to tmp dictionary
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);

            //Replace listeners object with new dictionary
            Listeners = TmpListeners;
        }
    }

    //Following function is deprecated
    //private void OnLevelWasLoaded()
    //{
    //    RemoveRedundancies();
    //}

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //Called on scene change. Clean up dictionary
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        RemoveRedundancies();
    }

}
