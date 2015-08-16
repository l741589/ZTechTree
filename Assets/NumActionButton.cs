using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Layout;

public class NumActionButton : ActionButton {

    public Text Num { get; private set; }
    public Text TextAdd { get; private set; }
    public Text TextSub { get; private set; }
    public int max = 1000000000;
    public int min = 0;
    private int value;
    public int Value { get {
        return value;
        }
        set {
            this.value = value;
            if (this.value > max) this.value = max;
            if (this.value < min) this.value = min;
            Num.SetText(this.value.ToString());
        }
    }

    private int unit = 1;
    public int Unit {
        get {
            return unit;
        }
        set {
            unit = value;
            switch (unit) {
            case 1: SetUnitText("1"); break;
            case 10: SetUnitText("10"); break;
            case 100: SetUnitText("100"); break;
            case 1000: SetUnitText("10^3"); break;
            case 10000: SetUnitText("10^4"); break;
            case 100000: SetUnitText("10^5"); break;
            case 1000000: SetUnitText("10^6"); break;
            case 10000000: SetUnitText("10^7"); break;
            case 100000000: SetUnitText("10^8"); break;
            case 1000000000: SetUnitText("10^9"); break;
            }
        }
    }

    private void SetUnitText(string s) {
        TextAdd.SetText("+"+s);
        TextSub.SetText("-" + s);
    }
    
    public override void Start() {
        base.Start();
        Unit = 1;
        Num = this.FindByName<Text>("Num");
        TextAdd = this.FindByName<Text>("TextAdd");
        TextSub = this.FindByName<Text>("TextSub");
    }

    public void AddClick() {
        Value += Unit;
    }

    public void SubClick() {
        Value -= Unit;
    }

    public void DivClick() {
        if (Unit>=10) Unit /= 10;
    }

    public void MulClick() {
        if (max/10>=Unit) Unit *= 10;
    }
}
