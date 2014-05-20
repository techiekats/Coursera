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
			//QuickSortWithFirstElementPivot(0, sorted.length -1);
			QuickSortWithLastElementPivot(0, sorted.length -1);
			//QuickSortWithMedianElementPivot(0, sorted.length -1);
			/*for (int i = 0; i < sorted.length; i++)
			{
				System.out.println(sorted[i]);
			}*/
			System.out.printf("\n%d",new Long[]{Long.valueOf(comparisonCount)});
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

public static void QuickSortWithFirstElementPivot (int startIndex, int endIndex) throws Exception
{
	if (endIndex - startIndex <= 1)
	{
		if (endIndex - startIndex < 1)
		{
			return;
		}
		comparisonCount ++;
		if (sorted[startIndex] > sorted[endIndex])
		{
		
			long temp = sorted[startIndex];
			sorted[startIndex] = sorted[endIndex];
			sorted[endIndex] = temp;
		}
		return;
	}
	int partition = Partition (startIndex + 1, endIndex, sorted[startIndex]);
	//swap
	long temp = sorted[partition];
	sorted[partition] = sorted[startIndex];
	sorted[startIndex] = temp;
	
	QuickSortWithFirstElementPivot (startIndex, partition-1);
	QuickSortWithFirstElementPivot (partition + 1, endIndex);
	
}


public static void QuickSortWithLastElementPivot (int startIndex, int endIndex) throws Exception
{
	if (endIndex - startIndex <= 1)
	{
		if (endIndex - startIndex < 1)
		{
			return;
		}
		comparisonCount ++;
		if (sorted[startIndex] > sorted[endIndex])
		{
		
			long temp = sorted[startIndex];
			sorted[startIndex] = sorted[endIndex];
			sorted[endIndex] = temp;
		}
		return;
	}
	long temp =  sorted[endIndex];
	 sorted[endIndex] = sorted[startIndex];
	 sorted[startIndex] = temp;
	int partition = Partition (startIndex+1, endIndex, sorted[startIndex]);
	//swap
	temp = sorted[partition];
	sorted[partition] = sorted[startIndex];
	sorted[startIndex] = temp;
	
	QuickSortWithLastElementPivot (startIndex, partition - 1);
	QuickSortWithLastElementPivot (partition + 1, endIndex);
	
}

public static void QuickSortWithMedianElementPivot (int startIndex, int endIndex) throws Exception
{
	if (endIndex - startIndex <= 1)
	{
		if (endIndex - startIndex < 1)
		{
			return;
		}
		comparisonCount ++;
		if (sorted[startIndex] > sorted[endIndex])
		{
		
			long temp = sorted[startIndex];
			sorted[startIndex] = sorted[endIndex];
			sorted[endIndex] = temp;
		}
		return;
	}
	int centerElementIndex = (endIndex-startIndex + 1)% 2 == 0 ? startIndex + (endIndex - startIndex) / 2 : startIndex + (endIndex - startIndex + 1) / 2;
	long median = InsertionSort (new long[]{sorted[startIndex], sorted[centerElementIndex], sorted[endIndex]})[1];
	//pre-processing. exchange median and first element
	
	if (median == sorted[centerElementIndex])
	{
		sorted[centerElementIndex] = sorted[startIndex];
		sorted[startIndex] = median;
	}
	if (median == sorted[endIndex])
	{
		sorted[endIndex] = sorted[startIndex];
		sorted[startIndex] = median;
	}
	int partition = Partition (startIndex+1, endIndex, median);
	//swap
	long temp = sorted[partition];
	sorted[partition] = sorted[startIndex];
	sorted[startIndex] = temp;
	
	QuickSortWithMedianElementPivot (startIndex, partition-1);
	QuickSortWithMedianElementPivot (partition + 1, endIndex);
	
}
private static long[]  InsertionSort (long[] array)
{
	int startIndex = 0;
	int endIndex = array.length - 1;
    for (int j = startIndex+1; j <= endIndex; j++) {
        long key = array[j];
        int i = j-1;
        while ( (i >= startIndex) && ( array [i] > key ) ) {
        	long temp = array[i+1];
        	array [i+1] = array [i];
        	array[i] = temp;
            i--;	
        }
        //sorted[i+1] = key;    
    }  
    return array;
}

//will return the last element position till where the elements are l.t. pivot
private static int Partition (int startIndex, int endIndex, long pivot)
{
	/*System.out.printf("\nAdding.. %d \t Pivot = %d\t{", new Long[]{Long.valueOf(endIndex-startIndex + 1), Long.valueOf(pivot)});
	for (int k = startIndex; k<= endIndex; k++)
	{
		System.out.printf("\t%d", new Long[]{Long.valueOf(sorted[k])});
	}
	System.out.print('}');*/
	comparisonCount += (endIndex-startIndex+1); 
	int sortedRegionBoundary = startIndex;
	int lesserThanPivotRegionBoundary = startIndex - 1;
	while (sortedRegionBoundary <= endIndex)
	{
		if (sorted[sortedRegionBoundary] < pivot)
		{
			lesserThanPivotRegionBoundary++;
			if (sortedRegionBoundary != lesserThanPivotRegionBoundary)
			{
				long temp = sorted[sortedRegionBoundary];
				sorted[sortedRegionBoundary] = sorted [lesserThanPivotRegionBoundary] ;
				sorted [lesserThanPivotRegionBoundary]  = temp;
			}
		}
		sortedRegionBoundary ++;
	}
	return lesserThanPivotRegionBoundary;
}
}
