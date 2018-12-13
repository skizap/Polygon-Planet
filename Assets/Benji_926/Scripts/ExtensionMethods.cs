﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{	
	// Returns a two-dimensional vector going in the same direction
	// as baseVector but with the specified magnitude
	public static Vector2 ScaledVector (this Vector2 baseVector, float magnitude)
	{
		float newX;	// X-component of vector to be returned
		float newY;	// Y-component of vector to be returned

		// Start by setting components to return to zero
		newX = 0;
		newY = 0;

		// Check if base vector is negligibly small to prevent divide by zero error
		if (baseVector.sqrMagnitude > Mathf.Epsilon) {
			// Inverse of the magnitude; stored for efficiency
			float inverseBaseMagnitude = 1f / baseVector.magnitude;
			// Calculate the components to be returned using ratio of corresponding parts
			// (given vector and desired vector form similar triangles, remember?)
			newX = (magnitude * baseVector.x) * inverseBaseMagnitude;
			newY = (magnitude * baseVector.y) * inverseBaseMagnitude;
		} // END if

		return new Vector2 (newX, newY);
	} // END method

    // Return a version of the vector passed in, rotated theta DEGREES counter-clockwise
    public static Vector2 RotatedVector (this Vector2 v, float theta)
    {
        float initAngle = Vector2.Angle(v, Vector2.right);  // V's initial angle from the x-axis
        float componentAngle;   // Angle used to calculate v's new x-y components
        float magnitude = v.magnitude;  // Magnitude of the vector, queued for efficiency

        // If v is in the 3rd or 4th quadrants,
        // we know the angle should be negative
        if (v.y < 0f)
        {
            initAngle *= -1f;
        }

        // Calculate and convert the angle used to find the new x-y components
        componentAngle = initAngle + theta;
        componentAngle *= Mathf.Deg2Rad;

        // Return the sign and cosine of the angle
        return new Vector2(magnitude * Mathf.Cos(componentAngle), magnitude * Mathf.Sin(componentAngle));
    }

	// Convert an array of a specified type to a list of the same type
	public static List<T> ToList<T> (this T[] array)
	{
		// Define a local list to return
		List<T> localList = new List<T> ();

		// Add each item in the array to the list
		foreach (T item in array) {
			localList.Add (item);
		} // END foreach

		return localList;
	} // END method

	public static void Reset (this Transform trans)
	{
		trans.position = Vector3.zero;
		trans.rotation = Quaternion.Euler (Vector3.zero);
		trans.localScale = Vector3.one;
	}
}
