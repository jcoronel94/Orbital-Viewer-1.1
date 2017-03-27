
#include "OVTakeStep.h"
#include <math.h>

extern "C" {

	MYAPI int Add(int a, int b) {
		return a + b;
	}


	MYAPI int AddArray(int * x, int n) {
		int sum = 0;
		for (int j = 0; j < n; j++) {
			sum += x[j];
		}
		return sum;
	}

	MYAPI void ModArray(int *x, int n) {
		for (int j = 0; j < n; j++) {
			x[j] += 1;
		}
	}

	MYAPI void TakeStepDLL(int * toDelete, double * x, double *y, double *z,
		double *vx, double *vy, double *vz,
		double * ax, double * ay, double * az,
		double *m, double * hillRadius,  int n, double dt, double G, double shieldRadius,
		double massCutoff) {

		for (int j = 0; j<n; j++) {
			ax[j] = 0.0;
			ay[j] = 0.0;
			az[j] = 0.0;
		}

		for (int j = 0; j<n; j++) {
			for (int k = j + 1; k<n; k++) {
				if (m[j] > massCutoff || m[k] > massCutoff) {
					double dx = (x[k] - x[j]);
					double dy = (y[k] - y[j]);
					double dz = (z[k] - z[j]);
					double dr = sqrt(dx*dx + dy*dy + dz*dz);
					double dr3 = dr*dr*dr;
					if (dr < hillRadius[k] && m[j] <= massCutoff && m[k] > massCutoff) {
						toDelete[j] = k;
					}
					else if (dr < hillRadius[j] && m[k] <= massCutoff && m[j] > massCutoff) {
						toDelete[k] = j;
					}
					if (dr > shieldRadius) {
						if (m[k] > massCutoff) {
							ax[j] += G * m[k] / dr3 * dx;
							ay[j] += G * m[k] / dr3 * dy;
							az[j] += G * m[k] / dr3 * dz;
						}
						if (m[j] > massCutoff) {
							ax[k] -= G * m[j] / dr3 * dx;
							ay[k] -= G * m[j] / dr3 * dy;
							az[k] -= G * m[j] / dr3 * dz;
						}
					}
					
				}
			}
		}

		for (int j = 0; j<n; j++) {
			vx[j] += ax[j] * dt;
			vy[j] += ay[j] * dt;
			vz[j] += az[j] * dt;
			x[j] += vx[j] * dt;
			y[j] += vy[j] * dt;
			z[j] += vz[j] * dt;
		}

		return;

	}






}

