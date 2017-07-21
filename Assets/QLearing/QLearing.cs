using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jerry;

public class QLearing : SingletonMono<QLearing>
{
    private Text info;
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

    private List<Cell.CellInfo> cellInfos = new List<Cell.CellInfo>();
    private List<Cell> cells = new List<Cell>();
    public Cell GetCellByID(int id)
    {
        return cells.Find((x) => x.Info.id == id);
    }

    public override void Awake()
    {
        base.Awake();

        info = this.transform.FindChild("Text").GetComponent<Text>();
        prefab = this.transform.FindChild("Prefab");
        grid = this.transform.FindChild("GridMgr");

        cellInfos.Add(new Cell.CellInfo() { id = 0, pos = new Vector3(0, -250, 0) });
        cellInfos.Add(new Cell.CellInfo() { id = 1, pos = new Vector3(100, 0, 0) });
        //this.StartCoroutine(Episode());

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
    }

    private IEnumerator Episode()
    {
        while (true)
        {
            yield return StartCoroutine(OneEpisode());
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator OneEpisode()
    {
        info.text = "";
        for (int i = 0; i < 5; i++)
        {
            AddInfo(i);
            yield return new WaitForSeconds(1f);
        }
    }

    private void AddInfo(int id)
    {
        if (string.IsNullOrEmpty(info.text))
        {
            info.text += id.ToString();
        }
        else
        {
            info.text += string.Format("->{0}", id);
        }
    }
}