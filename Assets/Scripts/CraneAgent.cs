using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CraneAgent : Agent
{
    public Transform Plane;
    public GameObject BoxPrefabs;
    public int sero = 15, garo = 15;
    private List<List<GameObject>> map;
    private List<List<float>> Type;

    public override void Initialize() {
        map = new List<List<GameObject>>();
        Type = new List<List<float>>();
        for (int i = 0; i < sero; i++) {
            List<GameObject> temp = new List<GameObject>();
            List<float> temp2 = new List<float>();
            for(int j = 0; j < garo; j ++) {
                GameObject box = Instantiate(BoxPrefabs, Plane);
                box.transform.localPosition
                    = new Vector3(-3.5f + ( 0.5f ) * j, 0.25f, -3.5f + ( 0.5f ) * i);
                temp.Add(box);
                temp2.Add(0f);
            }
            map.Add(temp);
            Type.Add(temp2);
        }
    }

    public override void OnEpisodeBegin() {
        
    }

    public override void CollectObservations(VectorSensor sensor) {
        
    }

    public override void OnActionReceived(float[] vectorAction) {
        
    }

    public override void Heuristic(float[] actionsOut) {
        
    }
}
