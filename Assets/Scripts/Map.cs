using System.Collections.Generic;
using GonDraz;
using UnityEngine;

public class Map : Base
{
    [SerializeField] private List<Map> mapsLoad;
    [SerializeField] private List<Map> mapsUnload;

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnloadMaps();
        LoadMaps();
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     UnloadMaps();
    // }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     Debug.Log("OnTriggerStay2D");
    // }

    private void LoadMaps()
    {
        foreach (var map in mapsLoad) map.Active();
    }

    private void UnloadMaps()
    {
        foreach (var map in mapsUnload) map.Inactive();
    }
}