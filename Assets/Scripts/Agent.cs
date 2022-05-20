using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class Agent : MonoBehaviour
{
    [Tooltip("The amount of time in milliseconds to wait after invoking the Agent")]
    public int invokeInterval = 10;
    [Tooltip("The AudioClips which the Agent must cycle through")]
    public List<AudioClip> clips = new List<AudioClip>(16);

    private long _lastStamp = long.MaxValue;
    
    private bool _successful = false;
    private int _currentIndex;

    /**
     * Starts the affective feedback clock
     */
    public void WakeUpAgent()
    {
        this._currentIndex = (new Random()).Next(this.clips.Count);
        this.ResetClock();
    }

    private long GetUnixMillis()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    
    private void ResetClock()
    {
        this._lastStamp = this.GetUnixMillis();
    }

    private AudioClip DequeueNext()
    {
        if (this._successful) this._currentIndex += 4;
        else this._currentIndex++;
        if (this._currentIndex >= this.clips.Count) this._currentIndex -= this.clips.Count;
        
        return this.clips[this._currentIndex];
    }

    protected void Start()
    {
        var sim = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        sim.OnCollision.AddListener(s =>
        {
            this.ResetClock();
            this._successful = true;
        });
    }

    protected void Update()
    {
        var nowStamp = this.GetUnixMillis();
        if (nowStamp - this._lastStamp > this.invokeInterval)
        {
            this.ResetClock();

            var source = this.gameObject.GetComponent<AudioSource>();
            var nextClip = this.DequeueNext();
            Debug.Log(nextClip.name);
            
            source.PlayOneShot(nextClip);
            this._successful = false;
        }
    }
}