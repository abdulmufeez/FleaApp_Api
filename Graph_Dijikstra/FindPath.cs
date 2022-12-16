using Priority_Queue;
using System.Text;

namespace fleaApi.Graph_Dijikstra
{
    #region WeightedGraphNode: Create Node Structure
    public class WeightedGraphNode<T>
    {
        #region Properties - Index, Value, Neighbors, Weights         
        public int Index { get; set; }
        public T Value { get; set; }
        public List<WeightedGraphNode<T>> Neighbors { get; set; } = new List<WeightedGraphNode<T>>();
        public List<int> Weights { get; set; } = new List<int>();
        #endregion  
        #region Basic Operations-AddNeighbors, RemoveNeighbors, ToString
        public bool AddNeighbors(WeightedGraphNode<T> neighbor)
        {
            if (Neighbors.Contains(neighbor))
            {
                return false;
            }
            else
            {
                Neighbors.Add(neighbor);
                return true;
            }
        }

        public override string ToString()
        {
            StringBuilder nodeString = new StringBuilder();
            nodeString.Append("[ Node Value - " + Value + " with Neighbors : ");
            for (int i = 0; i < Neighbors.Count; i++)
            {                
                nodeString.Append(Neighbors[i].Value + " ");
            }
            nodeString.Append("]");
            return nodeString.ToString();
        }
        #endregion
    }
    #endregion

    #region WeightedEdge: Create Weighted Edge Structure
    public class WeightedEdge<T>
    {
        public WeightedGraphNode<T> From { get; set; }
        public WeightedGraphNode<T> To { get; set; }
        public int Weight { get; set; }
        
        public override string ToString()
        {
            return $"WeightedEdge: {From.Value} -> {To.Value}, weight: {Weight}";
        }
    }
    #endregion

    #region WeightedGraph: Create Undirected, Weighted Structure, Build, Search and Get the path  
    public class WeightedGraph<T>
    {
        #region WeightedGraph: Internal Variable - list of nodes        
        List<WeightedGraphNode<T>> nodes = new List<WeightedGraphNode<T>>();
        private bool _isDirected = false;
        private bool _isWeighted = false;
        #endregion

        #region WeightedGraph: constructor
        public WeightedGraph(bool isDirected, bool isWeighted)
        {
            _isDirected = isDirected;
            _isWeighted = isWeighted;
        }
        #endregion        

        #region WeightedGraph: Readonly Properties - Count, Nodes        
        public int Count
        {
            get
            {
                return nodes.Count;
            }
        }
        public List<WeightedGraphNode<T>> Nodes
        {
            get
            {
                return nodes;
            }
        }
        #endregion       

        #region WeightedGraph: Add Node/Edge, Find, Remove Node/Edge, ToString                 
        public WeightedGraphNode<T> AddNode(T value)
        {
            WeightedGraphNode<T> node = new WeightedGraphNode<T>() { Value = value };
            if (Find(node) != null)
            {
                //duplicate value
                return null;
            }
            else
            {
                Nodes.Add(node);
                UpdateIndices();
                return node;
            }
        }
        public bool AddEdge(WeightedGraphNode<T> from, WeightedGraphNode<T> to, int weight)
        {
            WeightedGraphNode<T> source = Find(from);
            WeightedGraphNode<T> destination = Find(to);
            if (source == null || destination == null)
            {
                return false;
            }
            else if (source.Neighbors.Contains(destination))
            {
                return false;
            }
            else
            {
                //for direted graph only below 1st line is required  node1->node2
                from.AddNeighbors(to);
                if (_isWeighted)
                {
                    source.Weights.Add(weight);
                }
                //for undireted graph need below line as well
                if (!_isDirected)
                {
                    to.AddNeighbors(from);
                    if (_isWeighted)
                    {
                        to.Weights.Add(weight);
                    }
                }
                return true;
            }
        }
        public WeightedGraphNode<T> Find(WeightedGraphNode<T> weightedGraphNode)
        {
            foreach (WeightedGraphNode<T> node in nodes)
            {
                if (node.Value.Equals(weightedGraphNode.Value))
                {
                    return node;
                }
            }
            return null;
        }                   
        #endregion        

        #region WeightedGraph: GetEdges, GetRoot, Subset, Union, UpdateIndices         
        //Increase the Index Value
        private void UpdateIndices()
        {
            int i = 0;
            Nodes.ForEach(n => n.Index = i++);
        }            

        #endregion
        
        #region WeightedGraph: MinimumSpanningTreePrim 
        public WeightedEdge<T> this[int from, int to]
        {
            get
            {
                WeightedGraphNode<T> nodeFrom = Nodes[from];
                WeightedGraphNode<T> nodeTo = Nodes[to];
                int i = nodeFrom.Neighbors.IndexOf(nodeTo);
                if (i >= 0)
                {
                    WeightedEdge<T> edge = new WeightedEdge<T>()
                    {
                        From = nodeFrom,
                        To = nodeTo,
                        Weight = i < nodeFrom.Weights.Count ? nodeFrom.Weights[i] : 0
                    };
                    return edge;
                }

                return null;
            }
        }

        

        private void Fill<Q>(Q[] array, Q value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
        #endregion
    
        #region Shortest Path - Dijkstra Algorithm
        public List<WeightedEdge<T>> GetShortestPathDijkstra(WeightedGraphNode<T> source, WeightedGraphNode<T> target)
        {
            int[] previous = new int[Nodes.Count];
            //Set Every Previous node with initial value -1
            Fill(previous, -1);

            int[] distances = new int[Nodes.Count];
            //Set Every Previous node with initial value -1
            Fill(distances, int.MaxValue);
            //Initially distance will be 0 on starting node
            distances[source.Index] = 0;

            //Create SimplePriorityQueue for dynamicall update the priority of each node on the basis of distance and process accordingly
            SimplePriorityQueue<WeightedGraphNode<T>> nodes = new SimplePriorityQueue<WeightedGraphNode<T>>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                nodes.Enqueue(Nodes[i], distances[i]);
            }

            while (nodes.Count != 0)
            {
                WeightedGraphNode<T> node = nodes.Dequeue();
                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    WeightedGraphNode<T> neighbor = node.Neighbors[i];
                    int weight = i < node.Weights.Count ? node.Weights[i] : 0;
                    int weightTotal = distances[node.Index] + weight;

                    if (distances[neighbor.Index] > weightTotal)
                    {
                        distances[neighbor.Index] = weightTotal;
                        previous[neighbor.Index] = node.Index;
                        nodes.UpdatePriority(neighbor, distances[neighbor.Index]);
                    }
                }
            }

            //Getting all the index
            List<int> indices = new List<int>();
            int index = target.Index;
            while (index >= 0)
            {
                indices.Add(index);
                index = previous[index];
            }

            //Reverse all the index to get the correct order
            indices.Reverse();
            List<WeightedEdge<T>> result = new List<WeightedEdge<T>>();
            for (int i = 0; i < indices.Count - 1; i++)
            {
                WeightedEdge<T> edge = this[indices[i], indices[i + 1]];
                result.Add(edge);
            }
            //return list of WeightedEdge
            return result;
        }
        #endregion
    }
    #endregion
}
