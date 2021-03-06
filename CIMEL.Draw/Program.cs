﻿using CIMEL.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CIMELDrawNative;

namespace CIMEL.Draw
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // draw aeronent inversion
            Drawing drawing = new Drawing();

            try
            {
                // 1 calculate Matrix of aeronent
                OnInformed("Reading CIMEL inversion Matrix");
                if(args==null||args.Length<3)
                    throw new ArgumentException("Missing arguments!\r\ndraw [inputPath] [outputfile] [lat|lon]");
                string mwInput = args[0];
                string mwOutput = args[1];
                string location = args[2];
                string[] arrLocation = location.Split(new char[] {'|'}, StringSplitOptions.None);
                if (arrLocation.Length < 2)
                    throw new ArgumentException("invalid [location]!\r\n[location]= \"[lat|lon]\"");

                // get lat and lon of region
                double lat = ToDouble(arrLocation[0]);//region.Lat;
                double lon = ToDouble(arrLocation[1]);//region.Lon;
                /*
                object[] results = 
                 */
                    drawing.MatrixCIMEL(2, lat, lon, mwInput, mwOutput);
                /*
                 * Disable all drawing function
                var stats_inversion = results[0];
                var r = results[1];

                OnInformed("stats_inversions");
                PrintMatrix((double[,])stats_inversion, OnInformed);
                OnInformed("r");
                PrintMatrix((double[,])r, OnInformed);
                OnInformed("DONE to Calculate CIMEL inversion Matrix");

                // 2 draw SSA
                OnInformed("Drawing SSA figures");
                // MWArray mwYear = new MWNumericArray(new int[] {2013});
                // MWArray mwOuputbase = new MWCharArray(new string[]{ outputbase});
                double mwYear = 2013;
                string mwOuputbase = outputbase;
                string mwRegion = STNS_FN;

                OnInformed("\tARGUMENTS:");
                OnInformed(String.Format("\t{0} : {1}", "YEAR", mwYear));
                OnInformed(String.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawSSA(stats_inversion, r, mwYear, mwRegion, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to draw SSA figures");

                // 3 draw SSA Statistic
                OnInformed("Drawing SSA Statistic figures");
                OnInformed("\tARGUMENTS:");
                OnInformed(String.Format("\t{0} : {1}", "YEAR", mwYear));
                OnInformed(String.Format("\t{0} : {1}", "REGION", mwRegion));
                OnInformed(String.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawSSAStatistisc(stats_inversion, r, mwYear, mwRegion, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to draw SSA Statistic figures");

                // 4 draw CIMEL Inversions
                OnInformed("Drawing CIMEL Inversions figures");
                OnInformed("\tARGUMENTS:");
                OnInformed(String.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawAeronetInversions(stats_inversion, r, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to drawing CIMEL Inversions figures");
                */
            }
            catch (Exception ex)
            {
                OnFailed(ex.Message);
            }
            finally
            {
                drawing.Dispose();
            }
        }

        /// <summary>
        /// put the error message to error stream
        /// </summary>
        /// <param name="error"></param>
        private static void OnFailed(string error)
        {
            Console.Error.WriteLine(error);
        }

        /// <summary>
        /// put the info message to std output stream
        /// </summary>
        /// <param name="info"></param>
        private static void OnInformed(string info)
        {
            Console.Out.WriteLine(info);
        }

        private static double ToDouble(string value)
        {
            double result;
            if (!double.TryParse(value, out result))
                result = 0f;
            return result;
        }

        /*
        public static void PrintMatrix(double[,] arrary, Action<string, bool> log)
        {
            int d1 = arrary.GetLength(0);
            int d2 = arrary.GetLength(1);
            for (int i = 0; i < d1; i++)
            {
                string line = String.Empty;
                for (int j = 0; j < d2; j++)
                {
                    line += arrary[i, j] + " ";
                }
                log.Invoke(line, true);
            }
        }
         */
    }
}