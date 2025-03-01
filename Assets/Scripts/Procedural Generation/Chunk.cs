using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    LevelGenerator _levelGenerator;
    
    void Start()
    {
    }

    public void Init(LevelGenerator levelGenerator)
    {
        _levelGenerator = levelGenerator;
    }
}
