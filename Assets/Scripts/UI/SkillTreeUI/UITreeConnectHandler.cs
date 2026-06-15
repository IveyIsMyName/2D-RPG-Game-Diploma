using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UITreeConnectDetails
{
	public UITreeConnectHandler childNode;
	public NodeDirectionType direction;
	[Range(100f, 350f)] public float length;
	[Range(-50f, 50f)] public float rotation;
}

public class UITreeConnectHandler : MonoBehaviour
{
	private RectTransform rect => GetComponent<RectTransform>();

	[SerializeField] private UITreeConnectDetails[] connectionDetails;
	[SerializeField] private UITreeConnection[] connections;

	// Храним исходный цвет для каждой линии отдельно
	private Color[] originalConnectionColors;

	private void Awake()
	{
		originalConnectionColors = new Color[connections.Length];
		for (int i = 0; i < connections.Length; i++)
		{
			if (connections[i] != null && connections[i].GetConnectionImage() != null)
			{
				originalConnectionColors[i] = connections[i].GetConnectionImage().color;
			}
		}
	}

	public UITreeNode[] GetChildNodes()
	{
		List<UITreeNode> childrenToReturn = new List<UITreeNode>();
		foreach (var node in connectionDetails)
		{
			if (node.childNode != null)
			{
				childrenToReturn.Add(node.childNode.GetComponent<UITreeNode>());
			}
		}
		return childrenToReturn.ToArray();
	}

	public void UpdateConnections()
	{
		for (int i = 0; i < connectionDetails.Length; i++)
		{
			var detail = connectionDetails[i];
			var connection = connections[i];

			
			Image connImg = connection.GetConnectionImage();
			connection.DirectConnection(detail.direction, detail.length, detail.rotation);

			if (detail.childNode == null) continue;

			Vector2 targetPosition = connection.GetConnectionPoint(rect);
			detail.childNode.SetPosition(targetPosition);
			detail.childNode.transform.SetAsLastSibling();
		}
	}

	public void UpdateAllConnections()
	{
		UpdateConnections();
		foreach (var node in connectionDetails)
		{
			if (node.childNode == null) continue;
			node.childNode.UpdateConnections();
		}
	}

	
	public void UnlockConnectionImage(bool unlocked)
	{
		if (connections == null) return;

		if (originalConnectionColors == null || originalConnectionColors.Length != connections.Length)
		{
			originalConnectionColors = new Color[connections.Length];
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i] != null && connections[i].GetConnectionImage() != null)
				{
					originalConnectionColors[i] = connections[i].GetConnectionImage().color;
				}
			}
		}

		for (int i = 0; i < connections.Length; i++)
		{
			if (connections[i] == null) continue;

			var img = connections[i].GetConnectionImage();
			if (img != null)
			{
				img.color = unlocked ? Color.white : originalConnectionColors[i];
			}
		}
	}

	private void OnValidate()
	{
		if (connectionDetails.Length <= 0) return;
		if (connectionDetails.Length != connections.Length)
		{
			Debug.Log("Amount of details should be same as amount of connections. - " + gameObject.name);
		}
		UpdateConnections();
	}

	public void SetConnectionImage(Image image) { /* Больше не требуется, логика перенесена в UnlockConnectionImage */ }
	public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}