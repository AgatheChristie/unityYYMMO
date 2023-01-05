using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleCtrl2 : MonoBehaviour
{
    private Vector3 m_TargetPos = Vector3.zero;


    private CharacterController m_CharacterController;
    private float m_Speed = 10f;
    private float m_RotationSpeed = 0.2f;
    private Quaternion m_TargetQuaternion;

    private bool m_RotationOver = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        Debug.Log("aaaf:" + Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CharacterController == null)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log("aaaf");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.name.Equals("Ground", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    m_TargetPos = hitInfo.point;
                    m_RotationOver = false;
                    m_RotationSpeed = 0;
                }
            }
        }

        if (!m_CharacterController.isGrounded)
        {
            m_CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }

        if (m_TargetPos != Vector3.zero)
        {
            if (Vector3.Distance(m_TargetPos, transform.position) > 0.1f)
            {
                Vector3 direction = m_TargetPos - transform.position;
                direction = direction.normalized;
                direction = direction * Time.deltaTime * m_Speed;
                direction.y = 0;
                // transform.LookAt(new Vector3(m_TargetPos.x,transform.position.y,m_TargetPos.z));

                if (!m_RotationOver)
                {
                    m_RotationSpeed += 5f;
                    m_TargetQuaternion = Quaternion.LookRotation(direction);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, m_TargetQuaternion, Time.deltaTime * m_RotationSpeed);
                    if (Quaternion.Angle(m_TargetQuaternion, transform.rotation) < 1f)
                    {
                        m_RotationSpeed = 1;
                        m_RotationOver = true;
                    }
                }
                
                m_CharacterController.Move(direction);
            }
            // Debug.DrawLine(Camera.main.transform.position,m_TargetPos);
        }
    }
}