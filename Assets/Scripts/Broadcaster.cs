using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcaster : MonoBehaviour
{
    public static Broadcaster Instance;

    private void Awake() {
        if(Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void AddIntParam(string stateName, string eventName, int Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddBoolParam(string stateName, string eventName, bool Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddStringParam(string stateName, string eventName, string Value) {
        Parameters tempParam = new Parameters(); 
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }
    
    public void AddFloatParam(string stateName, string eventName, float Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddStartState(string stateName, string eventName, StartState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddGameState(string stateName, string eventName, GameState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddLevelState(string stateName, string eventName, LevelState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddTimerState(string stateName, string eventName, TimerState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddSFXState(string stateName, string eventName, SFXState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddVFXState(string stateName, string eventName, VFXState Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

}
