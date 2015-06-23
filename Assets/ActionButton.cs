using UnityEngine;
using System.Collections;
using Assets.TechTree.Data;
using UnityEngine.UI;
using Assets.Layout;
using System;
using Assets.TechTree.Actions;

public class ActionButton : MonoBehaviour {
    public Text Text { get;private set; }
    public BaseAction Action { get; set; }
	// Use this for initialization
	void Start () {
        Text = this.FindByName<Text>("Text");
        Text.SetText(Action.Raw.GetInfo().name);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Init(BaseAction action) {
        Action = action;
    }

    public void OnClick() {
        Action.OnAction(0);
    }
}
