using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Layout;
using Assets.TechTree.Data;
using Assets.TechTree;
public class ActionPanel : MonoBehaviour {

    public Text Desc { get; set; }
    public RectTransform ActionButtonGroup { get; set; }
    public UBaseItem Data { get; set; }

    public ActionPanel() {
        
    }

	// Use this for initialization
	void Start () {
        G.Instance.ActionPanel = this;
        Desc = this.FindByName<Text>("ItemDesc");
        ActionButtonGroup = this.FindByName<RectTransform>("ActionButtonGroup");
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Apply(UBaseItem item){
        Data = item;
        Desc.SetText(Data.Raw.desc);
        foreach (Transform e in ActionButtonGroup) Destroy(e.gameObject);
        foreach (var e in Data.Raw.action) {
            var a=Instantiate(G.Instance.Prefabs["ActionButton"] as GameObject);
            e.UserData.Button=a.GetComponent<ActionButton>();
            e.UserData.Button.Target.Init(e.UserData);
            a.transform.SetParent(ActionButtonGroup);
        }
    }
}
