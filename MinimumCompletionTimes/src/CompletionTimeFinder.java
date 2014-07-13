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
import java.util.function.Consumer;

public class CompletionTimeFinder {

	private static ArrayList<Tuple <Float, Integer, Integer>> jobs;

	public static void main(String[] args) {
		LoadDataFromFile();
		//LoadTestData2();
		//LoadTestData1();
		System.out.println ("Greedy completion time (weight - length) = " + ComputeMinCompletionTimeFromDifference());
		System.out.println ("Greedy completion time (weight / length) = " + ComputeMinCompletionTimeFromRatio());
	}

	private static void LoadDataFromFile ()
	{
		/*
		 * 	Greedy completion time (weight - length) = 69119377652
			Greedy completion time (weight / length) = 67311454237
		 * */
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/MinimumCompletionTimes", "jobs.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			jobs = new ArrayList<Tuple <Float, Integer, Integer>> ();
			
			for (int i = 1; i < fileLines.length; i++)
			{
				String[] row = fileLines[i].toString().split(" ");
				
				Tuple<Float, Integer,Integer> t = new Tuple<Float ,Integer, Integer>(0.0f, Integer.parseInt(row[0]), Integer.parseInt(row[1]));
			
				jobs.add (t);
			}	
	
		
	}
	private static void LoadTestData1 ()
	{
		//Answer #1: 11336, #2: 10548

		jobs = new ArrayList <Tuple <Float, Integer, Integer>> (5);
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 48, 14));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 4, 90));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 64, 22));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 54, 66));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 46, 6));
		
	}
	
	private static void LoadTestData2 ()
	{
		//Answer #1: 145924, #2: 138232

		jobs = new ArrayList <Tuple <Float, Integer, Integer>> (18);
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 50, 18));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 10, 44));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 94, 8));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 30, 26));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 98, 68));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 78, 6));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 2, 56));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 54, 20));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 30, 40));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 56, 62));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 60, 22));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 92, 10));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 98, 52));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 4, 52));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 80, 36));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 12, 88));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 32, 86));
		jobs.add(new Tuple <Float, Integer, Integer> (0.0f, 8, 88));

	}
	
	private static long ComputeMinCompletionTimeFromDifference ()
	{
		Consumer<Tuple<Float, Integer, Integer>> action = new Consumer<Tuple<Float, Integer, Integer>>(){

			public void accept(Tuple<Float, Integer, Integer> t) {
				t.x = (float)t.y - (float)t.z;
				
			}} ;
		jobs.forEach(action);
		Comparator<Tuple<Float, Integer, Integer>> c = new Comparator<Tuple<Float, Integer, Integer>>(){

			public int compare(Tuple<Float, Integer, Integer> o1,
					Tuple<Float, Integer, Integer> o2) {
				if (Float.compare(o1.x, o2.x) == 0)
				{
					return -(o1.y - o2.y);
				}
				return (int) -(o1.x - o2.x);
			}
		};
		jobs.sort(c);
		long sum = 0, weightedCompletionTime=0;

		for (Tuple<Float, Integer, Integer> t: jobs)
		{
			sum += t.z;
			weightedCompletionTime += (sum * t.y);
		}
		return weightedCompletionTime;
	}
	private static long ComputeMinCompletionTimeFromRatio ()
	{
		Consumer<Tuple<Float, Integer, Integer>> action = new Consumer<Tuple<Float, Integer, Integer>>(){

			public void accept(Tuple<Float, Integer, Integer> t) {
				t.x = (float) t.y / (float) t.z;
				
			}} ;
		jobs.forEach(action);
		Comparator<Tuple<Float, Integer, Integer>> c = new Comparator<Tuple<Float, Integer, Integer>>(){

			public int compare(Tuple<Float, Integer, Integer> o1,
					Tuple<Float, Integer, Integer> o2) {
				return Float.compare(o2.x, o1.x); //since we want descending sorting
			}
		};
		jobs.sort(c);
		long sum = 0, weightedCompletionTime=0;

		for (Tuple<Float, Integer, Integer> t: jobs)
		{
			sum += t.z;
			weightedCompletionTime += (sum * t.y);
		}
		return weightedCompletionTime;
	}
}
