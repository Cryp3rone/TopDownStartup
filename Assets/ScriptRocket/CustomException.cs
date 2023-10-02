using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CustomException {}
    
    public class SpawnerManagerException : Exception
    {
        public SpawnerManagerException(string errorInfo) : base(errorInfo)
        { }
    }
}
