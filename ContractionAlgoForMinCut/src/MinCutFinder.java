import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;
import java.util.function.UnaryOperator;

public class MinCutFinder  {

	private static HashMap<String, ArrayList<String>> graph;
	private static Integer minCut = Integer.MAX_VALUE;
	private final static Integer MAX_TRIALS=10;
	public static void main(String[] args) throws Exception {
			
			graph = new HashMap<String, ArrayList<String>>();
			for (int i = 0; i<MAX_TRIALS ; i++)
			{
				LoadGraphFromFile();
				//UTC
				//LoadGraphFromTestCase();
				minCut = Math.min(minCut, ComputeMinCut (GetNextRandom()));
			}
			System.out.println("\nMIN CUT:" + minCut.toString());
		
	}
	private static void LoadGraphFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/ContractionAlgoForMinCut", "kargerMinCut.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			graph = new HashMap<String, ArrayList<String>>();
			for (int i = 0; i < fileLines.length; i++)
			{
				ArrayList <String> adjListRow = new ArrayList<String>(Arrays.asList(fileLines[i].toString().split("\t")));
				String head = adjListRow.get(0);
				adjListRow.remove(0);
				graph.put(head, adjListRow);
			}	
		
	}
	private static void LoadGraphFromTestCase()
	{
		//UTC 1: 
		graph.clear();
		graph.put("1", new ArrayList<String>(Arrays.asList("2 3".split(" "))));
		graph.put("2", new ArrayList<String>(Arrays.asList("1 3 4".split(" "))));			
		graph.put("3", new ArrayList<String>(Arrays.asList("1 2 4".split(" "))));			
		graph.put("4", new ArrayList<String>(Arrays.asList("2 3".split(" "))));	
	}
	@SuppressWarnings("unchecked")
	private static int ComputeMinCut(final Tuple<String,String> edge) throws Exception
	{
		if (graph.size() == 2)
		{
			//Return the remaining number of edges
			return (graph.get(edge.x).size());
		}
		//Merge
		final String newKey = edge.x + "_" + edge.y;
		
		((ArrayList<String>) graph.get(edge.x)).addAll((ArrayList<String>) graph.get(edge.y));
		//merge step of two nodes
		graph.put(newKey, (ArrayList<String>) graph.get(edge.x).clone());

		UnaryOperator<String> operator = new UnaryOperator<String>(){
			public String apply(String t) {
			if (t.equals(edge.x) || t.equals(edge.y))
			{
				return newKey;
			}
			return t;
		}};
		
		for (String s: (ArrayList<String>) graph.get(newKey))
		{
			((ArrayList<String>)graph.get(s)).replaceAll(operator);
		}
		graph.remove(edge.x);
		graph.remove(edge.y);
		//to remove self loops
		((ArrayList<String>)graph.get(newKey)).replaceAll(operator);
		while (((ArrayList<String>) graph.get(newKey)).remove(newKey));
 		
		return ComputeMinCut(GetNextRandom());
	}
	private static Tuple<String,String> GetNextRandom(){
		Random generator = new Random();
		String randomKeyFrom = graph.keySet().toArray()[generator.nextInt(graph.size())].toString();
		ArrayList<String> randomKeyValueSet =  (ArrayList<String>) graph.get(randomKeyFrom);
		String randomKeyTo = randomKeyValueSet.get(generator.nextInt(randomKeyValueSet.size()));
		//System.out.printf("\nEdge-X:%s\tEdge-Y:%s", randomKeyFrom, randomKeyTo);
		return new Tuple<String,String>(randomKeyFrom, randomKeyTo);
	}
	
}
