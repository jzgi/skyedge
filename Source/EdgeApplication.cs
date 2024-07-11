﻿using System;
using System.Windows;
using ChainFX;
using ChainFX.Web;
using Microsoft.Extensions.Logging;

#pragma warning disable CS4014

// ReSharper disable AccessToStaticMemberViaDerivedType

namespace ChainEdge;

/// <summary>
/// The main WPF application that hosts all relevant resources.
/// </summary>
public class EdgeApplication : System.Windows.Application
{
    // use the embedded logger
    internal static FileLogger Logger => EmbedProxy.Logger;

    // use the embedded configure
    internal static JObj Config => EmbedProxy.Config;

    // use the embedded configure
    public static string Name => EmbedProxy.Nodal.name;

    public static string Tip => EmbedProxy.Nodal.tip;

    internal static readonly EdgeWindow Win;

    // connector to the cloud
    internal static readonly EdgeConnector Connector;

    internal static readonly Profile Profile;


    // // the main application instance
    // static readonly EdgeApp App = new()
    // {
    //     MainWindow = Win,
    //     ShutdownMode = ShutdownMode.OnMainWindowClose,
    // };

    public static readonly EdgeWrap Wrap = new();


    // the main application instance
    static readonly EdgeApplication App;


    static EdgeApplication()
    {
        Win = new()
        {
            WindowStyle = WindowStyle.SingleBorderWindow,
            WindowState = WindowState.Maximized,
        };


        App = new()
        {
            MainWindow = Win,
            ShutdownMode = ShutdownMode.OnMainWindowClose,
        };

        // create client
        //
        string url = Config[nameof(url)];
        if (url == null)
        {
            Logger.LogError("missing 'url' in application.json");
            return;
        }
        Connector = new EdgeConnector(url)
        {
        };

        // resolve current profile
        //
        string profile = Config[nameof(profile)];
        Profile = Profile.GetProfile(profile);
        if (Profile == null)
        {
            Logger.LogError("unsupported profile in application.json");
            return;
        }
    }

    [STAThread]
    public static void Main(string[] args)
    {
        // start the embedded web server
        //
        if (Profile is IProxiable)
        {
            EmbedProxy.Initialize();

            EmbedProxy.StartAsync(waiton: false);
        }


        // var jo = new JObj
        // {
        //     { "1", "万载百合广场" },
        //     { "created", "2024-04-02" },
        //     { "orgname", "中惠体验中心" },
        //     { "uname", "刘青云" },
        //     { "uaddr", "北京西城区玉渊潭102号" },
        //     {
        //         "@", new JArr()
        //         {
        //             new JObj()
        //             {
        //                 { "1", "无铅酱油" },
        //                 { "2", "1瓶" },
        //                 { "3", "￥23.0" },
        //             },
        //             new JObj()
        //             {
        //                 { "1", "百合粉" },
        //                 { "2", "1包" },
        //                 { "3", "￥80.0" },
        //             },
        //         }
        //     }
        // };
        //
        // var drv = Profile.GetDriver<ESCPOSSerialPrintDriver>(null);
        // drv.Add<NewOrderPrintJob>(jo);
        //


        // initial test for each & every driver
        Profile.Start();

        TaskbarIconUtility.Do();

        // start the app and open the main windows
        App.Run(Win);

        //
        // EmbedApp.StopAsync().RunSynchronously();
    }
}