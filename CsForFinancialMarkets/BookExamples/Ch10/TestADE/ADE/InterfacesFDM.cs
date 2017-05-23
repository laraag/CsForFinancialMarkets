// Interfaces.cs
//
// 23 Januari 2006 Jannes Albrink
// 2008-12-15 DD generic interfaces for 2d access
// 2012-11-15 DD clean-up
//
// (C) Datasim Component Technology 2006-2013

using System;


/// <summary>
/// Summary description for Interfaces.
/// </summary>
public interface IIBVPImp
{
    double diffusion(double x, double t);	// Coefficient of second derivative
    double convection(double x, double t);	// Coefficient of first derivative
    double zeroterm(double x, double t);	// Coefficient of zero derivative
    double RHS(double x, double t);			// Inhomogeneous forcing term
    
    // Boundary and initial conditions
    double BCL(double t);					// Left hand boundary condition
    double BCR(double t);					// Right hand boundary condition
    double IC(double x);					// Initial condition
    double Constraint(double x);            // Possible early exercise and other constraints
}

public interface IOptionFactory
{
    Option CreateOption();
}