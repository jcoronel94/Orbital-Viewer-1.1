using System.Collections;
using System.Runtime.InteropServices;

public class Wrappers : System.Object {

	[DllImport("OVTakeStep")]
	private static extern int Add(int a, int b);

	[DllImport("OVTakeStep", CallingConvention=CallingConvention.Cdecl)]
	private static extern int AddArray(int []a, int n);

	[DllImport("OVTakeStep", CallingConvention=CallingConvention.Cdecl)]
	private static extern void ModArray(int []a, int n);

	[DllImport("OVTakeStep", CallingConvention=CallingConvention.Cdecl)]
	private static extern void TakeStepDLL(int [] toDelete, double [] x, double [] y, double [] z,
		double [] vx, double [] vy, double [] vz, double [] ax, double [] ay, double [] az,
		double [] m, double [] hillRadius,  
		int n, double dt, double G, double shieldRadius, double massCutoff);

	public static int Wrapper_add(int a, int b) {
		return Add (a, b);
		//return a+b;
	} 

	public static int Wrapper_addArray(int []a, int n) {
		return AddArray (a, n);
		//return a+b;
	} 

	public static void Wrapper_modArray(int [] a, int n) {
		ModArray (a, n);
	}

	public static void Wrapper_TakeStepDLL(int [] toDelete, double [] x, double [] y, double [] z,
		double [] vx, double [] vy, double [] vz, double [] ax, double [] ay, double [] az,
		double [] m, double [] hillRadius, int n, double dt, double G, double shieldRadius, double massCutoff) {
		TakeStepDLL (toDelete, x, y, z, vx, vy, vz, ax, ay, az, m, hillRadius, n, dt, G, shieldRadius, massCutoff);
	}






}
