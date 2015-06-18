using UnityEngine;
using System.Collections;
using Assets.TechTree.Data;
using System.Collections.Generic;
using Assets.TechTree;
using UnityEngine.UI;

public class ListBox : MonoBehaviour {


	void Start () {
        G.Instance.ListBox = this;
	}
	
	void Update () {
	
	}

    public ListBox() {
        if (G.Instance != null) {
            G.Instance.ListBox = this;
        }
    }
}
