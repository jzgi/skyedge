﻿using System;
using SkyGate.Feature;

namespace SkyGate.Driver
{
    public class CameraRecognizerDriver : Driver, IRecog
    {
        public override void Test()
        {
            throw new NotImplementedException();
        }

        public double Loss(float output, float label)
        {
            throw new NotImplementedException();
        }

        public float Derivative(float output, float label)
        {
            throw new NotImplementedException();
        }

        public int GetItemIdByScan()
        {
            throw new NotImplementedException();
        }

        public int GetNumberByScan()
        {
            throw new NotImplementedException();
        }
    }
}