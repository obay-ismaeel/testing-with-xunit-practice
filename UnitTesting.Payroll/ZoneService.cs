﻿using System;
using UnitTesting.Payroll;

namespace UnitTesting.Payroll;

public class ZoneService: IZoneService
{
    private static Random random = new Random();
    public bool IsDangerZone(string dutyStation)
    {
        // Huge Logic Goes here

        // 1 / 10 probability 
        return random.Next(1, 10) == 3;
    }
}
