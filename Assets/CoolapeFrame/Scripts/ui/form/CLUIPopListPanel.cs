using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Coolape;

public class CLUIPopListPanel : CLPanelLua {
	public CLUILoopGrid grid;

	public List<string> items = new List<string>();
	public ArrayList valueItems = new ArrayList();
	public object callback;
	public string selectedItem = "";
	public static void show(ArrayList items, ArrayList valueItems, object callback) {
		show(wrapItems(items),  valueItems,  callback);
	}
	public static void show(List<string> items, ArrayList valueItems, object callback) {
		ArrayList orgs = new ArrayList();
		orgs.Add(items);
		orgs.Add(valueItems);
		orgs.Add(callback);
		CLPanelManager.getPanelAsy("PanelPopList", (Callback)onGetPanel, orgs);
	}

	public static void onGetPanel(params object[] obj) {
		CLUIPopListPanel p = (CLUIPopListPanel)(obj[0]);
		ArrayList orgs = (ArrayList)(obj[1]);
		p.items = ((List<string>)(orgs[0]));
		p.valueItems = (ArrayList)(orgs[1]);
		p.callback = orgs[2];
		CLPanelManager.showTopPanel(p, true, true);
	}

	public static List<string> wrapItems(ArrayList list) {
		List<string> items = new List<string>();
		int count = list.Count;
		for(int i=0; i < count; i++) {
			items.Add(list[i].ToString());
		}
		return items;
	}

	public override void show ()
	{
		base.show ();
		grid.setList(items.ToArray(), (Callback)initCell);
	}
	void initCell(params object[] obj) {
		CLCellBase cell = (CLCellBase)(obj[0]);
		object data = obj[1];
		cell.init(data, (Callback)onClickCell);
	}

	void onClickCell(params object[] orgs) {
		CLCellLua cell = (CLCellLua)(orgs[0]);
		object[] ret = call(cell.getLuaFunction("getData"));
		if(ret != null && ret.Length > 0) {
			selectedItem = ret[0].ToString();		
		}
		
		CLPanelManager.hideTopPanel();
		Utl.doCallback (callback, value, selectedItem);
	}
	
	public object value
	{
		get
		{
			if(valueItems.Count == items.Count) {
				int index = items.IndexOf(selectedItem);
				if(index >= 0) {
					return valueItems[index];
				}
			}
			return selectedItem;
		}
		set
		{
			selectedItem = value.ToString();
		}
	}

	public void OnClickClose(GameObject go) {
		CLPanelManager.hideTopPanel();
	}

}
