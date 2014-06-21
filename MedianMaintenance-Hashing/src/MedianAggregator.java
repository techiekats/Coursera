import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;


@SuppressWarnings("unused")
public class MedianAggregator {

	private static ArrayList<Integer> inputStream;
	private static int maxHeapSize = 5000;
	private static int sumOfMedians = 0;
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		inputStream = new ArrayList<Integer>();
		Heap minHeap = new Heap(maxHeapSize, true);
		Heap maxHeap = new Heap (maxHeapSize, false);
		//LoadInputFromTestData();
		LoadInputStreamFromFile();
		for (int i = 0; i< inputStream.size(); i++)
		{
			int current = inputStream.get(i);
			if (minHeap.size() == 0)
				minHeap.insert(current);
			else
			{
				if (current < minHeap.get())
				{
					minHeap.insert(current);
				}
				else
				{
					maxHeap.insert(current);
				}
			}
			int median = 0;
			//check Heap Sizes
			int sizeDiff = minHeap.size() - maxHeap.size();
			if (Math.abs(sizeDiff) > 1)
			{
				if (sizeDiff < 0)
				{
					minHeap.insert(maxHeap.remove());
				}
				else
				{
					maxHeap.insert(minHeap.remove());
				}			
			}
			
			median = minHeap.size() >= maxHeap.size() ? minHeap.get() : maxHeap.get();
			System.out.println ("Current:" + current+ "\tMedian:" + median);
			sumOfMedians += median;
		}
		System.out.println("Sum of medians(mod): " + sumOfMedians%10000);
	}
	private static void LoadInputFromTestData ()
	{
		inputStream.add(5);
		inputStream.add(10);
		inputStream.add(54);
		inputStream.add(15);
		inputStream.add(4);
		inputStream.add(7);
		inputStream.add(9);
		inputStream.add(4); //duplicate
		inputStream.add(14);
		inputStream.add(12);
		inputStream.add(19);
	}

	private static void LoadInputStreamFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/MedianMaintenance-Hashing", "Median.txt"});
		Object[] fileLines = null;
	
			try {
				fileLines = Files.lines(file).toArray();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			for (int i = 0; i < fileLines.length; i++)
			{
				ArrayList <String> adjListRow = new ArrayList<String>(Arrays.asList(fileLines[i].toString()));
				String value = adjListRow.get(0);
				inputStream.add(Integer.parseInt(value));
			}
		
	}
}
