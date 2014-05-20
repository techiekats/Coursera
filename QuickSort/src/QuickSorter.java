import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;
/**
 * @author khyati.shah
 *
 */
public class QuickSorter {

	private static long[] sorted;
	private static long comparisonCount = 0;
	public static void main(String[] args) throws Exception {
		Path file = Paths.get(System.getProperty("user.home"), new String[]{"Documents/Projects/Coursera/QuickSort", "QuickSort.txt"});
		Object[] fileLines;
		try {
			fileLines = Files.lines(file).toArray();
			sorted = new long[fileLines.length];
			for (int i = 0; i < fileLines.length; i++)
			{
				sorted[i] = Long.parseLong(fileLines[i].toString());
			}
			//UTC 1: InversionCount = 45
			//sorted = new long[]{10,9,8,7,6,5,4,3,2,1};
			//UTC 2: InversionCount = 0
			//sorted = new long[]{1,2,3,4,5,6,7,8,9,10};
			//UTC 3: InversionCount = 23
			//sorted = new long[] {2, 534, 33, 54, 245, 75, 3, 13, 14, 62};
			QuickSortWithFirstElementPivot(0, sorted.length -1);

			System.out.println(comparisonCount);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

public static void QuickSortWithFirstElementPivot (int startIndex, int endIndex) throws Exception
{
	
}

}
