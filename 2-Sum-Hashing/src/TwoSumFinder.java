import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map.Entry;

public class TwoSumFinder {

	//Key = Long, Value = numbers in the range for which pair exists
	private static HashMap<Long,Long> inputLoad;
	private static HashMap<Long,Long> duplicateKeys;
	private static Integer duplicatePairsCountAvailableForCount ;
	public static void main(String[] args) {
		inputLoad = new HashMap<Long, Long>();
		duplicateKeys = new HashMap<Long, Long>();
		duplicatePairsCountAvailableForCount = new Integer(0);
		//LoadHashTableFromTest();
		LoadGraphFromFile();
		//LoadHashTableFromTest2();

		for (long i = -10000; i<= 10000; i++)
		{
			duplicatePairsCountAvailableForCount +=  (IsDistinctPairAvailable(i) ? 1 : 0);
			System.out.println("Computed Duplicate Count For: " + i + " Value:"+ duplicatePairsCountAvailableForCount);			
		}
		//duplicatePairsCount = FindDistinctSumPairsFor((long)100);

		System.out.print(duplicatePairsCountAvailableForCount);
		
	}
	private static void LoadHashTableFromTest()
	{
		inputLoad.put(new Long(-100), Long.MAX_VALUE);
		inputLoad.put(new Long(25), Long.MAX_VALUE);
		inputLoad.put(new Long(33),	Long.MAX_VALUE);
		inputLoad.put(new Long(39), Long.MAX_VALUE);
		inputLoad.put(new Long(50), Long.MAX_VALUE);
		inputLoad.put(new Long(75), Long.MAX_VALUE);
		inputLoad.put(new Long(200), Long.MAX_VALUE);
		if (null != inputLoad.put(new Long(50), Long.MAX_VALUE))
		{
			duplicateKeys.put(new Long(100),Long.MAX_VALUE);
		}
		if (null != inputLoad.put((long)25, Long.MAX_VALUE))
		{
			duplicateKeys.put(new Long(50),Long.MAX_VALUE);
		}
	}
	
	private static void LoadHashTableFromTest2()
	{
		inputLoad.put(new Long(-2), Long.MAX_VALUE);
		inputLoad.put(new Long(1), Long.MAX_VALUE);
		inputLoad.put(new Long(2), Long.MAX_VALUE);
		inputLoad.put(new Long(3),	Long.MAX_VALUE);
		inputLoad.put(new Long(4), Long.MAX_VALUE);
		inputLoad.put(new Long(5), Long.MAX_VALUE);
		inputLoad.put(new Long(6), Long.MAX_VALUE);
		inputLoad.put(new Long(7), Long.MAX_VALUE);
		inputLoad.put(new Long(8), Long.MAX_VALUE);
		inputLoad.put(new Long(9), Long.MAX_VALUE);
		inputLoad.put(new Long(10), Long.MAX_VALUE);
		inputLoad.put(new Long(11), Long.MAX_VALUE);
		inputLoad.put(new Long(12), Long.MAX_VALUE);
		
		if (null != inputLoad.put((long)5, Long.MAX_VALUE))
		{
			duplicateKeys.put(new Long(10),Long.MAX_VALUE);
		}
		if (null != inputLoad.put((long)6, Long.MAX_VALUE))
		{
			duplicateKeys.put(new Long(12),Long.MAX_VALUE);
		}
	}
	private static void LoadGraphFromFile()
	{
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/2-Sum-Hashing", "2sum.txt"});
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
				
				if (null != inputLoad.put(Long.parseLong(value), Long.MAX_VALUE))
				{			
					duplicateKeys.put(Long.parseLong(value) * 2,Long.MAX_VALUE);
				}
			}
			System.out.println("Loaded HashTable. Count = " + inputLoad.size());
			System.out.println("Loaded Duplicate Entries Table. Count = " + duplicateKeys.size());
		
	}
	
	private static Boolean IsDistinctPairAvailable (final Long sum)
	{
		Iterator<Entry<Long,  Long>> it = inputLoad.entrySet().iterator();
		while (it.hasNext())
		{
			Entry<Long,  Long> entry = it.next();
			if (sum - entry.getKey() == entry.getKey())
			{
				continue;
			}
			Long pairEntry = inputLoad.get(sum - entry.getKey());
			if (null != pairEntry){
				//check if it is duplicate
				if (!pairEntry.equals(entry.getKey())){
					return true;
				}
			}
		}

		return duplicateKeys.get(sum)!= null;
		
	}
	
	private static Long FindDistinctSumPairsFor (final Long sum)
	{
		Long pairCount = (long)0;
		Iterator<Entry<Long,  Long>> it = inputLoad.entrySet().iterator();
		while (it.hasNext())
		{
			Entry<Long,  Long> entry = it.next();
			if (sum - entry.getKey() == entry.getKey())
			{
				continue;
			}
			Long pairEntry = inputLoad.get(sum - entry.getKey());
			if (null != pairEntry){
				//check if it is duplicate
				if (!pairEntry.equals(entry.getKey())){
					pairCount ++;
					inputLoad.put(sum - entry.getKey() , entry.getKey());
					inputLoad.put(entry.getKey(), sum - entry.getKey());
				}
			}
		}
		pairCount += (duplicateKeys.get(sum)== null ? 0 : 1);

		return pairCount;
		
	}
}
