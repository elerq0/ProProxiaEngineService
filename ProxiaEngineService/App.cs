using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProxiaEngineService.Models;
using ProxiaEngineService.Models.FileTypeModels;
using ProxiaEngineService.Properties;

namespace ProxiaEngineService
{
    public static class App
    {
        private static string UsageInfo => $"Usage:\n{AppDomain.CurrentDomain.FriendlyName} [flag] [parameter?]";
        private static readonly string connectionString = Settings.Default.ConnectionString;
        private static readonly Logger logger = new Logger(Settings.Default.LogPath, Settings.Default.LoggingMode == "Normal" ? LoggingMode.Normal : LoggingMode.Enhanced);
        private static readonly FilenameFormat format = Settings.Default.FilenameTSFormat == "Short" ? FilenameFormat.Short : FilenameFormat.Long;

        public static void Run(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                logger.Log("Null");
                PrintUsage();
                return;
            }
            var flag = args[0].Substring(1);
            logger.Log("Mode = [" + flag + "]", LoggingMode.Enhanced);

            switch (flag)
            {
                case "r":
                    if (args.Length != 2)
                    {
                        PrintUsage();
                        return;
                    }
                    var orderNumber = args[1];
                    Read(orderNumber);
                    break;
                case "w":
                    if (args.Length != 1)
                    {
                        PrintUsage();
                        return;
                    }
                    Write();
                    break;
                case "c":
                    Czesc();
                    break;
                case "cr":
                    CzescReset();
                    break;
                case "t":
                    Test();
                    break;
                default:
                    Console.WriteLine(UsageInfo);
                    break;
            }
        }

        public static void Test()
        {
            try
            {
                OptimaHandler optima = new OptimaHandler(Settings.Default.OptimaPath,
                                                         Settings.Default.OptimaUsername,
                                                         Settings.Default.OptimaPassword,
                                                         Settings.Default.OptimaCompany);


                optima.AddPart("testowy", DateTime.Now, "DOTHOLD08", 13, "GRĄDZIEL");
                optima.Save();
                optima.AddAction("testowy", DateTime.Now, "DOTLLATWO", "Opis testowy", "GRĄDZIEL", DateTime.Now, DateTime.MinValue.AddMinutes(45));
                optima.Save();

                optima.Dispose();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void CzescReset()
        {
            logger.Log("Starting [CzescReset]", LoggingMode.Enhanced);
            const string commandString = "delete from CDN.PROProxiaMatstamm";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                logger.Log("Opened Connection to DB", LoggingMode.Enhanced);
                using (var command = new SqlCommand(commandString, connection))
                {
                    command.ExecuteReader();
                    logger.Log("Exequted SQL Server Procedure", LoggingMode.Enhanced);
                }
            }
            logger.Log("Closed all connections, exit", LoggingMode.Enhanced);

        }
        private static void Czesc()
        {
            logger.Log("Starting [Czesc]", LoggingMode.Enhanced);
            const string commandString = "exec CDN.PROExportProxiaMatstamm";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                logger.Log("Opened Connection to DB", LoggingMode.Enhanced);
                using (var command = new SqlCommand(commandString, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        logger.Log("Exequted SQL Server Procedure", LoggingMode.Enhanced);
                        var materials = new List<DocumentBase>();
                        while (reader.Read())
                        {
                            var material = new Material();
                            material.Load(reader);
                            materials.Add(material);
                        }
                        logger.Log("Loaded Material from reader", LoggingMode.Enhanced);

                        var proxia = new ProxiaMessageHandler(Settings.Default.InterfacePath, format, false, logger, ProxiaMessageError.SkipOnError);
                        logger.Log("Created Proxia instance", LoggingMode.Enhanced);
                        if (materials.Count != 0)
                            proxia.WriteMessageList(logger, materials);
                        logger.Log("Saved Material to file", LoggingMode.Enhanced);

                    }
                }
            }
            logger.Log("Closed all connections, exit", LoggingMode.Enhanced);
        }


        private static void Write()
        {
            var proxia = new ProxiaMessageHandler(Settings.Default.InterfacePath, format, false, logger, ProxiaMessageError.SkipOnError);
            logger.Log("Created ProxiaMessageHandler instance", LoggingMode.Enhanced);

            if (!proxia.IsMessage()) return;


            var zp = GetProcessZP();
            logger.Log("Loaded ZP instance", LoggingMode.Enhanced);
            var doc = proxia.GetMessage();
            logger.Log("Loaded first message from ProxiaMessageHandler", LoggingMode.Enhanced);

            zp.Run(doc);
            logger.Log("Sent data to ZP with \"Run\" method", LoggingMode.Enhanced);
            proxia.NextMessage();
            logger.Log("Marked message as parsed", LoggingMode.Enhanced);
        }

        private static void Read(string orderNumber)
        {
            const string commandString = "exec CDN.PROPROXIAGetOrderinfo @NumerPelny";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                logger.Log("Opened Connection to DB", LoggingMode.Enhanced);
                using (var command = new SqlCommand(commandString, connection))
                {
                    command.Parameters.AddWithValue("@NumerPelny", orderNumber);
                    using (var reader = command.ExecuteReader())
                    {
                        logger.Log("Exequted SQL Server Procedure", LoggingMode.Enhanced);
                        var order = new Order();
                        order.Load(reader);
                        logger.Log("Loaded Order from reader", LoggingMode.Enhanced);

                        reader.NextResult();

                        var workCard = new WorkCard();
                        workCard.Load(reader);
                        logger.Log("Loaded Work Card from reader", LoggingMode.Enhanced);

                        var proxia = new ProxiaMessageHandler(Settings.Default.InterfacePath, format, false, logger, ProxiaMessageError.SkipOnError);
                        logger.Log("Created Proxia instance", LoggingMode.Enhanced);
                        proxia.WriteMessage(order);
                        logger.Log("Saved Order to file", LoggingMode.Enhanced);
                        proxia.WriteMessage(workCard);
                        logger.Log("Saved Work Card to file", LoggingMode.Enhanced);
                    }
                }
            }
            logger.Log("Closed all connections, exit", LoggingMode.Enhanced);
        }

        private static void PrintUsage() => Console.WriteLine(UsageInfo);

        private static IProcessZP GetProcessZP()
        {
            throw new NotImplementedException();
        }

        private interface IProcessZP
        {
            void Run(DocumentBase document);
        }
    }
}
