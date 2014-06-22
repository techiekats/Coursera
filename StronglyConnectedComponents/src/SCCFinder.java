import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map.Entry;
import java.util.Queue;
import java.util.Stack;


public class SCCFinder {

	private static HashMap<Long,Short> StronglyConnectedComponentsCount;
	private static HashMap<Long, Tuple<Tuple<List<Long>,Boolean>,Tuple<List<Long>,Boolean>>> Graph; 
	private static Heap nodesHeap;
	private static ArrayList<Long> sortedProcessedNodes; //sorted because they are inserted in sorted orders
	static Long currentLeader;
	public static void main(String[] args) {

		StronglyConnectedComponentsCount = new HashMap<Long, Short>();
		Graph = new HashMap<Long, Tuple<Tuple<List<Long>,Boolean>,Tuple<List<Long>,Boolean>>>();
		//nodesHeap = new Heap(8, true);
		sortedProcessedNodes = new ArrayList<Long>();
		LoadGraphFromFile();
		//LoadTestGraph();
		//1. Run DFS on reversed graph to find ordering
		while (nodesHeap.size() != 0)
		{
			DFSRev(nodesHeap.remove());
		}
		nodesHeap = null; //destroy heap
		//2. Run DFS on actual graph to find SCC. Remove an element once SCC is found and increment the bucket's count by 1
		while (sortedProcessedNodes.size() != 0)
		{
			currentLeader = new Long(sortedProcessedNodes.get(sortedProcessedNodes.size() - 1));
			StronglyConnectedComponentsCount.put(currentLeader, (short) 0);
			DFS (currentLeader);
		}
		
		Object[] results = StronglyConnectedComponentsCount.values().toArray();
		nodesHeap = new Heap(results.length, true);
		
		for (int i = 0; i<results.length; i++)
		{
			nodesHeap.insert(Long.parseLong(results[i].toString()));
		}
		for (int i = 0; i<5; i++)
		{
			System.out.print(nodesHeap.remove() + ",");
		}
	}
	
	@SuppressWarnings("unchecked")
	private static void LoadTestGraph (){
		nodesHeap = new Heap(8, true);
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

	private static void LoadGraphFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/StronglyConnectedComponents", "SCC.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				e.printStackTrace();
			}

			for (int i = 0; i < fileLines.length; i++)
			{
				ArrayList <String> adjListRow = new ArrayList<String>(Arrays.asList(fileLines[i].toString().split(" ")));
				Long head = Long.parseLong(adjListRow.get(0));
				Long tail = Long.parseLong(adjListRow.get(1));

				if (null == Graph.get(head))
				{
					Graph.put(head, new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>>(new Tuple<List<Long>, Boolean>(Arrays.asList(tail), false), new Tuple<List<Long>, Boolean>(new ArrayList<Long>(), false)));
				}
				else
				{
					Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>> e1 = Graph.get(head);
					List<Long> temp =	new ArrayList<Long>();
					temp.addAll(e1.x.x);
					temp.add(tail);
					Graph.put(head, new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>> (new Tuple<List<Long>, Boolean>(temp, false),new Tuple<List<Long>, Boolean>(e1.y.x, false)));
				}
				if (null == Graph.get(tail))
				{
					Graph.put(tail, new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>> (new Tuple<List<Long>, Boolean>(new ArrayList<Long>(), false),new Tuple<List<Long>, Boolean>(Arrays.asList(head), false)));
				}
				else
				{
					Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>> e1 = Graph.get(tail);
					List<Long> temp =	new ArrayList<Long>();
					temp.addAll(e1.y.x);
					temp.add(head);
					Graph.put(tail, new Tuple<Tuple<List<Long>, Boolean>, Tuple<List<Long>, Boolean>> (new Tuple<List<Long>, Boolean>(e1.x.x, false),new Tuple<List<Long>, Boolean>(temp, false)));
				}
				
			}
		
		nodesHeap = new Heap (Graph.keySet().size(), false);	

		Iterator<Long> keySet = Graph.keySet().iterator();
		while (keySet.hasNext())
		{
			nodesHeap.insert(keySet.next());
		}
		System.out.println("Loaded Graph");	
	}
	
	private static void DFSRev (final Long nodeForDfs)
	{
		if (Graph.get(nodeForDfs).y.y == true)
			return;

		Graph.get(nodeForDfs).y.y = true;
		for (int i =0; i< Graph.get(nodeForDfs).y.x.size(); i++)
		{
			Long entry = Graph.get(nodeForDfs).y.x.get(i);
			
			DFSRev(entry);
			
		}

		sortedProcessedNodes.add(nodeForDfs);
	}
	
	private static void DFS (final Long nodeForDfs)
	{
		if (Graph.get(nodeForDfs).x.y == true)
		return;

		Graph.get(nodeForDfs).x.y = true;
		sortedProcessedNodes.remove(nodeForDfs);
		for (int i =0; i< Graph.get(nodeForDfs).x.x.size(); i++)
		{
			Long entry = Graph.get(nodeForDfs).x.x.get(i);
			DFS(entry);
		}
		short count = StronglyConnectedComponentsCount.get(currentLeader);
		StronglyConnectedComponentsCount.put(currentLeader, ++count);

	}
}
