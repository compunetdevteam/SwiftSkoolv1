﻿using System;
using System.Runtime.Serialization;

namespace SwiftSkool.Services
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class DataPoint
    {

        public DataPoint()
        {

        }
        public DataPoint(double y, string label, string label2)
        {
            this.Y = y;
            this.Label = label;
        }
        public DataPoint(double y, string label)
        {
            this.Y = y;
            this.Label = label;
        }


        public DataPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        public DataPoint(double x, double y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DataPoint(double x, double y, double z, string label)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Label = label;
        }


        //Explicitly setting the name to be used while serializing to JSON. 
        [DataMember(Name = "label")]
        public string Label = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "x")]
        public Nullable<double> X = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "z")]
        public Nullable<double> Z = null;

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "legendText")]
        public string LegendText;
    }
}