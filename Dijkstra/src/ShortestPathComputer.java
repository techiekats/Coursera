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
	private static final String SOURCE="0";		
	private static HashMap<String, ArrayList<Tuple<String, Integer>>> graph;
	private static HashMap<String, Integer> computedDistances;
	public static void main(String[] args) {
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		//LoadGraphFromFile();
		//LoadGraphFromTest();
		LoadGraphFromTestData2();
		ComputeDijkstraShortestPath();

		Iterator<Entry<String, Integer>> it = computedDistances.entrySet().iterator();
		while (it.hasNext())
		{
			Entry<String, Integer> entry = it.next();
			System.out.println("Node:" + entry.getKey() + "\tDistance:" + computedDistances.get(entry.getKey()) + "\n");
		}
		
		//FOR COURSERA
		// 7,37,59,82,99,115,133,165,188,197.
		/*System.out.printf("%d,%d,%d,%d,%d,%d,%d,%d,%d,%d",computedDistances.get("7"), computedDistances.get("37"),computedDistances.get("59"),computedDistances.get("82"),computedDistances.get("99"),
							computedDistances.get("115"), computedDistances.get("133"),computedDistances.get("165"), computedDistances.get("188"), computedDistances.get("197"));*/
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
		graph.put("1", sList);

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
	

	/*EXPECTED OUTPUT:
	 * Vertex   Distance from Source
		0                0
		1                4
		2                12
		3                19
		4                21
		5                11
		6                9
		7                8
		8                14
	 * */
	private static void LoadGraphFromTestData2()
	{
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		
		//node 0
		ArrayList<Tuple<String,Integer>> list0 = new ArrayList<Tuple<String,Integer>>();
		list0.add(new Tuple<String,Integer>("1", 4));
		list0.add(new Tuple<String,Integer>("7", 8));
		graph.put("0", list0);

		//node 1
		ArrayList<Tuple<String,Integer>> list1 = new ArrayList<Tuple<String,Integer>>();
		list1.add(new Tuple<String,Integer>("2", 8));
		list1.add(new Tuple<String,Integer>("7", 11));
		graph.put("1", list1);

		//node 2
		ArrayList<Tuple<String,Integer>> list2 = new ArrayList<Tuple<String,Integer>>();
		list2.add(new Tuple<String,Integer>("3", 7));
		list2.add(new Tuple<String,Integer>("5", 4));
		list2.add(new Tuple<String,Integer>("8", 2));
		graph.put("2", list2);
		
		//node 3
		ArrayList<Tuple<String,Integer>> list3 = new ArrayList<Tuple<String,Integer>>();
		list3.add(new Tuple<String,Integer>("4", 9));
		list3.add(new Tuple<String,Integer>("5", 14));
		graph.put("3", list3);
		
		//node 4
		ArrayList<Tuple<String,Integer>> list4 = new ArrayList<Tuple<String,Integer>>();
		list4.add(new Tuple<String,Integer>("5", 10));
		graph.put("4", list4);	
		
		//node 5
		ArrayList<Tuple<String,Integer>> list5 = new ArrayList<Tuple<String,Integer>>();
		list5.add(new Tuple<String,Integer>("6", 2));
		graph.put("5", list5);	
		
		//node 6
		ArrayList<Tuple<String,Integer>> list6 = new ArrayList<Tuple<String,Integer>>();
		list6.add(new Tuple<String,Integer>("7", 1));
		list6.add(new Tuple<String,Integer>("8", 6));
		graph.put("6", list6);	
		
		//node 7
		ArrayList<Tuple<String,Integer>> list7 = new ArrayList<Tuple<String,Integer>>();
		list7.add(new Tuple<String,Integer>("8", 7));
		graph.put("7", list7);	
		
		//node 8
		graph.put("8", null);	

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
