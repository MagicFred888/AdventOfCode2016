﻿namespace AdventOfCode2016.Tools
{
    public class QuickDijkstra
    {
        public class Node(string nodeName)
        {
            public string Name { get; init; } = nodeName;

            public Dictionary<Node, long> Links { get; set; } = [];

            public override string ToString() => Name;
        }

        private readonly Dictionary<string, Node> _allNodes = [];

        public List<Node> Nodes => new(_allNodes.Values);

        public QuickDijkstra(List<(string from, string to, long distance)> allPair)
        {
            // Extract all from and to and keep only one instance of each to create node
            _allNodes.Clear();
            foreach (string nodeName in allPair.SelectMany(d => new[] { d.from, d.to }).Distinct().Order().ToList())
            {
                _allNodes.Add(nodeName, new Node(nodeName));
            }
            foreach (Node node in _allNodes.Values)
            {
                // Extract all item linked to current Node
                List<(string from, string to, long distance)> pairToLink = [.. allPair.FindAll(p => p.from == node.Name || p.to == node.Name)];
                node.Links = pairToLink.ConvertAll(p => new KeyValuePair<Node, long>(_allNodes[p.from == node.Name ? p.to : p.from], p.distance))
                                       .ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public long GetShortestWay(string from, string to)
        {
            Node startNode = _allNodes[from];
            Node endNode = _allNodes[to];
            Dictionary<Node, long> distances = _allNodes.Values.ToDictionary(n => n, n => long.MaxValue);
            distances[startNode] = 0;
            HashSet<Node> visited = [];
            Node currentNode = startNode;
            while (currentNode != endNode)
            {
                visited.Add(currentNode);
                foreach (KeyValuePair<Node, long> link in currentNode.Links)
                {
                    if (visited.Contains(link.Key))
                    {
                        continue;
                    }
                    long newDistance = distances[currentNode] + link.Value;
                    if (newDistance < distances[link.Key])
                    {
                        distances[link.Key] = newDistance;
                    }
                }
                currentNode = distances.Where(d => !visited.Contains(d.Key)).OrderBy(d => d.Value).FirstOrDefault().Key;
            }
            return distances[endNode];
        }

        public long GetShortestPathVisitingAllNodes(string start, bool returnToStart)
        {
            Node startNode = _allNodes[start];
            HashSet<Node> visited =
            [
                startNode
            ];
            return FindShortestPath(startNode, visited, 0, startNode, returnToStart);
        }

        private long FindShortestPath(Node currentNode, HashSet<Node> visited, long currentDistance, Node startNode, bool returnToStart)
        {
            if (visited.Count == _allNodes.Count)
            {
                // Return to the start node to complete the cycle if required
                return returnToStart ? currentDistance + currentNode.Links[startNode] : currentDistance;
            }

            long shortestPath = long.MaxValue;
            foreach (var link in currentNode.Links)
            {
                if (!visited.Contains(link.Key))
                {
                    visited.Add(link.Key);
                    long distance = FindShortestPath(link.Key, visited, currentDistance + link.Value, startNode, returnToStart);
                    if (distance < shortestPath)
                    {
                        shortestPath = distance;
                    }
                    visited.Remove(link.Key);
                }
            }
            return shortestPath;
        }
    }
}