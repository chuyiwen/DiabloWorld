using UnityEngine;
using System.Collections;

public interface IProtocol {
    // 每个操作都有自己唯一的操作码
	int iCommand { get; }
	
	void Process(Message_Body info);	// 这个方法是线程安全的.
}
