using UnityEditor;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
	public class DecisionNode : Node, IDecisionTreeNodeType {
		[Input(connectionType = ConnectionType.Override), HideInInspector] public bool Input;
		[Output(connectionType = ConnectionType.Override), HideInInspector] public bool Output1;
		[Output(connectionType = ConnectionType.Override), HideInInspector] public bool Output2;

		[SerializeField]
		[HideInInspector]
		int typeId;
		public int TypeId {
			get => typeId;
			set => typeId = value;
		}


		// Use this for initialization
		protected override void Init() {
			base.Init();
		
		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port) {
			return null; // Replace this
		}
	}
}