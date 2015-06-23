using UnityEngine;
using System.Collections;
using Assets.TechTree.Data;
using UnityEngine.UI;
using Assets.TechTree;
using Assets.Layout;
using System;
using Assets.TechTree.Actions;
using Assets.Util;
using System.Collections.Generic;

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
        this.transform.SetParent(G.Instance.ListBox.Target.transform);
        DataUpdated();
    }

    private bool invalidate = false;

    public void DataUpdated() {
        invalidate = true;
    }


    // Update is called once per frame
    void Update() {
        if (invalidate) {
            if (Data != null && Text != null) {
                Text.text = Data.Raw.name;
                string time = null;
                string num = null;
                string tag = null;
                foreach (var e in Data.Raw.action) {
                    var a = e.UserData;
                    if (String.IsNullOrEmpty(time)) time = a.Time;
                    if (String.IsNullOrEmpty(num)) num = a.Num;
                    if (String.IsNullOrEmpty(tag)) tag = a.Tag;
                }
                Time.SetText(time);
                Num.SetText(num);
                Tag.SetText(tag);
            }
            invalidate = false;
        }
    }

    public void OnClick() {
        G.Instance.ActionPanel.Target.Apply(Data);
    }

   
}
