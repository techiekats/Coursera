import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map.Entry;
import java.util.Queue;
import java.util.Stack;


public class SCCFinder {

	private static HashMap<Long,Short> StronglyConnectedComponentsCount;
	//Todo: mostly Integer not required
	private static HashMap<Long, Tuple<Tuple<List<Long>,Boolean>,Tuple<List<Long>,Boolean>>> Graph; 
	private static Heap nodesHeap;
	private static ArrayList<Long> sortedProcessedNodes; //sorted because they are inserted in sorted orders
	public static void main(String[] args) {

		StronglyConnectedComponentsCount = new HashMap<Long, Short>();
		Graph = new HashMap<Long, Tuple<Tuple<List<Long>,Boolean>,Tuple<List<Long>,Boolean>>>();
		nodesHeap = new Heap(8, true);
		sortedProcessedNodes = new ArrayList<Long>();
		LoadTestGraph();
		//1. Run DFS on reversed graph to find ordering
		DFSRev();
		nodesHeap = null; //destroy heap
		//2. Run DFS on actual graph to find SCC. Remove an element once SCC is found and increment the bucket's count by 1
		DFS ();
	}
	
	@SuppressWarnings("unchecked")
	private static void LoadTestGraph (){
		//node 1
		List<Long> from1 =  Arrays.asList(new Long(2));
		List<Long> to1 = Arrays.asList(new Long(3));
		Graph.put(new Long(1), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from1, false), new Tuple<List<Long>, Boolean>(to1, false)));
		nodesHeap.insert(new Long(1));
		//node 2
		List<Long> from2 = (List<Long>) Arrays.asList(new Long(3),new Long(4));
		List<Long> to2 = (List<Long>) Arrays.asList(new Long(1));
		Graph.put(new Long(2), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from2, false), new Tuple<List<Long>, Boolean>(to2, false)));
		nodesHeap.insert(new Long(2));
		//node 3
		List<Long> from3 = (List<Long>) Arrays.asList(new Long(1));
		List<Long> to3 = (List<Long>) Arrays.asList(new Long(2));
		Graph.put(new Long(3), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from3, false), new Tuple<List<Long>, Boolean>(to3, false)));
		nodesHeap.insert(new Long(3));
		//node 4
		List<Long> from4 = (List<Long>) Arrays.asList(new Long(5));
		List<Long> to4 = (List<Long>) Arrays.asList(new Long(2));
		Graph.put(new Long(4), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from4, false), new Tuple<List<Long>, Boolean>(to4, false)));
		nodesHeap.insert(new Long(4));
		//node 5
		List<Long> from5 = (List<Long>) Arrays.asList(new Long(6),new Long(8));
		List<Long> to5 = (List<Long>) Arrays.asList(new Long(4),new Long(7));
		Graph.put(new Long(5), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from5, false), new Tuple<List<Long>, Boolean>(to5, false)));
		nodesHeap.insert(new Long(5));
		//node 6
		List<Long> from6 = (List<Long>) Arrays.asList(new Long(7));
		List<Long> to6 = (List<Long>) Arrays.asList(new Long(5));
		Graph.put(new Long(6), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from6, false), new Tuple<List<Long>, Boolean>(to6, false)));
		nodesHeap.insert(new Long(6));
		//node 7
		List<Long> from7 = (List<Long>) Arrays.asList(new Long(5));
		List<Long> to7 = (List<Long>) Arrays.asList(new Long(6),new Long(8));
		Graph.put(new Long(7), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from7, false), new Tuple<List<Long>, Boolean>(to7, false)));
		nodesHeap.insert(new Long(7));
		//node 8
		List<Long> from8 = (List<Long>) Arrays.asList(new Long(7));
		List<Long> to8 = (List<Long>) Arrays.asList(new Long(5));
		Graph.put(new Long(8), new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(from8, false), new Tuple<List<Long>, Boolean>(to8, false)));
		nodesHeap.insert(new Long(8));
	}

	private static void DFSRev ()
	{
		Stack<Long> dfsOperations = new Stack<Long>();

		while (nodesHeap.size() != 0)
		{
			dfsOperations.clear();
			Long nodeForDfs = nodesHeap.get();
			while (!Graph.get(nodeForDfs).y.y)
			{
				nodesHeap.remove();
				Graph.get(nodeForDfs).y.y = true;
				dfsOperations.push(nodeForDfs);
				List<Long> toBeTraversed = Graph.get(nodeForDfs).y.x;
				nodeForDfs = toBeTraversed.size() != 0 ? toBeTraversed.get(0) : nodeForDfs;
			}
			while (!dfsOperations.empty())
			{
				sortedProcessedNodes.add(dfsOperations.pop());
			}
		}
	}
	
	private static void DFS ()
	{
		Stack<Long> dfsOperations = new Stack<Long>();

		while (sortedProcessedNodes.size() != 0)
		{
			dfsOperations.clear();
			Long nodeForDfs = sortedProcessedNodes.get(0);
			while (!Graph.get(nodeForDfs).x.y)
			{
				nodesHeap.remove();
				Graph.get(nodeForDfs).x.y = true;
				dfsOperations.push(nodeForDfs);
				List<Long> toBeTraversed = Graph.get(nodeForDfs).x.x;
				nodeForDfs = toBeTraversed.size() != 0 ? toBeTraversed.get(0) : nodeForDfs;
			}
			while (!dfsOperations.empty())
			{
				sortedProcessedNodes.remove(dfsOperations.pop());
			}
		}
	}
}
