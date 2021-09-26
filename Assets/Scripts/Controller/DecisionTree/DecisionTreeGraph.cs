using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DecisionTree.Nodes;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;
using IDecisionTreeNode = Model.NAI.NDecisionTree.IDecisionTreeNode;

namespace Controller.DecisionTree {
	[CreateAssetMenu]
	public class DecisionTreeGraph : NodeGraph {
		[HideInInspector] public string[] DecisionTypes;
		[HideInInspector] public string[] ActionTypes;
		[HideInInspector] public int[] DecisionTypeIds;
		[HideInInspector] public int[] ActionTypeIds;

		public void Init() {
			Lookup<BaseDecision>(out DecisionTypes, out DecisionTypeIds);
			Lookup<BaseAction>(out ActionTypes, out ActionTypeIds);

			foreach (var node in nodes)
				if (node is DecisionTreeSaverNode saver)
					saver.Load();
		}

		void Lookup<T>(out string[] types, out int[] ids) {
			var decisionEnums = LookupDecisionTreeEnums(typeof(T));
			types = decisionEnums.Select(e => e.ToString()).ToArray();
			ids = decisionEnums.Cast<int>().ToArray();
		}

		//TODO: extract
		IEnumerable<EDecisionTreeType> LookupDecisionTreeEnums(Type type) =>
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(type.IsAssignableFrom)
				.Where(t => t != type)
				.Select(Activator.CreateInstance)
				.Cast<IDecisionTreeNode>()
				.Select(n => n.Type);
	}
}