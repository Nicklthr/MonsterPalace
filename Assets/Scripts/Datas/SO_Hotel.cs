using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHotel", menuName = "MonsterPalace/Hotel")]
public class SO_Hotel : ScriptableObject
{
    public List<Room> rooms;

    public void AddRoom( Room room )
    {
        rooms.Add( room );
    }

    public void RemoveRoom( Room room )
    {
        rooms.Remove( room );
    }
}
