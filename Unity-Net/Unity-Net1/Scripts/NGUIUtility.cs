using UnityEngine;
using System.Collections;

public class NGUIUtility  {

	public static void SetParent (Transform parent, Transform child) {
		Vector3 scale = child.localScale;
		Vector3 position = child.localPosition;
		child.parent = parent;
		child.localScale = scale;
		child.localPosition = position;
	}
}
