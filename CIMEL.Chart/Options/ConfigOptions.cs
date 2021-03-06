// #define AUTO_INIT
using CIMEL.Chart.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peach.Log;

namespace CIMEL.Chart
{
    public partial class ConfigOptions
    {
        private static readonly Encoding EncodingCode = Encoding.GetEncoding("GB2312");

        private static ConfigOptions _default = new ConfigOptions();

        protected string OptionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options", "options.json");

        protected ConfigOptions()
        {
            // check if existing
            if (File.Exists(this.OptionsPath))
            {
                this.Load(this.OptionsPath);
            }
#if AUTO_INIT
            else
            {
                // initial it
                this.Initial(this.OptionsPath);
            }

            // initial all of the working folders
            this.InitialEnv();
#endif
        }
#if AUTO_INIT
        private void InitialEnv()
        {
            string[] pathes=new string[]
            {
                this.DATA_Dir,this.CHARTSET_Dir,this.INS_PARA_Dir,this.METADATA_Dir,this.MODIS_BRDF_Dir,this.OUTPUT_Dir
            };

            // initial path
            Parallel.ForEach(pathes, p =>
            {
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            });
        }
#endif
        [Browsable(false)]
        public static ConfigOptions Singleton
        {
            get
            {
                return _default;
            }
        }

        [Browsable(false)]
        public bool IsInitialized {
            get
            {
                return Utility.IsInit(this.DATA_Dir)
                                      && Utility.IsInit(this.MODIS_BRDF_Dir)
                                      && Utility.IsInit(this.INS_PARA_Dir)
                                      && Utility.IsInit(this.METADATA_Dir)
                                      && Utility.IsInit(this.OUTPUT_Dir)
                                      && Utility.IsInit(this.CHARTSET_Dir);
            }
        }

        [Category(CATELOG_INPUT),
        DisplayName(METADATA_NAME),
        Description(METADATA_DESC),
        PropertyOrder(0),
        Editor(typeof(FolderBrowserEditor), typeof(UITypeEditor))]
        public string METADATA_Dir { get; set; }

        [Category(CATELOG_INPUT),
        DisplayName(DATA_NAME),
        Description(DATA_DESC),
        PropertyOrder(1),
        Editor(typeof(FolderBrowserEditor), typeof(UITypeEditor))]
        public string DATA_Dir { get; set; }

        [Category(CATELOG_INPUT),
        DisplayName(MODIS_BRDF_NAME),
        Description(MODIS_BRDF_DESC),
        PropertyOrder(3),
        ReadOnly(true)]
        public string MODIS_BRDF_Dir { get; set; }

        [Category(CATELOG_INPUT),
        DisplayName(INS_PARA_NAME),
        Description(INS_PARA_DESC),
        PropertyOrder(2),
        ReadOnly(true)]
        public string INS_PARA_Dir { get; set; }

        [Category(CATELOG_OUTPUT),
        DisplayName(OUTPUT_NAME),
        Description(OUTPUT_DESC),
        PropertyOrder(4),
        ReadOnly(true)]
        public string OUTPUT_Dir { get; set; }

        [Category(CATELOG_OUTPUT),
        DisplayName(CHARTSET_NAME),
        Description(CHARTSET_DESC),
        PropertyOrder(5),
        ReadOnly(true)]
        public string CHARTSET_Dir { get; set; }

        [Category(CATELOG_PROGRAM),
        PropertyOrder(7),
        DisplayName(@"格式化程序"),
        Description("格式化和过滤ce318数据，为CIMEL反演算法做数据准备"),
        ReadOnly(true)]
        public string PROGRAM_OUTPUTOR { get; set; }

        [Category(CATELOG_PROGRAM),
        PropertyOrder(6),
        DisplayName(@"主生成程序"),
        Description("执行CIMEL反演算法"),
        ReadOnly(true)]
        public string PROGRAM_CREATOR { get; set; }

        [Category(CATELOG_PROGRAM),
        PropertyOrder(8),
        DisplayName(@"画图程序"),
        ReadOnly(true),
        Description("读取CIMEL反演产品数据生成矩阵文件")]
        public string PROGRAM_DRAWER { get; set; }

        [Category(CATELOG_PROGRAM),
        PropertyOrder(9),
        DisplayName(@"图像集程序"),
        ReadOnly(true),
        Description("生成图像集数据")]
        public string PROGRAM_SPLITTER { get; set; }

