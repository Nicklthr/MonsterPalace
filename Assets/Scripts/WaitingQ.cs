using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaitSpot
{
    public Transform position;
    public GameObject monster;
    public bool reception;
    public bool lineEnd;
}

public class WaitingQ : MonoBehaviour
{

    public List<System.Action> myEvent = new();

    public List<WaitSpot> waiting = new();

    public static WaitingQ instance;

    // Start is called before the first frame update
    void Start()
    {

        if( WaitingQ.instance == null )
        {
            WaitingQ.instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Register( System.Action callback )
    {
        myEvent.Add( callback );
    }

    public void NoPlace()
    {
        foreach ( System.Action action in myEvent )
        {
            action.Invoke();
        }
    }

    public void Sub( GameObject monster )
    {
        foreach ( WaitSpot spot in waiting )
        {
            if( spot.monster == null )
            {

                monster.GetComponent<MonsterController>().MoveInQueue(spot.position, spot.reception, spot.lineEnd);


                if (!spot.lineEnd)
                {
                    spot.monster = monster;     
                }

                break;
            }
        }

       
    }

    public void Quit( GameObject monster )
    {
        waiting[ 0 ].monster = null;

        for ( int i = 0; i < waiting.Count-1; i++ )
        {
            
                waiting[i].monster = waiting[i + 1].monster;
                waiting[i + 1].monster = null;

                if (waiting[i].monster != null)
                {
                    waiting[i].monster.GetComponent<MonsterController>().MoveInQueue(waiting[i].position, waiting[i].reception, waiting[i].lineEnd);
                }
                    
        }
    }

    public void Flush()
    {
        foreach ( WaitSpot spot in waiting )
        {
            spot.monster = null;
        }
    }

    public GameObject GetFirst()
    {
        return waiting[ 0 ].monster;
    }

    public GameObject Pop()
    {
        if ( waiting[ 0 ].monster == null ) return null;

        GameObject m = waiting[ 0 ].monster;
        waiting[ 0 ].monster = null;
        return m;
    }
}
