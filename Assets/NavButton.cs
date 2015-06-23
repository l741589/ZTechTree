using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Assets.TechTree;
using System.Collections.Generic;
using Assets.TechTree.Data;

public class NavButton : MonoBehaviour {

    private Text textView;
    public Text TextView {
        get {
            if (textView == null) textView = transform.GetComponentInChildren<Text>();
            return textView;
        }
    }

    // Use this for initialization
    void Start() {
        GetComponent<Toggle>().group = transform.parent.GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update() {

    }

    public String Text {
        get {
            return TextView.text;
        }
        set {
            TextView.text = value;
        }
    }

    public string DataId { get; set; }

    public void OnCheck(bool check) {
        if (check) {
            foreach (var e in G.Instance.fdata.AllItems) {
                var b = e.UserData;
                b.Visible = e.cateId == DataId;
            }
        }
    }
}
