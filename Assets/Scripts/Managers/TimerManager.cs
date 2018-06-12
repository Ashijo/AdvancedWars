using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager
{

    #region Singleton
    private static TimerManager instance;


    private TimerManager()
    {
        TimeBook = new Dictionary<object, List<Timer>>();
    }

    public static TimerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimerManager();
            }
            return instance;
        }
    }

    #endregion

    private static Dictionary<object, List<Timer>> TimeBook;


    public void AddTimer(object from, Timer timer)
    {


        if (TimeBook.ContainsKey(from))
        {
            TimeBook[from].Add(timer);
        }
        else
        {
            List<Timer> list = new List<Timer>();
            list.Add(timer);
            TimeBook.Add(from, list);
        }
    }

    public void Start()
    {

        TimeBook = new Dictionary<object, List<Timer>>();
    }

    public void Update()
    {
        float timeSinceLastUpdate = Time.deltaTime;



        if (TimeBook.Count != 0)
        {

            List<object> TimerListToRemove = new List<object>();

            foreach (KeyValuePair<object, List<Timer>> pair in TimeBook)
            {
                List<int> toRemove = new List<int>();

                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i].Update(timeSinceLastUpdate))
                    {
                        toRemove.Add(i);
                    }
                }

                if (toRemove.Count == pair.Value.Count)
                {
                    TimerListToRemove.Add(pair.Key);
                }

                foreach (int i in toRemove)
                {
                    pair.Value.RemoveAt(i);
                }
            }

            foreach (object o in TimerListToRemove)
            {
                TimeBook.Remove(o);
            }
        }
    }
}


public class Timer
{
    private float TimeLeft;
    private float FirstTime;

    public delegate void toCall();
    private bool isTempo;

    toCall handler;

    public Timer(float timeToWait, toCall handler, bool isTempo)
    {
        TimeLeft = timeToWait;
        FirstTime = timeToWait;

        this.handler = handler;
        this.isTempo = isTempo;
    }

    /// <summary>
    /// !!!DANGER!!! 
    /// Be sure to never use this function if the timer is actually in use or you gonna reset it
    /// </summary>
    public void ChangeTimeToWait(float newTimeToWait)
    {
        TimeLeft = newTimeToWait;
        FirstTime = newTimeToWait;
    }


    public bool Update(float timeSinceLastUpdate)
    {
        bool destroyMe = false;

        TimeLeft -= timeSinceLastUpdate;

        if (TimeLeft <= 0)
        {
            if (!isTempo) destroyMe = true;

            TimeLeft = FirstTime;
            handler.Invoke();
        }
        return destroyMe;
    }
}
