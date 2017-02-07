using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Message_Head {
	public byte HEAD_0 { get; set; }
	public byte HEAD_1 { get; set; }
	public byte HEAD_2 { get; set; }
	public byte HEAD_3 { get; set; }
	public byte ProtoVersion  { get; set; }
	public int ServerVersion  { get; set; }
	public int Length 		  { get; set; }	
}

public class Message_Body {
	public int iCommand { get; set; }
	public byte[] body 	{ get; set; }
}

public class MessageData {
	public Message_Head head { get; set; }
	public Message_Body body { get; set; }
}

public class MessageParse {
	
	public static Message_Head UnParseHead (byte[] buffer) {
		if (buffer.Length >= 13) {
			Message_Head head = new Message_Head();
			head.HEAD_0 = buffer[0];
			head.HEAD_1 = buffer[1];
			head.HEAD_2 = buffer[2];
			head.HEAD_3 = buffer[3];
			head.ProtoVersion = buffer[4];
			head.ServerVersion = System.BitConverter.ToInt32(buffer, 5);
			System.Array.Reverse(buffer, 9, 4);
			head.Length = System.BitConverter.ToInt32(buffer, 9);
			return head;
		}
		return null;
	}
	
	public static MessageData UnParse (byte[] buffer) {
		int iHead = 13;
		{
			Message_Head head = UnParseHead(buffer);
			if (head != null && head.Length <= (buffer.Length - iHead)) {
				Message_Body body = new Message_Body();
				System.Array.Reverse(buffer, 13, 4);
				body.iCommand = System.BitConverter.ToInt32(buffer, 13);
				body.body = new byte[head.Length - 4];
				System.Array.Copy(buffer, iHead + 4, body.body, 0, body.body.Length);
				MessageData data = new MessageData();
				data.head = head;
				data.body = body;
				return data;
			}
		}
		return null;
	}

	public static byte[] ParseHead (int iVersion, int iByteLength){
		byte[] arrBytes = new byte[13];
		arrBytes[0] = 78;  // N
		arrBytes[1] = 37;  // %
		arrBytes[2] = 38;  // &
		arrBytes[3] = 48;  // 0
		arrBytes[4] = 9;  // ????
		System.Array.Copy(System.BitConverter.GetBytes(iVersion),0,arrBytes,5, 4);
		System.Array.Copy(System.BitConverter.GetBytes(iByteLength),0,arrBytes,9, 4);
		System.Array.Reverse(arrBytes, 9, 4);
		return arrBytes;
	}
	
	public static byte[] ParseBody (int iCommand, string sJson){
		
		if(!string.IsNullOrEmpty(sJson)){
			byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(sJson);
			byte[] arrBytes = new byte[4+jsonBytes.Length];
			System.Array.Copy(System.BitConverter.GetBytes(iCommand),0,arrBytes,0, 4);
			System.Array.Reverse(arrBytes, 0, 4);
			System.Array.Copy(jsonBytes, 0, arrBytes, 4, jsonBytes.Length);
			return arrBytes;
		}
		
		return null;
		
	}
	
	public static byte[] Parse (int iVersion, int iCommand, string sJson){
		byte[] bodyBytes = ParseBody(iCommand, sJson);
		if (bodyBytes != null) {
			byte[] headBytes = ParseHead(iVersion, bodyBytes.Length);
			byte[] allBytes = new byte[headBytes.Length + bodyBytes.Length];
			System.Array.Copy(headBytes,0,allBytes,0, headBytes.Length);
			System.Array.Copy(bodyBytes,0,allBytes,headBytes.Length, bodyBytes.Length);
			return allBytes;
		}
		return null;
	}
}
