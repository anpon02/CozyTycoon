using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager instance;

    [SerializeField] private List<LineSpot> lineSpots;
    private int nextOpenSpot;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
        
        nextOpenSpot = 0;
    }

    public List<LineSpot> GetLineSpots() {
        return lineSpots;
    }

    public int GetNextOpenSpot() {
        return nextOpenSpot;
    }

    public void UpdateNextOpenSpot() {
        for(int i = 0; i < lineSpots.Count; ++i) {
            if(!lineSpots[i].GetPlaceIsTaken()) {
                nextOpenSpot = i;
                return;
            }
        }
    }

    public Vector3 GetNextSpotVector() {
        return lineSpots[nextOpenSpot].GetPlaceCoordinates();
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var l in lineSpots) Gizmos.DrawSphere(l.GetPlaceCoordinates(), 0.05f);
    }
}
