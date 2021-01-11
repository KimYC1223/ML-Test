using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MummyAgent : Agent
{
    private Transform MummyTransform;
    private Rigidbody MummyRigidbody;
    private Material OriginMat;

    public Transform TargetTransform;

    public Renderer Floor;
    public Material GoodMat;
    public Material BadMat;

    private Vector3 MummyVelocity;

    public void FixedUpdate() {
        MummyRigidbody.velocity = MummyVelocity;
    }

    //초기화 작업을 위해 한번 호출되는 메소드
    public override void Initialize() {
        MummyTransform = this.GetComponent<Transform>();
        MummyRigidbody = this.GetComponent<Rigidbody>();
        OriginMat = Floor.material;
    }

    //에피소드(학습단위)가 시작할때마다 호출
    public override void OnEpisodeBegin() {
        MummyVelocity = Vector3.zero;
        MummyRigidbody.angularVelocity = Vector3.zero;

        MummyTransform.localPosition = new Vector3(Random.Range(-4.5f,4.5f),0f,Random.Range(-4.5f,4.5f));
        TargetTransform.localPosition = new Vector3(Random.Range(-4.5f,4.5f),0.5f,Random.Range(-4.5f,4.5f));
    }

    //환경 정보를 관측 및 수집해 정책 결정을 위해 브레인에 전달하는 메소드
    public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor) {
        sensor.AddObservation(MummyTransform.localPosition);
        sensor.AddObservation(TargetTransform.localPosition);
        sensor.AddObservation(MummyRigidbody.velocity.x);
        sensor.AddObservation(MummyRigidbody.velocity.z);
    }

    //브레인(정책)으로 부터 전달 받은 행동을 실행하는 메소드
    public override void OnActionReceived(float[] vectorAction) {
        float h = Mathf.Clamp(vectorAction[0], -1f, 1f);
        float v = Mathf.Clamp(vectorAction[1], -1f, 1f);

        Vector3 movement = Vector3.zero;
        movement.Set(-h, 0f, -v);
        movement = movement.normalized;
        MummyVelocity = movement*5f;

        if(h != 0 || v != 0)
            MummyRigidbody.rotation = Quaternion.Slerp(MummyRigidbody.rotation, Quaternion.LookRotation(movement),
                10f*Time.deltaTime);
        

        SetReward(-0.001f);
    }

    //개발자(사용자)가 직접 명령을 내릴때 호출하는 메소드(주로 테스트용도 또는 모방학습에 사용)
    public override void Heuristic(float[] actionsOut) {
        actionsOut[0] = Input.GetAxis("Horizontal"); //좌,우 화살표 키 //-1.0 ~ 0.0 ~ 1.0
        actionsOut[1] = Input.GetAxis("Vertical");   //상,하 화살표 키 //연속적인 값
        Debug.Log($"[0]={actionsOut[0]} [1]={actionsOut[1]}");
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("DeadZone")) {
            Floor.material = BadMat;
            StartCoroutine(ResetFloorMat());
            SetReward(-1f);
            EndEpisode();
        } else if(collision.collider.CompareTag("Target")) {
            Floor.material = GoodMat;
            StartCoroutine(ResetFloorMat());
            SetReward(1f);
            EndEpisode();
        }
    }

    IEnumerator ResetFloorMat() {
        yield return new WaitForSeconds(0.5f);
        Floor.material = OriginMat;
    }
}
