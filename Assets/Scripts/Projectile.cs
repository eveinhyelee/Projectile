using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject fabCannon;
    public GameObject cursor;
    public GameObject Cannon;
    public Transform trsShootPoint;
    private Camera cam;
    public LayerMask layer;

    public float ratio = 0.5f;
    public float time = 2.0f;

    private void Awake()
    {
        cam =Camera.main;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        checkShootPoint(); 
    }

    private void checkShootPoint()
    {
        Ray camray = cam.ScreenPointToRay(Input.mousePosition); //마우스 포지션값
        if (Physics.Raycast(camray, out RaycastHit hit, 20f, layer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point;

            Vector3 Vo = calculateProjectileData(hit.point, Cannon.transform.position, time);

            Cannon.transform.rotation = Quaternion.LookRotation(Vo);

            if (Input.GetMouseButtonDown(0))
            {
                GameObject obj = Instantiate(fabCannon, trsShootPoint.position, Quaternion.identity);
                Rigidbody rigid = obj.GetComponent<Rigidbody>();
                rigid.velocity = Vo;
            }
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    private Vector3 calculateProjectileData(Vector3 _target, Vector3 _origin, float _time) //포물선의 수학적공식
    {
        Vector3 distance = _target - _origin;//이동해야할 거리
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude; //힘

        float Vxz = Sxz / _time; //속도
        float Vy = Sy / _time + ratio * Mathf.Abs(Physics.gravity.y)* _time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
