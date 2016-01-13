﻿using GeometryFriendsAgents.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryFriendsAgents.Pathfinding.DataStructures
{
    public class NodeRecordArray : WorldModelComponent, IOpenSet, IClosedSet
    {
        private NodeRecord[] NodeRecords { get; set; }
        private List<NodeRecord> SpecialCaseNodes { get; set; } 
        private NodePriorityHeap Open { get; set; }

        public NodeRecordArray(WorldModel WM) : base(WM)
        {
            this.NodeRecords = new NodeRecord[WM.Mesh.Count];
            
            for(int i = 0; i < WM.Mesh.Count; i++)
            {
                var node = WM.Mesh[i];
                this.NodeRecords[i] = new NodeRecord {node = node, status = NodeStatus.Unvisited};
            }

            this.SpecialCaseNodes = new List<NodeRecord>();

            this.Open = new NodePriorityHeap();
        }

        public NodeRecord GetNodeRecord(Point node)
        {

            throw new NotImplementedException();

        }

        public void AddSpecialCaseNode(NodeRecord node)
        {
            this.SpecialCaseNodes.Add(node);
        }

        void IOpenSet.Initialize()
        {
            this.Open.Initialize();
            //we want this to be very efficient (that's why we use for)
            for (int i = 0; i < this.NodeRecords.Length; i++)
            {
                this.NodeRecords[i].status = NodeStatus.Unvisited;
            }

            this.SpecialCaseNodes.Clear();
        }

        void IClosedSet.Initialize()
        {
        }

        public void AddToOpen(NodeRecord nodeRecord)
        {
            this.Open.AddToOpen(nodeRecord);
            nodeRecord.status = NodeStatus.Open;
        }

        public void AddToClosed(NodeRecord nodeRecord)
        {
            nodeRecord.status = NodeStatus.Closed;
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            var storedNode = this.GetNodeRecord(nodeRecord.node);

            if (storedNode != null && storedNode.status == NodeStatus.Open) return storedNode;
            else return null;
        }

        public NodeRecord SearchInClosed(NodeRecord nodeRecord)
        {
            var storedNode = this.GetNodeRecord(nodeRecord.node);

            if (storedNode != null && storedNode.status == NodeStatus.Closed) return storedNode;
            else return null;
        }

        public NodeRecord GetBestAndRemove()
        {
            return this.Open.GetBestAndRemove();
        }

        public NodeRecord PeekBest()
        {
            return this.Open.PeekBest();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            this.Open.Replace(nodeToBeReplaced, nodeToReplace);
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            this.Open.RemoveFromOpen(nodeRecord);
            nodeRecord.status = NodeStatus.Unvisited;
        }

        public void RemoveFromClosed(NodeRecord nodeRecord)
        {
            nodeRecord.status = NodeStatus.Unvisited;
        }

        ICollection<NodeRecord> IOpenSet.All()
        {
            return this.Open.All();
        }

        ICollection<NodeRecord> IClosedSet.All()
        {
            //this is not efficient, but we just use this method for debug
            return this.NodeRecords.Where(node => node.status == NodeStatus.Closed).ToList();
        }

        public int CountOpen()
        {
            return this.Open.CountOpen();
        }
    }
}
