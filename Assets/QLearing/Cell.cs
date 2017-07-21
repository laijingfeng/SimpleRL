using UnityEngine;
using System.Collections;
using Jerry;

public class Cell : MonoBehaviour
{
    public Transform p1;
    public Transform p2;

    private CellInfo info;
    public CellInfo Info
    {
        get
        {
            return info;
        }
    }

    private bool awaked = false;
    private bool inited = false;

    void Awake()
    {
        p1 = this.transform.FindChild("p1");
        p2 = this.transform.FindChild("p2");
        awaked = true;
        TryWork();
    }

    public void SetInfo(CellInfo _info)
    {
        info = _info;
        inited = true;
        TryWork();
    }

    void Update()
    {

    }

    private void TryWork()
    {
        if (!awaked || !inited)
        {
            return;
        }
        this.transform.localPosition = info.pos;
    }

    [ContextMenu("xxx")]
    private void DoDraw()
    {
        if (info.id == 0)
        {
            JerryDrawer.Draw<DrawerElementPath>()
            .SetPoints(this.p1, QLearing.Inst.GetCellByID(1 - info.id).p1)
            .SetColor(Color.red);
        }
        else
        {
            JerryDrawer.Draw<DrawerElementPath>()
            .SetPoints(this.p2, QLearing.Inst.GetCellByID(1 - info.id).p2)
            .SetColor(Color.blue);
        }
    }

    public class CellInfo
    {
        public int id = 0;
        public Vector3 pos = Vector3.zero;
        public Color color = Color.white;
    }
}