using System.Collections;
using System.Collections.Generic;
using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class QLearing : SingletonMono<QLearing>
{
    private Transform prefab;
    private Transform grid;

    public float[,] R = new float[6, 6] 
    {
        {-1, -1, -1, -1, 0, -1},
        {-1, -1, -1, 0, -1, 100},
        {-1, -1, -1, 0, -1, -1},
        {-1, 0, 0, -1, 0, -1},
        {0, -1, -1, 0, -1, 100},
        {-1, 0, -1, -1, 0, 100},
    };

    public float[,] Q = new float[6, 6] 
    {
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
    };

    private const int Len = 6;
    private const float learingRate = 0.8f;
    private List<Cell.CellInfo> cellInfos = new List<Cell.CellInfo>();
    private List<Cell> cells = new List<Cell>();
    public Cell GetCellByID(int id)
    {
        return cells.Find((x) => x.Info.id == id);
    }

    public override void Awake()
    {
        base.Awake();

        prefab = this.transform.FindChild("Prefab");
        grid = this.transform.FindChild("GridMgr");

        cellInfos.Add(new Cell.CellInfo() { id = 0, pos = new Vector3(-180, 298, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 1, pos = new Vector3(-35, 518, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 2, pos = new Vector3(-180, 408, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 3, pos = new Vector3(-35, 408, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 4, pos = new Vector3(-35, 298, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 5, pos = new Vector3(110, 408, 0) });

        for (int i = 0; i < cellInfos.Count; i++)
        {
            Cell cell = JerryUtil.CloneGo<Cell>(new JerryUtil.CloneGoData()
            {
                parant = grid,
                prefab = prefab.gameObject,
                active = true,
                clean = false,
            });
            cell.SetInfo(cellInfos[i]);
            cells.Add(cell);
        }

        for (int i = 0; i < Len; i++)
        {
            for (int j = 0; j < Len; j++)
            {
                if (R[i, j] >= 0 && i != j)
                {
                    Draw(i, j);
                }
            }
        }

        this.StartCoroutine(Episode());
    }

    private void Draw(int a, int b)
    {
        Cell cellA = GetCellByID(a);
        if (cellA == null)
        {
            return;
        }
        Cell cellB = GetCellByID(b);
        if (cellB == null)
        {
            return;
        }

        DrawPoint dp = GetDrawPoint(cellA, cellB);
        JerryDrawer.Draw<DrawerElementPath>()
            .SetPoints(dp.a, dp.b)
            .SetColor(dp.c);
        JerryDrawer.Draw<DrawerElementLabel>()
            .SetID((a * 100 + b).ToString())
            .SetColor(Color.green)
            .SetPos((dp.a.position + dp.b.position) * 0.5f + new Vector3(0f, 0.08f, 0))
            .SetText(Q[a, b].ToString());
    }

    private void RefreshText(int a, int b)
    {
        DrawerElementLabel d = JerryDrawer.GetElement<DrawerElementLabel>((a * 100 + b).ToString());
        if (d != null)
        {
            d.SetText(Q[a, b].ToString());
        }
    }

    private DrawPoint GetDrawPoint(Cell a, Cell b)
    {
        DrawPoint ret = new DrawPoint();
        if (a.rd.position.x < b.ld.position.x)//左右
        {
            ret.a = a.ru;
            ret.b = b.lu;
            ret.c = Color.red;
        }
        else if (a.ld.position.x > b.rd.position.x)//右左
        {
            ret.a = a.ld;
            ret.b = b.rd;
            ret.c = Color.blue;
        }
        else if (a.lu.position.y < b.ld.position.y)//下上
        {
            ret.a = a.lu;
            ret.b = b.ld;
            ret.c = Color.red;
        }
        else//上下
        {
            ret.a = a.rd;
            ret.b = b.ru;
            ret.c = Color.blue;
        }
        return ret;
    }

    private IEnumerator Episode()
    {
        while (true)
        {
            yield return StartCoroutine(OneEpisode(Random.Range(0, 6)));
            for (int i = 0; i < Len; i++)
            {
                GetCellByID(i).SetColor(Color.white);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator OneEpisode(int id)
    {
        GetCellByID(id).SetColor(Color.yellow);
        yield return new WaitForSeconds(1f);
        while (id != 5)
        {
            List<int> ids = new List<int>();
            List<float> vals = new List<float>();
            for (int i = 0; i < Len; i++)
            {
                if (R[id, i] >= 0)
                {
                    ids.Add(i);
                    vals.Add(Q[id, i]);
                }
            }
            int idx = Util.SoftRandom(vals.ToArray());
            int nid = ids[idx];

            //Debug.LogWarning("============ " + id.ToString() + " cnt:" + ids.Count + " select:" + idx);

            Q[id, nid] = R[id, nid] + learingRate * GetMax(nid);
            RefreshText(id, nid);
            GetCellByID(id).SetColor(Color.gray);
            id = nid;
            GetCellByID(id).SetColor(Color.yellow);
            yield return new WaitForSeconds(1f);
        }
    }

    private float GetMax(int id)
    {
        float val = 0;
        for (int i = 0; i < Len; i++)
        {
            if (R[id, i] >= 0)
            {
                if (Q[id, i] > val)
                {
                    val = Q[id, i];
                }
            }
        }
        return val;
    }

    public class DrawPoint
    {
        public Transform a;
        public Transform b;
        public Color c;
    }
}