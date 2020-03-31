using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxiaEngineService.OptimaAPI;


namespace ProxiaEngineService
{
    public class OptimaHandler
    {
        private Optima optima;
        private readonly string username;
        private readonly string password;
        private readonly string company;
        private readonly string configurationDB = "CDN_KNF_Konfiguracja_Dotykacka";
        private readonly string attName = "BUYERORDERNUMBER";

        public OptimaHandler(string path, string username, string password, string company)
        {
            optima = new Optima(path, true);
            this.username = username;
            this.password = password;
            this.company = company;

            optima.Login(username, password, company);
        }

        public void Dispose()
        {
            optima.LogOut();
        }

        public void Save()
        {
            optima.Save();
        }

        public void SaveWithoutSessionRefresh()
        {
            optima.SaveWithoutSessionRefresh();
        }

        public OP_CSRSLib.SrsZlecenie GetOrCreateOrder(string zlecenieNr, DateTime dataPrzyjęcia, string numSymbol)
        {
            try
            {
                ADODB.Recordset rs = optima.Execute("select DAt_SrZId from CDN.DokAtrybuty join CDN.DefAtrybuty on DeA_DeAId = DAt_DeAId and DeA_Kod = '" + attName + "' where DAt_WartoscTxt = '" + zlecenieNr + "'");

                if (rs.RecordCount == 1)
                {
                    return optima.GetZlecenieByID(int.Parse(rs.Fields["DAt_SrZId"].Value.ToString()));
                }
                else if (rs.RecordCount == 0)
                {
                    OP_CSRSLib.SrsZlecenie zlecenie = optima.CreateZlecenie();
                    zlecenie.DataPrzyjecia = dataPrzyjęcia;
                    var numerator = zlecenie.Numerator;
                    numerator.DefinicjaDokumentu = optima.GetNumeratorBySymbol(numSymbol);

                    CDNTwrb1.IDefAtrybut defAtrybut = optima.GetDefAtribute(attName, 4);
                    CDNTwrb1.IDokAtrybut atrybut = zlecenie.Atrybuty.AddNew();
                    atrybut.DeAID = defAtrybut.ID;
                    atrybut.Wartosc = zlecenieNr;

                    return zlecenie;
                }
                else
                    throw new Exception("Błąd! Znaleziono " + rs.RecordCount + " zleceń o numerze [" + zlecenieNr + "]");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddPart(string zlecenieNr, DateTime dataPrzyjecia, string towarKod, int ilość, string pracOpeKod)
        {
            try
            {
                OP_CSRSLib.SrsZlecenie zlecenie = GetOrCreateOrder(zlecenieNr, dataPrzyjecia, "SRW");
                CDNTwrb1.Towar towar = optima.GetGoodByCode(towarKod, 1);

                OP_CSRSLib.SrsCzesc czesc = zlecenie.Czesci.AddNew();
                czesc.TwrId = towar.ID;
                czesc.IloscPobieranaJM = ilość;

                SetServiceman(czesc, pracOpeKod);
            }
            catch (Exception e)
            {
                throw new Exception("Błąd przy dodawaniu części o kodzie [" + towarKod + "] dla zlecenia o nr [" + zlecenieNr + "]: " + e.Message);
            }
        }

        public void AddAction(string zlecenieNr, DateTime dataPrzyjecia, string usługaKod, string opis, string pracOpeKod, DateTime dataWykonania, DateTime czasTrwania) // data, nr zlenewnia, opis cznn, kod prc, czas trw
        {
            try
            {
                OP_CSRSLib.SrsZlecenie zlecenie = GetOrCreateOrder(zlecenieNr, dataPrzyjecia, "SRW");
                CDNTwrb1.Towar towar = optima.GetGoodByCode(usługaKod, 0);

                OP_CSRSLib.SrsCzynnosc czynnosc = zlecenie.Czynnosci.AddNew();
                czynnosc.TwrId = towar.ID;
                czynnosc.Opis = opis;

                SetServiceman(czynnosc, pracOpeKod);

                czynnosc.DataWykonania = dataWykonania;
                czynnosc.CzasTrwania = czasTrwania;
            }
            catch (Exception e)
            {
                throw new Exception("Błąd przy dodawaniu czynnosci o kodzie [" + usługaKod + "] dla zlecenia o nr [" + zlecenieNr + "]: " + e.Message);
            }
        }

        private void SetServiceman(dynamic obj, string pracOpeKod)
        {
            ADODB.Recordset rs = optima.Execute("select PRI_PraId from CDN.Pracidx where PRI_Kod = '" + pracOpeKod + "' and PRI_Typ = 1");
            if (rs.RecordCount == 1)
            {
                obj.SerwisantTyp = 3;
                obj.SerwisantId = int.Parse(rs.Fields["PRI_PraId"].Value.ToString());
            }
            else if (rs.RecordCount == 0)
                throw new Exception("Nie znaleziono pracownika o kodzie [" + pracOpeKod + "]");
            else
                throw new Exception("Błąd! Znaleziono " + rs.RecordCount + " pracowników o kodzie [" + pracOpeKod + "]");
        }
    }
}
