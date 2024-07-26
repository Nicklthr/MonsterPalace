using Esper.ESave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SaveManager : MonoBehaviour
{
    
    [SerializeField] private SO_Bestiary bestiary;
    [SerializeField] private JetonSO jetons;
    [SerializeField] private SO_RoomType room;
    [SerializeField] private List<SO_RoomType> rooms;
    [SerializeField] private List<SO_Food> foods;

    public UnityEvent onSaveGame = new UnityEvent();
    public UnityEvent onLoadGame = new UnityEvent();
    public UnityEvent onEndSave = new UnityEvent();
    public UnityEvent onEndLoad = new UnityEvent();


    private void Start()
    {

    }


    public void SaveGame()
    {

        onSaveGame.Invoke();


        ES3.Save("bestiary", bestiary);
        ES3.Save("jetons", jetons);


        for (int i = 0; i < rooms.Count; i++)
        {
            ES3.Save(rooms[i].roomName.ToString(), rooms[i].isUnlocked);
        }


        for (int i = 0; i < foods.Count; i++)
        {
            ES3.Save(foods[i].foodName.ToString(), foods[i].isUnlocked);
        }



        onEndSave.Invoke();
    }

    public void LoadGame()
    {
        onLoadGame.Invoke();
       
        if (ES3.KeyExists("bestiary"))
        {
            bestiary = ES3.Load<SO_Bestiary>("bestiary");
        }

        if (ES3.KeyExists("jetons"))
        {
            jetons = ES3.Load<JetonSO>("jetons");
        }

        for(int i = 0; i < rooms.Count; i++)
        {
            if (ES3.KeyExists(rooms[i].roomName.ToString()))
            {
                rooms[i].isUnlocked = ES3.Load<bool>(rooms[i].roomName.ToString());
            }
        }

        for (int i = 0; i < foods.Count; i++)
        {
            if (ES3.KeyExists(foods[i].foodName.ToString()))
            {
                foods[i].isUnlocked = ES3.Load<bool>(foods[i].foodName.ToString());
            }
        }

        onEndLoad.Invoke();

    }
}
