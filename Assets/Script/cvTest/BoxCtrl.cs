using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCtrl : MonoBehaviour
{

    private Vector3 m_TargetPos = Vector3.zero;

    private float m_Speed  = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("aaaf:"+Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonUp(0))
        {
            // 单个物体
            // GameObject boxObj = GameObject.FindGameObjectWithTag("Box");
            // if (boxObj != null)
            // {
            //     boxObj.transform.localPosition = boxObj.transform.localPosition + new Vector3(0, 0, 10);
            // }
            //
            // 多个物体
            GameObject[] boxArr = GameObject.FindGameObjectsWithTag("Box");
            if (boxArr != null && boxArr.Length > 0)
            {
                for (int i = 0; i < boxArr.Length; i++)
                {
                    boxArr[i].transform.localPosition = boxArr[i].transform.localPosition + new Vector3(0, 0, 10);
                }
              
            }
            
        }
        return;


        if (Input.GetMouseButtonUp(0))
        {
           // Debug.Log("aaaf");

           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

           RaycastHit hitInfo;
           if (Physics.Raycast(ray,out hitInfo))
           {
               if (hitInfo.collider.gameObject.name.Equals("Ground",System.StringComparison.CurrentCultureIgnoreCase))
               {
                   m_TargetPos = hitInfo.point;
               }
           }
        }
        if (m_TargetPos != Vector3.zero)
        {

            if (Vector3.Distance(m_TargetPos,transform.position) > 0.1f)
            {
                transform.LookAt(m_TargetPos);
                transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
            }
           // Debug.DrawLine(Camera.main.transform.position,m_TargetPos);
          
        }
    }
}
