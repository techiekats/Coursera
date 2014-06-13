import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;


public class ShortestPathComputer {

	private final int INFINITY=1000000;
	private static HashMap<String, ArrayList<Tuple<String, Integer>>> graph;

	public static void main(String[] args) {
		graph = new HashMap<String, ArrayList<Tuple<String, Integer>>>();
		LoadGraphFromFile();
		int shortestPath = ComputeDijkstraShortestPath();

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

	private static int ComputeDijkstraShortestPath ()
	{
		return 0;
	}
}
