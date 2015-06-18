using UnityEngine;
using System.Collections;
using Assets.TechTree;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.TechTree.Data;

public class NavPanel : MonoBehaviour {

    private GameObject originButton;



    private void AddButton(string text,string dataId){
        var b = Instantiate(originButton);
        var c = b.GetComponent<NavButton>();
        c.transform.SetParent(transform);
        c.Text = text;
        c.DataId = dataId;
    }

	// Use this for initialization
	void Start () {
        G.Instance.OnLoaded(g => {
            originButton = G.Instance.Prefabs["NavButton"];
            AddButton("科技", "tech");
            AddButton("物品", "item");
            AddButton("设施", "buld");
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
