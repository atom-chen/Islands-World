using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hivemind;
using Coolape;

public class DataExporter
{

	[ExportHiveData]
	public static void exportEventCfg (List<Node> nodes)
	{
		ArrayList list = new ArrayList ();
		ArrayList cell = new ArrayList ();
		Node node = null;
		PStr pstr = PStr.b ();

		for (int i = 0; i < nodes.Count; i++) {
			node = nodes [i];
			if (node is Root) {
				continue;
			}
			pstr.a (node.index);
			pstr.a ("\t");
			if (node.behaviorTree.rootNode.attr != null) {
				pstr.a (((MyRoot)(node.behaviorTree.rootNode.attr)).fubenID);
			} else {
				pstr.a (0);
			}
			pstr.a ("\t");
			pstr.a (node.condition);
			pstr.a ("\t");
			if (node is NodeAction) {
				if (((NodeAction)node).action != null) {
					MyAction action = ((NodeAction)node).action as MyAction;
					pstr.a (action.triggerType);
					pstr.a ("\t");
					pstr.a (action.hideSelf.ToString ().ToLower ());
					pstr.a ("\t");
					pstr.a (action.talkId);
					pstr.a ("\t");
					pstr.a (action.npcCfg);
					pstr.a ("\t");
					pstr.a (action.boxCfg);
					pstr.a ("\t");
				} else {
					pstr.a (0);
					pstr.a ("\t");
					pstr.a ("false");
					pstr.a ("\t");
					pstr.a (0);
					pstr.a ("\t");
					pstr.a ("");
					pstr.a ("\t");
					pstr.a (0);
					pstr.a ("\t");
				}
			} else {
				pstr.a (0);
				pstr.a ("\t");
				pstr.a ("false");
				pstr.a ("\t");
				pstr.a (0);
				pstr.a ("\t");
				pstr.a ("");
				pstr.a ("\t");
				pstr.a (0);
				pstr.a ("\t");
			}
			pstr.a (node.result2);
			pstr.a ("\t");
			pstr.a (node.result1);
			pstr.a ("\t");
			pstr.a ("");

			pstr.a ("\n");
		}
		Debug.Log (pstr.e ());
	}
}
