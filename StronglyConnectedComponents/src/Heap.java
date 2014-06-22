import java.io.*;
public class Heap {
	   private Long[] heapArray;
	   private int maxSize;           // size of array
	   private int currentSize;       // number of nodes in array
	   private final boolean isMaxHeap;
	// -------------------------------------------------------------
	   public Heap(int mx, boolean type)            // constructor
	      {
	      maxSize = mx;
	      currentSize = 0;
	      heapArray = new Long[maxSize];  // create array
	      isMaxHeap = type;
	      }
	// -------------------------------------------------------------
	   public boolean isEmpty()
	      { return currentSize==0; }
	// -------------------------------------------------------------
	   public boolean insert(Long key)
	      {
	      if(currentSize==maxSize)
	         return false;
	      Long newNode = new Long(key);
	      heapArray[currentSize] = newNode;
	      trickleUp(currentSize++);
	      return true;
	      }  // end insert()
	// -------------------------------------------------------------
	   public void trickleUp(int index)
	      {
	      int parent = (index-1) / 2;
	      Long bottom = heapArray[index];
	      if (isMaxHeap)
	      {
	      while( index > 0 &&
	             heapArray[parent] < bottom )
	         {
	         heapArray[index] = heapArray[parent];  // move it down
	         index = parent;
	         parent = (parent-1) / 2;
	         }  // end while
	      }
	      else
	      {
	    	  while( index > 0 &&
	 	             heapArray[parent] > bottom )
	 	         {
	 	         heapArray[index] = heapArray[parent];  // move it down
	 	         index = parent;
	 	         parent = (parent-1) / 2;
	 	         }  // end while
	      }
	      heapArray[index] = bottom;
	      }  // end trickleUp()
	// -------------------------------------------------------------
	   public Long remove()           // delete item with max key
	      {                           // (assumes non-empty list)
		   Long root = heapArray[0];
	      heapArray[0] = heapArray[--currentSize];
	      trickleDown(0);
	      return root;
	      }  // end remove()
	   public Long get ()
	   {
		   Long root = heapArray[0];
   
		return root;
		   
	   }
	   public Integer size()
	   {
		   return currentSize;
	   }
	// -------------------------------------------------------------
	   public void trickleDown(int index)
	      {
	      int largerChild;
	      Long top = heapArray[index];       // save root
	      while(index < currentSize/2)       // while node has at
	         {                               //    least one child,
	         int leftChild = 2*index+1;
	         int rightChild = leftChild+1;
	         
	         if (isMaxHeap)
	         {
	                                         // find larger child
	         if(rightChild < currentSize &&  // (rightChild exists?)
	                             heapArray[leftChild] <
	                             heapArray[rightChild])
	            largerChild = rightChild;
	         else
	            largerChild = leftChild;
	                                         // top >= largerChild?
	         if( top >= heapArray[largerChild])
	            break;
	         heapArray[index] = heapArray[largerChild];
	         index = largerChild;            // go down
	         }                  // shift child up
	         else
	         {
	        	 if(rightChild < currentSize &&  // (rightChild exists?)
                         heapArray[leftChild] >
                         heapArray[rightChild])
			        largerChild = rightChild;
			     else
			        largerChild = leftChild;
			                                     // top >= largerChild?
			     if( top <= heapArray[largerChild])
			        break;
			     heapArray[index] = heapArray[largerChild];
			     index = largerChild;            // go down
	         }
	         
	         }  // end while
	      heapArray[index] = top;            // root to index
	      }  // end trickleDown()
	// -------------------------------------------------------------
	   public boolean change(int index, Long newValue)
	      {
	      if(index<0 || index>=currentSize)
	         return false;
	      Long oldValue = heapArray[index]; // remember old
	      heapArray[index]=newValue;  // change to new

	      if(oldValue < newValue)             // if raised,
	         trickleUp(index);                // trickle it up
	      else                                // if lowered,
	         trickleDown(index);              // trickle it down
	      return true;
	      }  // end change()
	// -------------------------------------------------------------
	   public void displayHeap()
	      {
	      System.out.print("heapArray: ");    // array format
	      for(int m=0; m<currentSize; m++)
	         if(heapArray[m] != null)
	            System.out.print( heapArray[m] + " ");
	         else
	            System.out.print( "-- ");
	      System.out.println();
	                                          // heap format
	      int nBlanks = 32;
	      int itemsPerRow = 1;
	      int column = 0;
	      int j = 0;                          // current item
	      String dots = "...............................";
	      System.out.println(dots+dots);      // dotted top line

	      while(currentSize > 0)              // for each heap item
	         {
	         if(column == 0)                  // first item in row?
	            for(int k=0; k<nBlanks; k++)  // preceding blanks
	               System.out.print(' ');
	                                          // display item
	         System.out.print(heapArray[j]);

	         if(++j == currentSize)           // done?
	            break;

	         if(++column==itemsPerRow)        // end of row?
	            {
	            nBlanks /= 2;                 // half the blanks
	            itemsPerRow *= 2;             // twice the items
	            column = 0;                   // start over on
	            System.out.println();         //    new row
	            }
	         else                             // next item on row
	            for(int k=0; k<nBlanks*2-2; k++)
	               System.out.print(' ');     // interim blanks
	         }  // end for
	      System.out.println("\n"+dots+dots); // dotted bottom line
	      }  // end displayHeap()
	// -------------------------------------------------------------
	   
}
