using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Layout;
using Assets.TechTree.Data;
public class ActionPanel : MonoBehaviour {

    public Text Desc { get; set; }
    public RectTransform ActionButtonGroup { get; set; }

	// Use this for initialization
	void Start () {
        Desc = this.FindByName<Text>("ItemDesc");
        ActionButtonGroup = this.FindByName<RectTransform>("ActionButtonGroup");
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Apply(BaseItem item){

    }
}
