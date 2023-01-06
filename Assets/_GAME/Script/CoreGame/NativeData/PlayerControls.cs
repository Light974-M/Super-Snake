using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerControls
{
    [SerializeField]
    private List<KeyCode> _valuesControls;

    public List<KeyCode> ValuesControls
    {
        get { return _valuesControls; }
        set { _valuesControls = value; }
    }
}
