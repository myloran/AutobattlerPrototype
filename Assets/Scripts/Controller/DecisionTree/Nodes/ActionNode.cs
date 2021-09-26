using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
	public class ActionNode : Node, IDecisionTreeNodeType {
		[Input(connectionType = ConnectionType.Override), HideInInspector] public bool input;

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