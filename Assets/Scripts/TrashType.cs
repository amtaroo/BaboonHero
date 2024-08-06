using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TrashType
{
    Recyclable,
    Compostable,
    Hazardous,
    General
}

public class TrashItem : MonoBehaviour
{
    public TrashType trashType;
}