        /// <summary>
        /// Loads the options to memory
        /// </summary>
        /// <param name="optionFile"></param>
        private void Load(string optionFile)
        {
            string content = File.ReadAllText(optionFile,EncodingCode);
#if AUTO_INIT
            // initial if it's empty file
            if (string.IsNullOrEmpty(content))
            {
                this.Initial(optionFile);
            }
            else
            {
#endif
                try
                {
                    var options = (dynamic)JObject.Parse(content);
                    this.DATA_Dir = (string)options.input.data;
                    this.MODIS_BRDF_Dir = (string)options.input.modis_brdf;
                    this.INS_PARA_Dir = (string)options.input.ins_para;
                    this.METADATA_Dir = (string)options.input.metadata;
                    this.OUTPUT_Dir = (string)options.output.output;
                    this.CHARTSET_Dir = (string) options.output.chartset;
                    this.PROGRAM_CREATOR = (string)options.processor.creator;

                    // initial creator
                    if (string.IsNullOrEmpty(this.PROGRAM_CREATOR))
                        this.PROGRAM_CREATOR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData",
                            "create_input.exe");
                    // initial outputor
                    this.PROGRAM_OUTPUTOR = (string)options.processor.outputor;
                    if(string.IsNullOrEmpty(this.PROGRAM_OUTPUTOR))
                        this.PROGRAM_OUTPUTOR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData",
                            "main.exe");
                    // initial drawer
                    this.PROGRAM_DRAWER = (string)options.processor.drawer;
                    if (string.IsNullOrEmpty(this.PROGRAM_DRAWER))
                        this.PROGRAM_DRAWER = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData",
                            "draw.exe");
                    // initial splitter
                    this.PROGRAM_SPLITTER = (string) options.processor.splitter;
                    if (string.IsNullOrEmpty(this.PROGRAM_SPLITTER))
                        this.PROGRAM_SPLITTER = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData",
                            "splitter.exe");
                }
                catch (Exception ex)
                {
                    Logger.Default.Error(ex.Message);
#if AUTO_INIT
                    // rebuild the config options if it's broken
                    this.Initial(optionFile);
#endif
                }
#if AUTO_INIT
                if (!this.IsInitialized)
                {
                    this.Initial(optionFile);
                }
            }
