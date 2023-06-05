﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ChainEdge.Drivers;
using ChainFx;
using ChainFx.Web;
using Application = System.Windows.Application;

namespace ChainEdge
{
    /// <summary>
    /// The encapsulation of the application.
    /// </summary>
    public class MainApp : Application
    {
        const string APP_JSON = "app.json";


        // the global logger
        static FileLogger logger;

        // the singleton application instance
        static MainApp app;

        static JObj cfg;

        // the present profile 
        static Job _job;


        // single threaded
        private static readonly EdgeQueue edgeq = new();

        public static EdgeQueue EdgeQueue => edgeq;


        private static Map<int, Driver> drivers = new(32)
        {
#if LABEL
            { 1, new LabelPrintDriver() }
#elif SCALE
            { 1, new ScaleDriver() }
#endif
        };


        static MainApp()
        {
            // app instance
            try
            {
                app = new MainApp()
                {
                    MainWindow = new MainWindow()
                    {
                        Title = "ChainEdge",
                        WindowStyle = WindowStyle.SingleBorderWindow,
                        WindowState = WindowState.Maximized
                    },
                    ShutdownMode = ShutdownMode.OnMainWindowClose,
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        void RegisterEventType(int typ, string d)
        {
        }

        [STAThread]
        public static void Main(string[] args)
        {
            var drv = new ObjectDetectorDriver();
            drv.Test();
            
            // load app config
            var bytes = File.ReadAllBytes("drivers.json");
            var parser = new JsonParser(bytes, bytes.Length);
            cfg = (JObj)parser.Parse();

            WebApp.StartAsync();

            // win.Show();
            app.Run(app.MainWindow);

            WebApp.StopAsync().RunSynchronously();
        }
    }
}