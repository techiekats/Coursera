import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;
import java.util.function.BiConsumer;
import java.util.function.Consumer;


public class ShortestPathComputer {

	private static final int INFINITY=1000000;
	private static final String SOURCE="s";		
	private static HashMap<String, ArrayList<Tuple<String, Integer>>> graph;
	private static HashMap<String, Integer> computedDistances;
	public static void main(String[] args) {
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		//LoadGraphFromFile();
		LoadGraphFromTest();
		ComputeDijkstraShortestPath();

		Iterator<Entry<String, Integer>> it = computedDistances.entrySet().iterator();
		while (it.hasNext())
		{
			Entry<String, Integer> entry = it.next();
			System.out.println("Node:" + entry.getKey() + "\tDistance:" + computedDistances.get(entry.getKey()) + "\n");
		}
	}
	private static void LoadGraphFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/Dijkstra", "dijkstraData.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
			for (int i = 0; i < fileLines.length; i++)
			{
				ArrayList <String> adjListRow = new ArrayList<String>(Arrays.asList(fileLines[i].toString().split("\t")));
				String head = adjListRow.get(0);
				adjListRow.remove(0);
				ArrayList<Tuple<String,Integer>> adjacentEdgesList = new ArrayList<Tuple<String,Integer>>();
				for (int j = 0; j < adjListRow.size(); j++)
				{
					Tuple<String, Integer> t = new Tuple<String, Integer>(adjListRow.get(j).split(",")[0], Integer.parseInt(adjListRow.get(j).split(",")[1]));
					adjacentEdgesList.add(t);
				}
				graph.put(head, adjacentEdgesList);
			}	
		
	}

	private static void LoadGraphFromTest()
	{
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		
		//node s
		ArrayList<Tuple<String,Integer>> sList = new ArrayList<Tuple<String,Integer>>();
		sList.add(new Tuple<String,Integer>("t", 1));
		sList.add(new Tuple<String,Integer>("u", 2));
		graph.put("s", sList);

		//node t
		ArrayList<Tuple<String,Integer>> tList = new ArrayList<Tuple<String,Integer>>();
		tList.add(new Tuple<String,Integer>("v", 4));
		graph.put("t", tList);
		
		//node u
		ArrayList<Tuple<String,Integer>> uList = new ArrayList<Tuple<String,Integer>>();
		uList.add(new Tuple<String,Integer>("v",2));
		uList.add(new Tuple<String,Integer>("w", 3));
		graph.put("u", uList);
		
		//node v
		ArrayList<Tuple<String,Integer>> vList = new ArrayList<Tuple<String,Integer>>();
		vList.add(new Tuple<String,Integer>("x",3));
		graph.put("v", vList);
	
		//node w
		ArrayList<Tuple<String,Integer>> wList = new ArrayList<Tuple<String,Integer>>();
		wList.add(new Tuple<String,Integer>("x",1));
		graph.put("w", wList);
		
		//node x
		graph.put("x", null);
	}
	
	private static void ComputeDijkstraShortestPath ()
	{
		String nextVertex = SOURCE;
		HashMap<String, Tuple<Boolean,Boolean>> computationStatus = new HashMap<String, Tuple<Boolean, Boolean>>();
		//Hashmap<String, <isProcessed, isReachable>>

		//construct initial set and compute distances
		computedDistances = new HashMap<String,Integer>();
	
		for (Iterator<String> k = graph.keySet().iterator(); k.hasNext();)
		{
			String key = k.next();
			computedDistances.put(key, INFINITY);
			computationStatus.put(key, new Tuple<Boolean, Boolean>(false, false));
		}
		computedDistances.put(SOURCE, 0);
	
		while (graph.size() != 0)
		{
			String temp = nextVertex;
			Integer distanceOfCurrentVertex = computedDistances.get(nextVertex); 
			Integer minimum = INFINITY;
			//update the graph with new calculations
			if (null != graph.get(nextVertex))
			{
				computationStatus.put(nextVertex, new Tuple<Boolean, Boolean>(true, true));
				for  (Iterator <Tuple<String,Integer>> distanceFromSource = graph.get(nextVertex).iterator(); distanceFromSource.hasNext();)
				{
					
					Tuple<String, Integer> t = distanceFromSource.next();
					computationStatus.put(t.x, new Tuple<Boolean, Boolean>(computationStatus.get(t.x).x, true));

					if (t.y + distanceOfCurrentVertex < computedDistances.get(t.x))
					{
						computedDistances.put(t.x, t.y+ distanceOfCurrentVertex);
					}
					if (computedDistances.get(t.x) < minimum)
					{
						nextVertex = t.x;	
						minimum = computedDistances.get(t.x);
					}
				}
			}	
			else
			{
				Iterator<Entry<String, Tuple<Boolean, Boolean>>> it = computationStatus.entrySet().iterator();
				while (it.hasNext())
				{
					Entry<String, Tuple<Boolean, Boolean>>  entry = it.next();
					if (!entry.getValue().x)
					{
						if (entry.getValue().y)
						{
							nextVertex = entry.getKey();
							break;
						}
					}
				}
			}
				
			//remove the vertex from source
			graph.remove(temp);
		}
	}
}