#endif
        }

        /// <summary>
        /// Initial an empty option file
        /// </summary>
        /// <param name="optionFile"></param>
        private void Initial(string optionFile)
        {
#if AUTO_INIT
            dynamic options = new
            {
                input = new
                {
                    data =
                        string.IsNullOrEmpty(this.DATA_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data")
                            : this.DATA_Dir,
                    modis_brdf =
                        string.IsNullOrEmpty(this.MODIS_BRDF_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modis_brdf")
                            : this.MODIS_BRDF_Dir,
                    ins_para =
                        string.IsNullOrEmpty(this.INS_PARA_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ins_para")
                            : this.INS_PARA_Dir,
                    metadata =
                        string.IsNullOrEmpty(this.METADATA_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadata")
                            : METADATA_Dir,
                },
                processor = new
                {
                    creator =
                        string.IsNullOrEmpty(this.PROGRAM_CREATOR)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData",
                                "create_input.exe")
                            : this.PROGRAM_CREATOR,
                    outputor =
                        string.IsNullOrEmpty(this.PROGRAM_OUTPUTOR)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData", "main.exe")
                            : this.PROGRAM_OUTPUTOR,
                    drawer =
                        string.IsNullOrEmpty(this.PROGRAM_DRAWER)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData", "draw.exe")
                            : this.PROGRAM_DRAWER,
                    splitter =
                        string.IsNullOrEmpty(this.PROGRAM_SPLITTER)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIMELData", "splitter.exe")
                            : this.PROGRAM_SPLITTER,
                },
                output = new
                {
                    output =
                        string.IsNullOrEmpty(this.OUTPUT_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output")
                            : OUTPUT_Dir,
                    chartset =
                        string.IsNullOrEmpty(this.CHARTSET_Dir)
                            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chartset")
                            : CHARTSET_Dir
                }
            };

            using (StreamWriter sw = new StreamWriter(optionFile, false))
            {
                JsonSerializer.Create().Serialize(new JsonTextWriter(sw), options);
            }

            this.DATA_Dir = (string)options.input.data;
            this.MODIS_BRDF_Dir = (string)options.input.modis_brdf;
            this.INS_PARA_Dir = (string)options.input.ins_para;
            this.METADATA_Dir = (string)options.input.metadata;
            this.OUTPUT_Dir = (string)options.output.output;
            this.CHARTSET_Dir = (string) options.output.chartset;
            this.PROGRAM_CREATOR = (string) options.processor.creator;
            this.PROGRAM_OUTPUTOR = (string) options.processor.outputor;
            this.PROGRAM_DRAWER = (string) options.processor.drawer;
            this.PROGRAM_SPLITTER = (string) options.processor.splitter;
#endif
        }

        /// <summary>
        /// Save the options from the user entered
        /// </summary>
        public void Save()
        {
            string optionFile = this.OptionsPath;
            dynamic option = new
            {
                input = new
                {
                    data = this.DATA_Dir,
                    modis_brdf = this.MODIS_BRDF_Dir,
                    ins_para = this.INS_PARA_Dir,
                    metadata = this.METADATA_Dir,
                },
                processor = new
                {
                    outputor = this.PROGRAM_OUTPUTOR,
                    creator = this.PROGRAM_CREATOR,
                    drawer=this.PROGRAM_DRAWER,
                    splitter=this.PROGRAM_SPLITTER
                },
                output = new
                {
                    output = this.OUTPUT_Dir,
                    chartset = this.CHARTSET_Dir
                }
            };

            using (StreamWriter sw = new StreamWriter(optionFile, false,EncodingCode))
            {
                JsonSerializer.Create().Serialize(new JsonTextWriter(sw), option);
            }
        }

        /// <summary>
        /// Retrieves all working folders within an array of FolderDescription
        /// </summary>
        /// <returns></returns>
        public FolderDescription[] GetFolders()
        {
            return new FolderDescription[]{
                new FolderDescription(DATA_NAME,this.DATA_Dir,DATA_DESC),
                new FolderDescription(MODIS_BRDF_NAME,this.MODIS_BRDF_Dir,MODIS_BRDF_DESC),
                new FolderDescription(INS_PARA_NAME,this.INS_PARA_Dir,INS_PARA_DESC),
                new FolderDescription(METADATA_NAME,this.METADATA_Dir,METADATA_DESC),
                new FolderDescription(OUTPUT_NAME,this.OUTPUT_Dir,OUTPUT_DESC),
                new FolderDescription(CHARTSET_NAME,this.CHARTSET_Dir,CHARTSET_DESC)
            };
        }

        public string ValidateDirs()
        {
            StringBuilder valErrors = new StringBuilder();
            foreach (var folderDesc in Singleton.GetFolders())
            {
                string name = folderDesc.Name;
                string dir = folderDesc.Path;
                // just checks the Data and Meta data folders
                if (Utility.IsEmpty(dir))
                    valErrors.AppendLine(string.Format(@"抱歉, 请设置[{0}]", folderDesc.Name));
                else
                {
                    if (!Utility.IsExist(dir))
                    {
                        if (name == METADATA_Dir)
                            valErrors.AppendLine(string.Format(@"抱歉, 目录不存在, 请重新设置[{0}]", folderDesc.Name));
                        else
                        {
                            if (name == DATA_NAME)
                            {
                                bool autoInit = false;
                                string root = Singleton.METADATA_Dir;
                                if (!Utility.IsEmpty(root))
                                {
                                    string preDefined = Path.Combine(root, "data");
                                    autoInit =
                                        string.Compare(preDefined, dir, StringComparison.CurrentCultureIgnoreCase) == 0;
                                }
                                if (autoInit)
                                    Directory.CreateDirectory(dir);
                                else
                                    valErrors.AppendLine(string.Format(@"抱歉, 目录不存在, 请重新设置[{0}]", folderDesc.Name));
                            }
                            else
                                // for other folders, initial them if not existing
                                Directory.CreateDirectory(dir);
                        }
                    }
                }
            }
            return valErrors.ToString();
        }

        /// <summary>
        /// Refreshes the config options
        /// </summary>
        public void Refresh()
        {
            // check if existing
            if (File.Exists(this.OptionsPath))
            {
                this.Load(this.OptionsPath);
            }
            else
            {
                this.CHARTSET_Dir = string.Empty;
                this.INS_PARA_Dir = string.Empty;
                this.MODIS_BRDF_Dir = string.Empty;
                this.OUTPUT_Dir = string.Empty;
                this.METADATA_Dir = string.Empty;
                this.DATA_Dir = string.Empty;
            }
        }
    }

    public class FolderDescription
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }

        public FolderDescription(string name, string path, string description)
        {
            this.Name = name;
            this.Path = path;
            this.Description = description;
        }
    }
}