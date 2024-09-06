using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "ReactionImages", menuName = "MonsterPalace/ReactionImage")]

public class SO_ReactionImage : ScriptableObject
{
    [SerializedDictionary("ID", "Image" )]
    public SerializedDictionary<string, Sprite> ReactionImages;

    public Sprite returnEmote(string id)
    {
        return ReactionImages[id];
    }

}
