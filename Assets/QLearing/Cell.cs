using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Transform ld;
    public Transform lu;
    public Transform rd;
    public Transform ru;

    private Text text;
    private Image img;

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
        lu = this.transform.FindChild("lu");
        ld = this.transform.FindChild("ld");
        ru = this.transform.FindChild("ru");
        rd = this.transform.FindChild("rd");
        img = this.transform.GetComponent<Image>();
        text = this.transform.FindChild("Text").GetComponent<Text>();
        awaked = true;
        TryWork();
    }

    public Transform GetPoint(Transform next)
    {
        bool right = next.localPosition.x > this.transform.localPosition.x;
        bool up = next.localPosition.y > this.transform.localPosition.y;
        if (right && up)
        {
            return ru;
        }
        else if (right && !up)
        {
            return rd;
        }
        else if (!right && up)
        {
            return lu;
        }
        else
        {
            return ld;
        }
    }

    public void SetInfo(CellInfo _info)
    {
        info = _info;
        inited = true;
        TryWork();
    }

    public void SetColor(Color col)
    {
        img.color = col;
    }

    private void TryWork()
    {
        if (!awaked || !inited)
        {
            return;
        }
        this.transform.name = info.id.ToString();
        this.transform.localPosition = info.pos;
        text.text = string.Format("{0}", info.id);
    }

    public class CellInfo
    {
        public int id = 0;
        public Vector3 pos = Vector3.zero;
    }
}