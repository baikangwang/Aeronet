﻿using Peach.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aeronet.Chart.AeronetData
{
    public class DataWorker
    {
        #region Events

        public event MessageHandler Informed;

        public event MessageHandler Failed;

        public event MessageHandler Started;

        /// <summary>
        /// The completed event message which will be triggered as either finishing successfully or faital error occurs
        /// </summary>
        public event MessageHandler Completed;

        protected virtual void OnInformed(string message, bool external = true)
        {
            var handler = Informed;
            if (handler != null) handler(this, new EventMessage(message, external));
        }

        protected virtual void OnFailed(string message, bool external = true)
        {
            var handler = Failed;
            if (handler != null) handler(this, new EventMessage(message, external));
        }

        protected virtual void OnStarted(EventMessage message)
        {
            var handler = Started;
            if (handler != null) handler(this, message);
        }

        protected virtual void OnCompleted(EventMessage message)
        {
            var handler = Completed;
            if (handler != null) handler(this, message);
        }

        #endregion Events

        private ProcessStartInfo NewStartInfo(string command, string args)
        {
            string program = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, command);
            ProcessStartInfo startInfo = new ProcessStartInfo(program, args)
            {
                RedirectStandardOutput = true, // display output in current screen
                RedirectStandardError = true, // display error in current screen
                CreateNoWindow = true, // don't launch another command line windows
                UseShellExecute = false, // perform in current command line windows
                ErrorDialog = false // the error will be displayed in current windows
            };

            return startInfo;
        }

        private bool Run(ProcessStartInfo startInfo)
        {
            bool success = true;
            // initial creator process
            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (s, e) => { this.OnInformed(e.Data, false); };
                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        success = false;
                        OnFailed(e.Data, false);
                    }
                };

                // Run creator
                process.Start();

                // read error
                process.BeginErrorReadLine();
                // read output
                process.BeginOutputReadLine();

                // waiting until exit
                process.WaitForExit();
            }

            return success;
        }

        private void InitWorkingFolder(string[] folders)
        {
            if (folders == null || folders.Length == 0) return;

            foreach (string folder in folders)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        private void InitRegionFolder(string region)
        {
            string[] workingfolders = new string[] { "output", ConfigOptions.FOUT };
            foreach (string wf in workingfolders)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wf, region);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        private void Cleanup(string region)
        {
            string input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigOptions.FOUT, region);
            string[] files = Directory.GetFiles(input, "*.*", SearchOption.TopDirectoryOnly);
            // delete all resource excepts 130119
            string[] reserveds = new string[] { "hangzhou_808_130119" };

            foreach (string f in files)
            {
                bool reserved = reserveds.Any(reservedFile => Regex.IsMatch(f, reservedFile, RegexOptions.IgnoreCase));
                if (!reserved)
                    File.Delete(f);
                else
                {
                    this.OnInformed(string.Format("{0} - normal", f));
                }
            }

            // revert FNAME
            string fname = Path.Combine(input, "FNAME");
            string content = @" 130119";
            File.WriteAllText(fname, content);
            this.OnInformed("Rewrite FNAME");

            // revert FNAME.txt
            content =
@"    hangzhou_808_130119_011148
    hangzhou_808_130119_021201
    hangzhou_808_130119_031202";
            string fnametxt = Path.Combine(input, "FNAME.txt");
            File.WriteAllText(fnametxt, content);
            this.OnInformed("Rewrite FNAME.txt");
        }

        public void Exit()
        {
            this.OnCompleted(new EventMessage("Completed", false));
        }

        /// <summary>
        /// Stop the data process
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException("Hasn't been implemented");
        }

        /// <summary>
        /// Start the data process
        /// </summary>
        public void Start()
        {
            this.OnStarted(new EventMessage("Started", false));

            // todo: acquire the version from runtime
            OnInformed("Aeronet Inversion VER: 1.0.0.1");

            // initial
            OnInformed("Initial arguments");

            string STNS_FN = Utility.GetAppSettingValue("ARG_STNS_FN", @default: "hangzhou");
            OnInformed(string.Format("STNS_FN : {0}", STNS_FN));

            string STNS_ID = Utility.GetAppSettingValue("ARG_STNS_ID", @default: "808");
            OnInformed(string.Format("STNS_FN : {0}", STNS_ID));

            string FDATA = Utility.GetAppSettingValue("ARG_FDATA", @default: "hangzhou-808-1");
            OnInformed(string.Format("STNS_FN : {0}", FDATA));

            string ipt = ConfigOptions.FIPT;
            string @out = ConfigOptions.FOUT;
            string brdf = ConfigOptions.FBRDF;
            string dat = ConfigOptions.FDAT;

            OnInformed("Initial working folders");
            InitWorkingFolder(new string[] { ipt, @out, brdf, dat });

            OnInformed("Initial input & output folders");
            InitRegionFolder(STNS_FN);

#if !DEBUGMATLAB
            // initial creator command arguments
            string commandArgs = string.Format("{0} {1} {2} {3} {4} {5} {6}", STNS_FN, STNS_ID, FDATA, ipt, @out, brdf, dat);
            ProcessStartInfo creatorProInfo = NewStartInfo(ConfigOptions.PROGRAM_CREATOR, commandArgs);
            // show command line and args
            OnInformed(string.Format("{0} {1}", ConfigOptions.PROGRAM_CREATOR, commandArgs));
            OnInformed(string.Format("{0} = {1}", "STNS_FN", STNS_FN));
            OnInformed(string.Format("{0} = {1}", "STNS_ID", STNS_ID));
            OnInformed(string.Format("{0} = {1}", "FDATA", FDATA));
            OnInformed(string.Format("{0} = {1}", "FIPT", ipt));
            OnInformed(string.Format("{0} = {1}", "FOUT", @out));
            OnInformed(string.Format("{0} = {1}", "FBRDF", brdf));
            OnInformed(string.Format("{0} = {1}", "FDAT", dat));
            // perform creator process
            OnInformed("***************************************************************");
            bool sucess = Run(creatorProInfo);
            OnInformed("***************************************************************");
            if (!sucess)
            {
                Exit();
                return;
            }

#if DEMON
            // only keep a few of testing files for next step
            // only keep 130119 files, FNAME.txt and FNAME
            LogInfo("For demo presentation, only keeps the 130119 testing files");
            Cleanup(STNS_FN);
#endif
            // initial outputor command arguments
            ProcessStartInfo outputorProInfo = NewStartInfo(ConfigOptions.PROGRAM_OUTPUTOR, string.Format("{0}", STNS_FN));
            // show command line and args
            OnInformed(string.Format("{0} {1}", ConfigOptions.PROGRAM_OUTPUTOR, STNS_FN));
            OnInformed(string.Format("{0} = {1}", "STNSSTR", STNS_FN));
            // perform outputor process
            OnInformed("***************************************************************");
            sucess = Run(outputorProInfo);
            OnInformed("***************************************************************");
            if (!sucess)
            {
                Exit();
                return;
            }
#endif
            bool success = true;
            // draw aeronent inversion
            AeronetDrawNative.Drawing drawing = new AeronetDrawNative.Drawing();
            try
            {
                OnInformed("***************************************************************");
                string inputbase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output", STNS_FN) + System.IO.Path.DirectorySeparatorChar;
                string outputbase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "cimel_network", STNS_FN,
                        "dubovik") + System.IO.Path.DirectorySeparatorChar;
                string outputfile = Path.Combine(outputbase,
                    string.Format("Dubovik_stats_{0}_{1}_{2:yyyyMMdd}.dat", STNS_FN, STNS_ID, DateTime.Now));

                if (!Directory.Exists(outputbase))
                    Directory.CreateDirectory(outputbase);

                // 1 calculate Matrix of aeronent
                OnInformed("Calculating Aeronet inversion Matrix");
                string mwInput = inputbase;
                string mwOutput = outputfile;
                // get lat and lon of region
                Region region = RegionStore.Singleton.FindRegion(STNS_FN);

                double lat = region.Lat;
                double lon = region.Lon;
                OnInformed("\tARGUMENTS:");
                OnInformed(string.Format("\t{0} : {1}", "INPUT", mwInput));
                OnInformed(string.Format("\t{0} : {1}", "OUTPUT", mwOutput));
                object[] results = drawing.MatrixAeronet(2, lat, lon, mwInput, mwOutput);
                var stats_inversion = results[0];
                var r = results[1];

                OnInformed("stats_inversions");
                Utility.PrintMatrix((double[,])stats_inversion, OnInformed);
                OnInformed("r");
                Utility.PrintMatrix((double[,])r, OnInformed);
                OnInformed("DONE to Calculate Aeronet inversion Matrix");

                // 2 draw SSA
                OnInformed("Drawing SSA figures");
                // MWArray mwYear = new MWNumericArray(new int[] {2013});
                // MWArray mwOuputbase = new MWCharArray(new string[]{ outputbase});
                double mwYear = 2013;
                string mwOuputbase = outputbase;
                string mwRegion = STNS_FN;

                OnInformed("\tARGUMENTS:");
                OnInformed(string.Format("\t{0} : {1}", "YEAR", mwYear));
                OnInformed(string.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawSSA(stats_inversion, r, mwYear, mwRegion, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to draw SSA figures");

                // 3 draw SSA Statistic
                OnInformed("Drawing SSA Statistic figures");
                // MWArray mwRegion = new MWCharArray(new string[]{ STNS_FN});
                OnInformed("\tARGUMENTS:");
                OnInformed(string.Format("\t{0} : {1}", "YEAR", mwYear));
                OnInformed(string.Format("\t{0} : {1}", "REGION", mwRegion));
                OnInformed(string.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawSSAStatistisc(stats_inversion, r, mwYear, mwRegion, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to draw SSA Statistic figures");

                // 4 draw Aeronet Inversions
                OnInformed("Drawing Aeronet Inversions figures");
                OnInformed("\tARGUMENTS:");
                OnInformed(string.Format("\t{0} : {1}", "OUTPUT", mwOuputbase));
                drawing.DrawAeronetInversions(stats_inversion, r, mwOuputbase);
                drawing.WaitForFiguresToDie();
                OnInformed("DONE to drawing Aeronet Inversions figures");
            }
            catch (Exception ex)
            {
                OnFailed(ex.Message);
                success = false;
            }
            finally
            {
                OnInformed("***************************************************************");
                drawing.Dispose();
            }
            if (!success)
            {
                Exit();
                return;
            }

            OnInformed("All jobs are complete!");
            Exit();
        }
    }
}