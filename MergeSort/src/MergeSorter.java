import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.*;

public class MergeSorter {

private static long INSERTION_SORT_INVOCATION_THRESHOLD = 100;
private static long[] sorted;
private static long inversionCount = 0;
public static void main(String[] args) throws Exception {
		Path file = Paths.get(System.getProperty("user.home"), "Documents/Projects/MergeSort", "IntegerArray.txt");
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
			MergeSort(0, sorted.length -1);

			System.out.printf("\nInversion Count=%d", inversionCount);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
public static void MergeSort (int startIndex, int endIndex) throws Exception
{
	if ((endIndex - startIndex) < INSERTION_SORT_INVOCATION_THRESHOLD)
	{
		if ((endIndex - startIndex) < 5)
		{
			throw new Exception("difference less than 5:" + startIndex +"\t" + endIndex + "\t" + (endIndex-startIndex));
		}
		System.out.printf("Invoking insertion subroutine from %d to %d..\n", startIndex, endIndex);
		InsertionSort (startIndex, endIndex);
	}
	else
	{
		System.out.printf("Invoking Merge sort from %d to %d", startIndex, endIndex);
		MergeSort (startIndex, startIndex + (endIndex-startIndex)/2);
		MergeSort (startIndex + (endIndex-startIndex)/2 + 1, endIndex);
		System.out.printf("Invoking merge subroutine from %d to %d..\n", startIndex, endIndex);
		Merge (startIndex, endIndex);
	}	
}
public static void InsertionSort (int startIndex, int endIndex)
{
	
    for (int j = startIndex+1; j <= endIndex; j++) {
        long key = sorted[j];
        int i = j-1;
        while ( (i >= startIndex) && ( sorted [i] > key ) ) {
        	long temp = sorted[i+1];
            sorted [i+1] = sorted [i];
            sorted[i] = temp;
            i--;	inversionCount ++;
        }
        //sorted[i+1] = key;    
    }  
}
public static void Merge (int startIndex, int endIndex)
{
	long [] lhs = Arrays.copyOfRange(sorted, startIndex, (endIndex-startIndex)/2 + 1 + startIndex); //in copyofRange, the to index is exclusive
	long [] rhs = Arrays.copyOfRange(sorted, startIndex + (endIndex-startIndex)/2 + 1, endIndex + 1);

	int lhsIndex = 0, rhsIndex = 0;
	
	for (int i = startIndex; i <= endIndex; i++)
	{
		if (lhsIndex == lhs.length)
		{
			System.out.println("To copy remainder of right hand side array. Hence returning");
			return;
		}
		if (rhsIndex == rhs.length)
		{
			System.out.println("To copy remainder of left hand side array.");
			while (lhsIndex < lhs.length)
			{
				sorted[i++] = lhs[lhsIndex++];
				inversionCount+= (rhs.length - rhsIndex);
			}
			return;
		}
		if (lhs[lhsIndex] < rhs[rhsIndex])
		{
			sorted[i] =  lhs[lhsIndex++];
		}
		else
		{
			inversionCount += (lhs.length - lhsIndex);	
			sorted[i] =  rhs[rhsIndex++];	
		}
					 
	}
}
}

