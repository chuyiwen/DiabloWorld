using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapItemParent : MonoBehaviour {
	
	public MapItem mapItem;
	
	private Vector3[] mapItemPos = { 
		new Vector3(100, -175, 0), 
		new Vector3(100, 20, 0),
		new Vector3(-100, 150, 0)
	};
	
	public MapItem[] Init (int iStart, int iEnd) {
		if (iEnd - iStart == 1) {
			if (iStart == 0) {
				MapItem firstItem = (MapItem)GameObject.Instantiate(mapItem);
				firstItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart], iStart);
				NGUIUtility.SetParent(transform, firstItem.transform);
				firstItem.transform.localPosition = mapItemPos[1];
				
				MapItem secondItem = (MapItem)GameObject.Instantiate(mapItem);
				secondItem.SetData(Globals.It.MapJsonMgr.MapJsons[iEnd], iEnd);
				NGUIUtility.SetParent(transform, secondItem.transform);
				secondItem.transform.localPosition = mapItemPos[2];
				
				return new MapItem[] { firstItem, secondItem };
			}
			else{
				MapItem firstItem = (MapItem)GameObject.Instantiate(mapItem);
				firstItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart], iStart);
				NGUIUtility.SetParent(transform, firstItem.transform);
				firstItem.transform.localPosition = mapItemPos[0];
				
				MapItem secondItem = (MapItem)GameObject.Instantiate(mapItem);
				secondItem.SetData(Globals.It.MapJsonMgr.MapJsons[iEnd], iEnd);
				NGUIUtility.SetParent(transform, secondItem.transform);
				secondItem.transform.localPosition = mapItemPos[1];
				return new MapItem[] { firstItem, secondItem };
			}
		}
		else {
			if (iStart == iEnd) {
				MapItem firstItem = (MapItem)GameObject.Instantiate(mapItem);
				firstItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart], iStart);
				NGUIUtility.SetParent(transform, firstItem.transform);
				firstItem.transform.localPosition = mapItemPos[0];
				return new MapItem[] { firstItem };
			}
			else{
				MapItem firstItem = (MapItem)GameObject.Instantiate(mapItem);
				firstItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart], iStart);
				NGUIUtility.SetParent(transform, firstItem.transform);
				firstItem.transform.localPosition = mapItemPos[0];
				
				MapItem secondItem = (MapItem)GameObject.Instantiate(mapItem);
				secondItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart + 1], iStart + 1);
				NGUIUtility.SetParent(transform, secondItem.transform);
				secondItem.transform.localPosition = mapItemPos[1];
			
				MapItem thirdItem = (MapItem)GameObject.Instantiate(mapItem);
				thirdItem.SetData(Globals.It.MapJsonMgr.MapJsons[iStart + 2], iStart + 2);
				NGUIUtility.SetParent(transform, thirdItem.transform);
				thirdItem.transform.localPosition = mapItemPos[2];
				
				return new MapItem[] { firstItem, secondItem, thirdItem };
			}
		}
	}
}
