#pragma once



#define MYAPI __declspec(dllexport)

extern "C" {

MYAPI int Add(int a, int b);


MYAPI int AddArray(int * x, int n);
MYAPI void ModArray(int *x, int n);

MYAPI void TakeStepDLL(int * toDelete, double * x, double *y, double *z,
	double *vx, double *vy, double *vz,
	double * ax, double * ay, double * az,
	double *m, double * hillRadius,  int n, double dt, double G, double shieldRadius,
	double massCutoff);




}


