using UnityEngine;
using System.Collections;
using Assets.TechTree.Data;
using UnityEngine.UI;
using Assets.TechTree;
using Assets.Layout;
using System;

public class ItemButton : MonoBehaviour {

    private UBaseItem data;
    public Text Text { get; private set; }
    public Text Time { get; private set; }
    public Text Tag { get; private set; }
    public Text Num { get; private set; }
    public UBaseItem Data {
        get { return data; }
        set {
            data = value;
            DataUpdated();
        }
    }

    // Use this for initialization
    void Start() {
        var cs = GetComponentsInChildren<Text>();
        foreach (var c in cs) {
            switch (c.name) {
            case "Text": Text = c as Text; break;
            case "Time": Time = c as Text; break;
            case "Tag": Tag = c as Text; break;
            case "Num": Num = c as Text; break;
            }
        }
        Time.gameObject.SetActive(false);
        Tag.gameObject.SetActive(false);
        Num.gameObject.SetActive(false);
        GetComponent<Button>().onClick.AddListener(OnClick);
        this.transform.SetParent(G.Instance.ListBox.transform);
        DataUpdated();
    }

    public void DataUpdated() {
        if (Data != null && Text != null) {
            Text.text = Data.Raw.name;
            if (Data.EndTime != null) {
                CountDown();
            }
            if (Data is UTech) {
                var tech = data as UTech;
                Tag.SetText(tech.Studied ? "" : "NEW!");
            } else if (Data is UItem) {
                var item = data as UItem;
                Num.SetText("*" + item.Count);
            }
        }
    }


    private void setTime(TimeSpan? time) {
        if (!Time.IsDestroyed()) {
            if (time ==null) {
                Time.SetText(null);
            } else {
                Time.SetText(time.Value.Hours + ":" + time.Value.Minutes + ":" + time.Value.Seconds);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnClick() {
        if (Data.EndTime != null) return;
        if (Data is UTech && (Data as UTech).Studied) return;
        if (Data is UItem && !Pay()) return;
        Data.EndTime = DateTime.UtcNow.AddMilliseconds(Data.Raw.time);
        CountDown();
    }

    public void TimeDone() {
        Data.EndTime = null;
        setTime(null);
        if (Data is UTech) {
            var t = Data as UTech;
            t.Studied = true;
            Tag.SetText(null);
        } else if (Data is UItem) {
            var t = Data as UItem;
            ++t.Count;
            Num.SetText("*" + t.Count);
        } else {
            throw new NotImplementedException();
        }
        UpdateReverseDependences();
    }

    public void UpdateReverseDependences() {
        foreach (var e in Data.ReverseDependences) {
            e.CanShow = e.Raw.cond.Can();
        }
    }

    public void UpdateDependences() {
        foreach (var e in Data.Dependences) {
            if (e.Button != null) e.Button.DataUpdated();
        }
    }

    public bool Pay() {
        if (!Data.Raw.Pay()) return false;
        UpdateDependences();
        return true;
    }

    public void CountDown() {
        Scheduler.Loop(() => {
            if (Data.EndTime != null) {
                var span = Data.EndTime.Value.Subtract(DateTime.UtcNow);
                //Debug.Log(span);
                if (span.Ticks > 0) {
                    setTime(span);
                } else {
                    TimeDone();
                    return -1;
                }
                return 0.5f;
            } else {
                setTime(null);
            }
            return -1;
        });
    }
}
