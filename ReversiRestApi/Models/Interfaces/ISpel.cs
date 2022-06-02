using System;

namespace ReversiRestApi.Models.Interfaces
{
    public enum Kleur { Geen, Wit, Zwart };
    public enum Status { Wachtende, Bezig, Afgelopen, Verwijderd };

    public interface ISpel
    {
        Guid ID { get; set; }
        string Omschrijving { get; set; }

        //het unieke token van het spel
        string Token { get; set; }
        string Speler1Token { get; set; }
        string Speler2Token { get; set; }

        Kleur[,] Bord { get; set; }
        Kleur AandeBeurt { get; set; }
        public Status Status { get; set; }
        void Pas();
        bool Afgelopen();

        //welke kleur het meest voorkomend op het speelbord
        Kleur OverwegendeKleur();

        //controle of op een bepaalde positie een zet mogelijk is
        bool ZetMogelijk(int rijZet, int kolomZet);
        void DoeZet(int rijZet, int kolomZet);
    }
}
