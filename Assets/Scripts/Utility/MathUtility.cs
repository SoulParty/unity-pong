using System;
using UnityEngine;

public class MathUtility {
    public static double convertToRadians(double angle) {
        return (Math.PI * angle) / 180;
    }

    public static double roundToNearestHalf(double number) {
        return Math.Round(number / 100, MidpointRounding.AwayFromZero) / 0.02;
    }

    public static double roundToNearestTenth(double number) {
        return ((int) (number / 10)) / 0.1;
    }

    public static double convertToDegrees(double radians) {
        return (180 * radians) / Math.PI;
    }

    public static double angleSanityCheck(double radians) {
        if (radians > 1.57) {
            return 3.14 - radians;
        }
        return radians;
    }
}
