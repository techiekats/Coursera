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
import java.util.function.Predicate;
import java.util.function.UnaryOperator;


public class MiniMumSpanningTreeFinder
{

	private static final int INFINITY=1000000;
	private static String SOURCE="1";		
	private static HashMap<String, ArrayList<Tuple<String, Integer>>> graph;
	private static long mstCost = 0;
	private static Integer graphKey = Integer.MIN_VALUE;
	//private static HashMap<String, Integer> computedDistances;
	public static void main(String[] args) {
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		LoadGraphFromFile();
		//LoadGraphFromTest();
		//LoadGraphFromTestData2();
		FindMST();

		System.out.printf("MST cost=" + mstCost);
	}
	private static void LoadGraphFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/Prims-MST", "edges.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			//graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
			//graphKey = Integer.parseInt(fileLines[0].toString().split(" ")[0]);
			graphKey = Integer.MIN_VALUE;
			for (int i = 1; i < fileLines.length; i++)
			{
				ArrayList <String> edge = new ArrayList<String>(Arrays.asList(fileLines[i].toString().split(" ")));
				String head = edge.get(0).trim();
				String tail = edge.get(1).trim();
				Integer weight = Integer.parseInt(edge.get(2));
				ArrayList<Tuple<String,Integer>> a1 = (null == graph.get(head)) ? new ArrayList<Tuple<String,Integer>>() : graph.get(head);
				ArrayList<Tuple<String,Integer>> a2 = (null == graph.get(tail)) ? new ArrayList<Tuple<String,Integer>>() : graph.get(tail);
				a1.add(new Tuple<String,Integer> (tail, weight));
				a2.add(new Tuple<String,Integer> (head, weight));

				graph.put(head, a1);
				graph.put(tail, a2);
			}	
		SOURCE = fileLines[1].toString().split(" ")[0];
		Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> action = new Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> (){

			public void accept(
					Entry<String, ArrayList<Tuple<String, Integer>>> t) {
					for (Tuple<String, Integer> edge: t.getValue())
					{
						System.out.printf("\n%s\t%s\t%d", t.getKey(), edge.x, edge.y);
					}
					System.out.print("\n---------------------\n");
			}};
		graph.entrySet().forEach(action );
	}

	private static void LoadGraphFromTest()
	{
		//MST cost = 9
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		
		//node s
		ArrayList<Tuple<String,Integer>> sList = new ArrayList<Tuple<String,Integer>>();
		sList.add(new Tuple<String,Integer>("t", 1));
		sList.add(new Tuple<String,Integer>("u", 2));
		graph.put("1", sList);

		//node t
		ArrayList<Tuple<String,Integer>> tList = new ArrayList<Tuple<String,Integer>>();
		tList.add(new Tuple<String,Integer>("1", 1));
		tList.add(new Tuple<String,Integer>("v", 4));
		graph.put("t", tList);
		
		//node u
		ArrayList<Tuple<String,Integer>> uList = new ArrayList<Tuple<String,Integer>>();
		uList.add(new Tuple<String,Integer>("v",2));
		uList.add(new Tuple<String,Integer>("w", 3));
		uList.add(new Tuple<String,Integer>("1", 2));
		graph.put("u", uList);
		
		//node v
		ArrayList<Tuple<String,Integer>> vList = new ArrayList<Tuple<String,Integer>>();
		vList.add(new Tuple<String,Integer>("x",3));
		vList.add(new Tuple<String,Integer>("t",4));
		vList.add(new Tuple<String,Integer>("u",2));
		graph.put("v", vList);
	
		//node w
		ArrayList<Tuple<String,Integer>> wList = new ArrayList<Tuple<String,Integer>>();
		wList.add(new Tuple<String,Integer>("x",1));
		wList.add(new Tuple<String,Integer>("u",3));
		graph.put("w", wList);
		
		//node x
		ArrayList<Tuple<String,Integer>> xList = new ArrayList<Tuple<String,Integer>>();
		xList.add(new Tuple<String,Integer>("w",1));
		xList.add(new Tuple<String,Integer>("v",3));
		graph.put("x", xList);
	}
	
	private static void LoadGraphFromTestData2()
	{
		//MST cost=37
		SOURCE = "0";
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		
		//node 0
		ArrayList<Tuple<String,Integer>> list0 = new ArrayList<Tuple<String,Integer>>();
		list0.add(new Tuple<String,Integer>("1", 4));
		list0.add(new Tuple<String,Integer>("7", 8));
		graph.put("0", list0);

		//node 1
		ArrayList<Tuple<String,Integer>> list1 = new ArrayList<Tuple<String,Integer>>();
		list1.add(new Tuple<String,Integer>("0", 4));
		list1.add(new Tuple<String,Integer>("7", 11));
		list1.add(new Tuple<String,Integer>("2", 8));

		graph.put("1", list1);

		//node 2
		ArrayList<Tuple<String,Integer>> list2 = new ArrayList<Tuple<String,Integer>>();
		list2.add(new Tuple<String,Integer>("3", 7));
		list2.add(new Tuple<String,Integer>("5", 4));
		list2.add(new Tuple<String,Integer>("8", 2));
		list2.add(new Tuple<String,Integer>("1", 8));

		graph.put("2", list2);
		
		//node 3
		ArrayList<Tuple<String,Integer>> list3 = new ArrayList<Tuple<String,Integer>>();
		list3.add(new Tuple<String,Integer>("4", 9));
		list3.add(new Tuple<String,Integer>("5", 14));
		list3.add(new Tuple<String,Integer>("2", 7));

		graph.put("3", list3);
		
		//node 4
		ArrayList<Tuple<String,Integer>> list4 = new ArrayList<Tuple<String,Integer>>();
		list4.add(new Tuple<String,Integer>("5", 10));
		list4.add(new Tuple<String,Integer>("3", 9));

		graph.put("4", list4);	
		
		//node 5
		ArrayList<Tuple<String,Integer>> list5 = new ArrayList<Tuple<String,Integer>>();
		list5.add(new Tuple<String,Integer>("6", 2));
		list5.add(new Tuple<String,Integer>("2", 4));
		list5.add(new Tuple<String,Integer>("3", 14));
		list5.add(new Tuple<String,Integer>("4", 10));


		graph.put("5", list5);	
		
		//node 6
		ArrayList<Tuple<String,Integer>> list6 = new ArrayList<Tuple<String,Integer>>();
		list6.add(new Tuple<String,Integer>("7", 1));
		list6.add(new Tuple<String,Integer>("8", 6));
		list6.add(new Tuple<String,Integer>("5", 2));

		graph.put("6", list6);	
		
		//node 7
		ArrayList<Tuple<String,Integer>> list7 = new ArrayList<Tuple<String,Integer>>();
		list7.add(new Tuple<String,Integer>("8", 7));
		list7.add(new Tuple<String,Integer>("0", 8));
		list7.add(new Tuple<String,Integer>("1", 11));
		list7.add(new Tuple<String,Integer>("6", 1));

		graph.put("7", list7);	
		
		//node 8
		ArrayList<Tuple<String,Integer>> list8 = new ArrayList<Tuple<String,Integer>>();
		list8.add(new Tuple<String,Integer>("2", 2));
		list8.add(new Tuple<String,Integer>("7", 7));
		list8.add(new Tuple<String,Integer>("6", 6));
		graph.put("8", list8);
		
		Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> action = new Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> (){

			public void accept(
					Entry<String, ArrayList<Tuple<String, Integer>>> t) {
					for (Tuple<String, Integer> edge: t.getValue())
					{
						System.out.printf("\n%s\t%s\t%d", t.getKey(), edge.x, edge.y);
					}
					System.out.print("\n---------------------\n");
			}};
		graph.entrySet().forEach(action );
	}

	
	@SuppressWarnings("unchecked")
	private static void FindMST ()
	{
		String nextVertex = SOURCE;
		while (graph.get(nextVertex).size() != 0)
		{
			final String temp = nextVertex;
			Integer minimum = INFINITY;
			
			System.out.println("\nNext Vertex=" + nextVertex);
			//System.out.println("Graph Size= " + graph.size());
			for  (Iterator <Tuple<String,Integer>> adjEdge = graph.get(nextVertex).iterator(); adjEdge.hasNext();)
			{
				Tuple<String, Integer> t = adjEdge.next();
				//System.out.printf("X:%s\tY:%d\n", t.x, t.y);
				if (t.y < minimum)
				{
					minimum = t.y;
					nextVertex = t.x;
					System.out.println("Next Vertex updated to:" + nextVertex);
					System.out.println("Edge cost added to MST:" + minimum);
				}
			}
			mstCost += minimum;

			graphKey++;
			final String newKey = graphKey.toString();
			final String temp2 = nextVertex;
			
			Predicate<Tuple<String, Integer>> filter = new Predicate <Tuple<String, Integer>> (){

				public boolean test(Tuple<String, Integer> t) {
					//System.out.println ("\nWill remove " + t.x + "?" + ( t.x.equalsIgnoreCase(temp) || t.x == temp2 || t.x == newKey));
					//System.out.printf("\n%s\t%s\t%s", temp, temp2, newKey);
					return t.x.equalsIgnoreCase(temp) || t.x.equalsIgnoreCase(temp2) || t.x.equalsIgnoreCase(newKey);
				}
			};

			if (null == graph.get (nextVertex))
			{
				return;
			}
			ArrayList <Tuple<String, Integer>> a = graph.get(nextVertex);
			//System.out.print(a.size());
			a.addAll(graph.get(temp));
			//merge step of two nodes
			graph.put(newKey,  a);
			graph.get(newKey).removeIf(filter);
			UnaryOperator<Tuple<String,Integer>> operator = new UnaryOperator<Tuple<String,Integer>>(){
				public Tuple<String,Integer> apply(Tuple<String,Integer> t) {
				if (t.x.equals(temp) || t.x.equals(temp2))
				{
					return new Tuple<String,Integer>(newKey, t.y);
				}
				return t;
			}};
			//remove the vertex from source
			graph.remove(temp);
			graph.remove(nextVertex);
			for (Tuple<String, Integer> s: (ArrayList<Tuple<String,Integer>>) graph.get(newKey))
			{
				if (null != graph.get(s.x))
				{
					//System.out.println("Modifying:" + s.x);
					((ArrayList<Tuple<String,Integer>>)graph.get(s.x)).replaceAll(operator);
				}
			}
			nextVertex = newKey;
			/*Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> action = new Consumer<Entry<String, ArrayList<Tuple<String, Integer>>>> (){

				public void accept(
						Entry<String, ArrayList<Tuple<String, Integer>>> t) {
						for (Tuple<String, Integer> edge: t.getValue())
						{
							System.out.printf("\n%s\t%s\t%d", t.getKey(), edge.x, edge.y);
						}
						System.out.print("\n---------------------\n");
				}};
			graph.entrySet().forEach(action );*/
		}
	}
}
