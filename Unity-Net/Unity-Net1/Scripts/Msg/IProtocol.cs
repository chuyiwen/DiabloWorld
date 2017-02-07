using UnityEngine;
using System.Collections;

public interface IProtocol {

	int iCommand { get; }
	
	void Process(Message_Body info);	// 这个方法是线程安全的.
}
