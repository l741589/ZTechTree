using UnityEngine;
using System.Collections;
using Assets.TechTree.Data;
using UnityEngine.UI;
using Assets.Layout;
using System;
using Assets.TechTree.Actions;
using UnityEngine.EventSystems;

public class ActionButton : UIBehaviour {
    public Text Text { get;private set; }
    public BaseAction Action { get; set; }
    private bool invalidate = true;
	// Use this for initialization
	public virtual void Start () {
        Text = this.FindByName<Text>("Text");
        if (Action!=null) Text.SetText(Action.Raw.GetInfo().name);
	}
	
	// Update is called once per frame
	public void Update () {
        if (invalidate) {
            if (Action != null) 
            gameObject.SetActive(Action.Enabled);
            invalidate = false;
        }
	}

    public void Init(BaseAction action) {
        Action = action;
        this.gameObject.SetActive(action.Enabled);
    }

    public void OnClick() {
        Action.OnAction(0);
    }

    public void DataUpdated() {
        invalidate = true;
        if (!gameObject.activeSelf) Update();
    }
}
