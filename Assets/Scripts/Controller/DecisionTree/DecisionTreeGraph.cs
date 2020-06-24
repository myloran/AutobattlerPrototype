using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DecisionTree.Nodes;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree {
	[CreateAssetMenu]
	public class DecisionTreeGraph : NodeGraph {
		[HideInInspector] public string[] DecisionTypes;
		[HideInInspector] public string[] ActionTypes;
		[HideInInspector] public int[] DecisionIds;
		[HideInInspector] public int[] ActionIds;

		public void Init() {
			Lookup<BaseDecision>(out DecisionTypes, out DecisionIds);
			Lookup<BaseAction>(out ActionTypes, out ActionIds);

			foreach (var node in nodes) {
				if (node is DecisionTreeSaverNode saver) {
					saver.Load();
				}
			}
		}

		void Lookup<T>(out string[] types, out int[] ids) {
			var decisionEnums = LookupDecisionTreeEnums(typeof(T));
			types = decisionEnums.Select(e => e.ToString()).ToArray();
			ids = decisionEnums.Cast<int>().ToArray();
		}

		//TODO: extract
		IEnumerable<EDecision> LookupDecisionTreeEnums(Type type) =>
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(type.IsAssignableFrom)
				.Where(t => t != type)
				.Select(Activator.CreateInstance)
				.Cast<IDecisionTreeNode>()
				.Select(n => n.Type);
	}
}