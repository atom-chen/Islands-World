/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  管理代理回调，目的是为了先把回调根据某个k管理起来，然后调用时方便取得
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class CLDelegate
	{
		public Hashtable delegateInfro = new Hashtable ();

		public void add (string key, object callback, object orgs)
		{
			ArrayList list = MapEx.getList (delegateInfro, key);
			if (list == null) {
				list = ObjPool.listPool.borrowObject();
            }
            NewList infor = ObjPool.listPool.borrowObject();
			infor.Add (callback);
			infor.Add (orgs);
			list.Add (infor);
			delegateInfro [key] = list;
		}

        public void remove(string key, object callback) {
            ArrayList list = MapEx.getList(delegateInfro, key);
            if (list == null)
            {
                return;
            }

            NewList cell = null;
            while(list.Count > 0) {
                cell = (list[list.Count - 1]) as NewList;
                if(cell[0] == null || cell[0].Equals(callback)) {
                    ObjPool.listPool.returnObject(cell);
                    list.RemoveAt(list.Count - 1);
                }
            }
        }

		public void removeDelegates (string key)
		{
			if (delegateInfro [key] != null) {
                NewList list = (delegateInfro[key]) as NewList;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ObjPool.listPool.returnObject(list[i] as NewList);
                    }
                    list.Clear();
                    ObjPool.listPool.returnObject(list);
                    list = null;
                }
			}
			delegateInfro.Remove (key);
		}

		public ArrayList getDelegates (string key)
		{
			return MapEx.getList (delegateInfro, key);
		}
	}
}