using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo  {

    private int id;

    private string name;

    internal int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    internal string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public PlayerInfo(int id , string name)
    {
        this.id = id;
        this.name = name;
    }
}
