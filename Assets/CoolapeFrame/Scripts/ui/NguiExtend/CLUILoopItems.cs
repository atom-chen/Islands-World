using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Coolape
{
	public class CLUILoopItems : UIWrapContent
	{
		public ArrayList list;
		public object initCellFunc;
		bool isInited = false;
		// Use this for initialization
		protected override void Start ()
		{
			if (isInited)
				return;
			isInited = true;
			onInitializeItem = initCell;
			base.Start ();
		}

		ArrayList wrapList (object data)
		{
			ArrayList _list = null;
			if (data is LuaTable) {
				_list = CLUtlLua.luaTableVals2List ((LuaTable)data);
			} else if (data is ArrayList) {
				_list = (ArrayList)data;
			} else if (data is object[]) {
				_list = new ArrayList ();
				_list.AddRange ((object[])data);
			}
			if (_list == null) {
				_list = new ArrayList ();
			}
			return _list;
		}

		public void setList (object dataList, object initCell)
		{
			Start ();
			this.list = wrapList (dataList);
			this.initCellFunc = initCell;
//			if (list != null && list.Count > 1) {
//				minIndex = 0;
//				maxIndex = list.Count;
//			}
			WrapContent ();
		}

		void initCell (GameObject go, int wrapIndex, int realIndex)
		{
			Debug.Log (wrapIndex + "========" + realIndex);
			if (initCellFunc != null && realIndex >= 0 && list != null && realIndex < list.Count) {
				Utl.doCallback (initCellFunc, go.GetComponent<CLCellBase> (), list [realIndex]);
			}
		}
	}
}
