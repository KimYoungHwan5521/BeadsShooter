using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    public List<CharacterData> characters = new();
    public IEnumerator Initiate()
    {
        characters.Add(new("Ball Handler", 20));
        yield return null;
    }
}
