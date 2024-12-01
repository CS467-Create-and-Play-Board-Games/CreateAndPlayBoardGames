using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TokenSelectionData", menuName = "Scripts/TokenSelectionData")]
public class TokenSelectionData : ScriptableObject
{
    public Dictionary<int, Sprite> playerSelectedTokens = new Dictionary<int, Sprite>();
}