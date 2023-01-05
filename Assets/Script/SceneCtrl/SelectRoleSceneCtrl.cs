using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoleSceneCtrl : MonoBehaviour
{
    private Dictionary<int, GameObject> m_JobObjectDic = new Dictionary<int, GameObject>();

    //  private List<JobEntity> m_JobList;
    private List<JobEntity> m_JobList;


    public Transform[] CreateRoleContainers;

    [SerializeField] private Transform[] CreateRoleSceneModel;

    private Dictionary<int, RoleCtrl> m_JobRoleCtrl;
    private UISceneSelectRoleView m_UISceneSelectRoleView;

    [SerializeField] private Transform DragTarget;
    [SerializeField] private float m_RotateSpeed = 100f;

    private GameObject m_CurrSelectRoleModel;
    private int m_CurrSelectRoleId;

    private float m_RotateAngle = 90;
    private float m_TargetAngle = 0;
    private bool m_IsRotateing = false;
    private List<GameObject> m_CloneCreatRoleList = new List<GameObject>();
    private int m_CurrSelectJobId;

    private bool m_IsCreateRole;

    private void Awake()
    {
        m_UISceneSelectRoleView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.SelectRole)
            .GetComponent<UISceneSelectRoleView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DelegateDefine.Instance.OnSceneLoadOk != null)
        {
            DelegateDefine.Instance.OnSceneLoadOk();
        }

        if (m_UISceneSelectRoleView != null)
        {
            m_UISceneSelectRoleView.SelectRoleDragView.OnSelectRoleDrag = OnSelectRoleDrag;

            if (m_UISceneSelectRoleView.JobItems != null && m_UISceneSelectRoleView.JobItems.Length > 0)
            {
                for (int i = 0; i < m_UISceneSelectRoleView.JobItems.Length; i++)
                {
                    m_UISceneSelectRoleView.JobItems[i].OnSelectJob = OnSelectJobCallBack;
                }
            }
        }

        //服务器返回  登录消息(角色列表消息)
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Get_Role_List_C2S, OnLogOnGameServerReturn);

        //服务器返回  创建角色消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Create_Role_C2S, OnCreateRoleReturn);
        //服务器返回  进入游戏消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Enter_Game_C2S, OnEnterGameReturn);
        //服务器返回  删除角色消息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Delete_Role_C2S, OnDeleteRoleReturn);
        //服务器返回  本角色角色信息
        SocketDispatcher.Instance.AddEventListener(ProtoCodeDef.Role_Myself_Info_C2S, OnMyselfInfoReturn);

        m_UISceneSelectRoleView.OnBtnBeginGameClick = OnBtnBeginGameClick;
        m_UISceneSelectRoleView.OnBtnDeleteRoleClick = OnBtnDeleteRoleClick;
        m_UISceneSelectRoleView.OnBtnReturnClick = OnBtnReturnClick;
        m_UISceneSelectRoleView.OnBtnCreateRoleClick = OnBtnCreateRoleClick;


        // 加载角色镜像
        LoadJobObject();
        // 请求角色列表
        LogOnGameServer();
    }


    private void ClearCloneCreatRole()
    { 
        if (m_CloneCreatRoleList != null && m_CloneCreatRoleList.Count > 0)
        {
            for (int i = 0; i < m_CloneCreatRoleList.Count; i++)
            {
                Destroy(m_CloneCreatRoleList[i]);
            }

            m_CloneCreatRoleList.Clear();
        }
    }

    private void OnBtnCreateRoleClick()
    {
        ToCreateRoleUI();
    }

    private void OnBtnReturnClick()
    {
        //如果是新建角色界面 并且当前没有角色 返回选区场景
        //如果是新建角色界面 有角色 返回已有角色界面
        //如果是已有角色界面 则返回选区场景
        if (m_IsCreateRole)
        {
            if (m_RoleList == null || m_RoleList.Count == 0)
            {
                NetWorkSocket.Instance.DisConnect();
                SceneMgr.Instance.LoadToLogOn();
            }
            else
            {
                // 并且当前有角色
                // 清除新建角色时候的模型 
                ClearCloneCreatRole();
                m_CurrSelectRoleId = 0;
                DragTarget.eulerAngles = Vector3.up * 0;

                //返回已有角色界面
                m_IsCreateRole = false;
                SetCreateRoleSceneModelActive(false);
                m_UISceneSelectRoleView.SetUICreateRoleActive(false);
                m_UISceneSelectRoleView.SetUISelectRoleActive(true);

                //选择已有角色
                m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
                SetSelectRole(m_RoleList[0].RoleId);
            }
        }
        else
        {
            NetWorkSocket.Instance.DisConnect();
            SceneMgr.Instance.LoadToLogOn();
        }
    }

    public void SetCreateRoleSceneModelActive(bool isActive)
    {
        GameObjectUtil.QSetTransformActive(CreateRoleSceneModel, isActive);
        // if (CreateRoleSceneModel != null && CreateRoleSceneModel.Length > 0)
        // {
        //     for (int i = 0; i < CreateRoleSceneModel.Length; i++)
        //     {
        //         CreateRoleSceneModel[i].gameObject.SetActive(isActive);
        //     }
        // }
    }

    private void SetSelectJob()
    {
        for (int i = 0; i < m_JobList.Count; i++)
        {
            if (m_JobList[i].Id == m_CurrSelectJobId)
            {
                m_UISceneSelectRoleView.SelectRoleJobDescView.SetUI(m_JobList[i].Name, m_JobList[i].Desc);
                break;
            }
        }

        for (int i = 0; i < m_UISceneSelectRoleView.JobItems.Length; i++)
        {
            m_UISceneSelectRoleView.JobItems[i].SetSelected(m_CurrSelectJobId);
        }
    }

    private void OnSelectJobCallBack(int jobId, int rotateAngle)
    {
        if (m_IsRotateing)
        {
            return;
        }

        m_CurrSelectJobId = jobId;
        SetSelectJob();
        m_IsRotateing = true;
        m_TargetAngle = rotateAngle;
    }

    private void OnSelectRoleDrag(int obj)
    {
        if (m_IsRotateing)
        {
            return;
        }

        m_RotateAngle = Mathf.Abs(m_RotateAngle) * (obj == 0 ? -1 : 1);
        m_IsRotateing = true;
        m_TargetAngle = DragTarget.eulerAngles.y + m_RotateAngle;

        if (obj == 1)
        {
            m_CurrSelectJobId--;
            if (m_CurrSelectJobId <= 0)
            {
                m_CurrSelectJobId = 4;
            }
        }
        else
        {
            m_CurrSelectJobId++;
            if (m_CurrSelectJobId > 4)
            {
                m_CurrSelectJobId = 1;
            }
        }

        SetSelectJob();
    }

    private void Update()
    {
        if (m_IsRotateing)
        {
            float toAngle =
                Mathf.MoveTowardsAngle(
                    DragTarget.eulerAngles.y,
                    m_TargetAngle,
                    Time.deltaTime * m_RotateSpeed);
            DragTarget.eulerAngles = Vector3.up * toAngle;
            // DragTarget.eulerAngles = new Vector3(90, toAngle, 0);


            if (Mathf.RoundToInt(m_TargetAngle) == Mathf.RoundToInt(toAngle) ||
                Mathf.RoundToInt(m_TargetAngle + 360) == Mathf.RoundToInt(toAngle))
            {
                m_IsRotateing = false;
                DragTarget.eulerAngles = Vector3.up * m_TargetAngle;
                // DragTarget.eulerAngles = new Vector3(90, m_TargetAngle, 0);
            }
        }
    }

    private void LoadJobObject()
    {
        m_JobList = JobDBModel.Instance.GetList();
        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject obj = Resources.Load(string.Format("CVMyPrefab/{0}", m_JobList[i].PrefabName)) as GameObject;
            if (obj != null)
            {
                m_JobObjectDic[m_JobList[i].Id] = obj;
            }
        }
    }

    #region 设置角色的Prefab

    private void CloneCreatRole()
    {
        if (CreateRoleContainers == null || CreateRoleContainers.Length < 4)
        {
            return;
        }

        ClearCloneCreatRole();

        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject objRole = Instantiate(m_JobObjectDic[m_JobList[i].Id]);
            objRole.transform.parent = CreateRoleContainers[i];

            objRole.transform.localScale = Vector3.one;
            objRole.transform.localPosition = Vector3.zero;
            objRole.transform.localRotation = Quaternion.Euler(Vector3.zero);

            m_CloneCreatRoleList.Add(objRole);

            RoleCtrl roleCtrl = objRole.GetComponent<RoleCtrl>();
            if (roleCtrl != null)
            {
                m_JobRoleCtrl[m_JobList[i].Id] = roleCtrl;
            }
            else
            {
                Debug.Log("roleCtrl == null");
            }
        }
    }

    #endregion


    #region 触发请求角色列表和请求角色列表服务器回调

    private void LogOnGameServer()
    {
        Role_Get_Role_List_C2SProto proto = new Role_Get_Role_List_C2SProto();

        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void OnLogOnGameServerReturn(byte[] p)
    {
        Role_Get_Role_List_S2CProto proto = Role_Get_Role_List_S2CProto.GetProto(p);

        ushort roleCount = proto.RoleCount;
        Debug.Log("roleCount:" + roleCount);

        if (roleCount == 0)
        {
            //新建角色
            m_IsCreateRole = true;
            SetCreateRoleSceneModelActive(true);
            m_UISceneSelectRoleView.SetUICreateRoleActive(true);
            m_UISceneSelectRoleView.SetUISelectRoleActive(false);

            CloneCreatRole();

            //初始化的时候 当前职业Id是1
            m_CurrSelectJobId = 1;
            SetSelectJob();
            m_UISceneSelectRoleView.RandomName();
        }
        else
        {
            m_IsCreateRole = false;
            SetCreateRoleSceneModelActive(false);
            m_UISceneSelectRoleView.SetUICreateRoleActive(false);
            m_UISceneSelectRoleView.SetUISelectRoleActive(true);
            if (proto.RoleList != null)
            {
                m_RoleList = proto.RoleList;
                m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
                SetSelectRole(m_RoleList[0].RoleId);
            }
        }
    }

    #endregion


    #region 触发创建角色和创建角色服务器回调

    private void OnBtnBeginGameClick()
    {
        if (m_IsCreateRole)
        {
            Role_Create_Role_C2SProto proto = new Role_Create_Role_C2SProto();
            proto.Realm = 1;
            proto.Career = (byte)m_CurrSelectJobId;
            proto.Sex = 1;
            proto.NickName = m_UISceneSelectRoleView.txtNickName.text;

            if (string.IsNullOrEmpty(proto.NickName))
            {
                MessageCtrl.Instance.Show("提示", "请输入您的昵称");
                return;
            }

            NetWorkSocket.Instance.SendMsg(proto.ToArray());
        }
        else
        {
            Role_Enter_Game_C2SProto proto = new Role_Enter_Game_C2SProto();
            proto.RoleId = m_CurrSelectRoleId;
            NetWorkSocket.Instance.SendMsg(proto.ToArray());
        }
    }

    private void OnCreateRoleReturn(byte[] p)
    {
        Role_Create_Role_S2CProto proto = Role_Create_Role_S2CProto.GetProto(p);

        //   ushort isSuccess = proto.IsSuccess;
        //  int playerId = proto.PlayerId;
        if (proto.IsSuccess == 1)
        {
            // todo 创建真正的角色 跳转场景
            Debug.Log("创建角色成功");
        }
        else if (proto.IsSuccess == 5)
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败:角色名称长度为1~5个汉字");
        }
        else if (proto.IsSuccess == 0)
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败:失败");
        }
        else if (proto.IsSuccess == 3)
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败:角色名称已经被使用");
        }
        else if (proto.IsSuccess == 2)
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败:未知错误");
        }
        else if (proto.IsSuccess == 6)
        {
            MessageCtrl.Instance.Show("提示", "创建角色失败:AccName已经被使用");
        }
    }

    #endregion

    //=======================角色信息相关=================

    #region 角色信息相关

    public void OnMyselfInfo()
    {
        Role_Myself_Info_C2SProto proto = new Role_Myself_Info_C2SProto();
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void OnMyselfInfoReturn(byte[] p)
    {
        Role_Myself_Info_S2CProto proto = Role_Myself_Info_S2CProto.GetProto(p);

        if (proto.Id == m_CurrSelectRoleId)
        {
            Debug.Log("请求角色信息成功");
            SceneMgr.Instance.LoadToCity();
        }
        else
        {
            MessageCtrl.Instance.Show("提示", "请求角色信息失败");
        }
    }

    #endregion

    //=======================删除角色相关=================

    #region 删除角色相关

    private void OnBtnDeleteRoleClick()
    {
        m_UISceneSelectRoleView.DeleteSelectRole(GetRoleItem(m_CurrSelectRoleId).RoleNickName,
            OnDeleteRoleClickCallBack);
    }

    private void OnDeleteRoleClickCallBack()
    {
        Role_Delete_Role_C2SProto proto = new Role_Delete_Role_C2SProto();
        proto.RoleId = m_CurrSelectRoleId;
        NetWorkSocket.Instance.SendMsg(proto.ToArray());
    }

    private void OnDeleteRoleReturn(byte[] p)
    {
        Role_Delete_Role_S2CProto proto = Role_Delete_Role_S2CProto.GetProto(p);

        if (proto.IsSuccess == 1)
        {
            Debug.Log("删除角色成功");
            m_UISceneSelectRoleView.CloseDeleteRoleView();
            DeleteRole(m_CurrSelectRoleId);
        }
        else if (proto.IsSuccess == 0)
        {
            MessageCtrl.Instance.Show("提示", "删除角色失败");
        }
    }

    private void DeleteRole(int roleId)
    {
        for (int i = m_RoleList.Count - 1; i >= 0; i--)
        {
            if (m_RoleList[i].RoleId == roleId)
            {
                m_RoleList.RemoveAt(i);
            }
        }

        if (m_RoleList.Count == 0)
        {
            ToCreateRoleUI();
        }
        else
        {
            m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
            SetSelectRole(m_RoleList[0].RoleId);
        }
    }

    #endregion

    private void ToCreateRoleUI()
    {
        m_UISceneSelectRoleView.ClearRoleListUI();
        //删除角色模型
        if (m_CurrSelectRoleModel != null)
        {
            Destroy(m_CurrSelectRoleModel);
        }

        //新建角色
        m_IsCreateRole = true;
        SetCreateRoleSceneModelActive(true);
        m_UISceneSelectRoleView.SetUICreateRoleActive(true);
        m_UISceneSelectRoleView.SetUISelectRoleActive(false);


        CloneCreatRole();
        //初始化的时候 当前职业Id是1
        m_CurrSelectJobId = 1;
        SetSelectJob();
        m_UISceneSelectRoleView.RandomName();
    }

    //=======================进入游戏相关=================

    #region 进入游戏相关

    private void OnEnterGameReturn(byte[] p)
    {
        Role_Enter_Game_S2CProto proto = Role_Enter_Game_S2CProto.GetProto(p);

        if (proto.IsSuccess == 1)
        {
            // todo 创建真正的角色 跳转场景
            Debug.Log("进入游戏成功");
            // 发送请求本角色信息协议
            OnMyselfInfo();
        }
        else if (proto.IsSuccess == 0)
        {
            MessageCtrl.Instance.Show("提示", "进入游戏失败");
        }
    }

    #endregion

    //=======================已有角色相关=================

    #region 已有角色相关

    private List<Role_Get_Role_List_S2CProto.RoleItem> m_RoleList;

    private Role_Get_Role_List_S2CProto.RoleItem GetRoleItem(int roleId)
    {
        if (m_RoleList != null)
        {
            for (int i = 0; i < m_RoleList.Count; i++)
            {
                if (m_RoleList[i].RoleId == roleId)
                {
                    return m_RoleList[i];
                }
            }
        }

        return default(Role_Get_Role_List_S2CProto.RoleItem);
    }

    //把角色的prefab展示到摄像机前面
    private void SetSelectRole(int roleId)
    {
        if (m_CurrSelectRoleId == roleId)
        {
            return;
        }

        m_CurrSelectRoleId = roleId;

        if (m_CurrSelectRoleModel != null)
        {
            Destroy(m_CurrSelectRoleModel);
        }

        Role_Get_Role_List_S2CProto.RoleItem item = GetRoleItem(roleId);

        if (CreateRoleContainers == null || CreateRoleContainers.Length < 4)
        {
            return;
        }

        m_CurrSelectRoleModel = Instantiate(m_JobObjectDic[item.RoleCareer]);
        m_CurrSelectRoleModel.transform.parent = CreateRoleContainers[0];

        m_CurrSelectRoleModel.transform.localScale = Vector3.one;
        m_CurrSelectRoleModel.transform.localPosition = Vector3.zero;
        m_CurrSelectRoleModel.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RoleCtrl roleCtrl = m_CurrSelectRoleModel.GetComponent<RoleCtrl>();
    }

    private void SelectRoleCallBack(int roleId)
    {
        SetSelectRole(roleId);
    }

    #endregion

    private void OnDestroy()
    {
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Role_Get_Role_List_C2S, OnLogOnGameServerReturn);
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Role_Create_Role_C2S, OnCreateRoleReturn);
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Role_Enter_Game_C2S, OnEnterGameReturn);
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Role_Delete_Role_C2S, OnDeleteRoleReturn);
        SocketDispatcher.Instance.RemoveEventListener(ProtoCodeDef.Role_Myself_Info_C2S, OnMyselfInfoReturn);
    }
}