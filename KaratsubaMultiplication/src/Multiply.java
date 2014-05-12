public class Multiply {

	public static void main(String[] args) {
		
		System.out.println("\nMultiplication: 12345 * 5678=" + multiply(12345, 5678));
	}
	private static int multiply (int multiplicand1, int multiplicand2)
	{
		//For 3 digit numbers, directly send output
		if ((multiplicand1/1000 == 0) && multiplicand2/1000 == 0)
		{
			return multiplicand1 * multiplicand2;
		}
		int n = Max(numberOfDigits (multiplicand1), numberOfDigits(multiplicand2));
		if (n%2 != 0)
			n+=1;
		int a = multiplicand1 / powerBase10 (1,n/2);
		int b = multiplicand1 % powerBase10(1,n/2);
		int c = multiplicand2 / powerBase10 (1, n/2);
		int d = multiplicand2 % powerBase10(1, n/2);
		System.out.printf("A=%d B=%d C=%d D=%d\n", a,b,c,d);
		int ac = multiply (a,c);
		int bd = multiply (b,d);
		int term1 = powerBase10 (ac, n);
		int term2 = bd;
		int term3 = powerBase10 (multiply (a+b, c+d) -ac -bd, n/2);
		System.out.printf("\nTerm1=%s\tTerm2=%s\tTerm3=%s", term1, term2, term3);
		return term1 + term2 + term3;
	}
	private static int numberOfDigits (int n)
	{
		System.out.printf("Number=%s", n);
		int power = 0;
		while (n/10 != 0 || n != 0)
		{
			n /= 10;
			power += 1;
		}
		System.out.printf("\tDigits=%s\n",power);
		return power;
	}
	private static int powerBase10(int base, int index)
	{
		System.out.printf("Base=%s\tIndex=%s",base, index);
		while (index != 0)
		{
			base *= 10;
			index -= 1;
		}
		System.out.println("\tResult=" + base);
		return base;
	}
	private static int Max (int n1, int n2)
	{
		if (n1 >= n2) return n1;
		return n2;
	}
}
