using ReversiRestApi.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ReversiRestApi.Models
{
    public class Spel : ISpel
    {
        private const int bordOmvang = 8;
        private Kleur[,] bord;

        public Guid ID { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public string Omschrijving { get; set; }
        public Kleur AandeBeurt { get; set; }
        public Status Status { get; set; }
        public List<(int id, int wit, int zwart)> Beurten { get; set; } = new List<(int id, int wit, int zwart)>() { (0, 2, 2) };

        public Kleur[,] Bord
        {
            get { return bord; }
            set { bord = value; }
        }

        private readonly int[,] richting = new int[8, 2] {
                                {  0,  1 },         // naar rechts
                                {  0, -1 },         // naar links
                                {  1,  0 },         // naar onder
                                { -1,  0 },         // naar boven
                                {  1,  1 },         // naar rechtsonder
                                {  1, -1 },         // naar linksonder
                                { -1,  1 },         // naar rechtsboven
                                { -1, -1 } };       // naar linksboven
        public Spel()
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("/", "q");    // slash mijden ivm het opvragen van een spel via een api obv het token
            Token = Token.Replace("+", "r");    // plus mijden ivm het opvragen van een spel via een api obv het token

            Bord = new Kleur[bordOmvang, bordOmvang];
            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;
            AandeBeurt = Kleur.Geen;
        }

        public void Pas()
        {
            // controleeer of er geen zet mogelijk is voor de speler die wil passen, alvorens van beurt te wisselen.
            if (IsErEenZetMogelijk(AandeBeurt))
                throw new Exception("Passen mag niet, er is nog een zet mogelijk");
            else
            {
                var stats = Stats();
                Beurten.Add((Beurten.Count + 1, stats.Item1, stats.Item2));
                WisselBeurt();
            }
        }


        public bool Afgelopen()     // return true als geen van de spelers een zet kan doen
        {
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (ZetMogelijk(rijZet, kolomZet))
                    {
                        return false;
                    }
                }
                    
            }
            return true;
        }

        public Kleur OverwegendeKleur()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }
            if (aantalWit > aantalZwart)
                return Kleur.Wit;
            if (aantalZwart > aantalWit)
                return Kleur.Zwart;
            return Kleur.Geen;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
                throw new Exception($"Zet ({rijZet},{kolomZet}) ligt buiten het bord!");
            return ZetMogelijk(rijZet, kolomZet, AandeBeurt);
        }

        public void DoeZet(int rijZet, int kolomZet)
        {
            if (ZetMogelijk(rijZet, kolomZet, AandeBeurt))
            {
                for (int i = 0; i < 8; i++)
                {
                    {
                        if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet,
                                                                 AandeBeurt,
                                                                 richting[i, 0], richting[i, 1]))
                        {
                            DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(rijZet, kolomZet, AandeBeurt, richting[i, 0], richting[i, 1]);
                        }
                            
                    }
                }

                var stats = Stats();
                Beurten.Add((Beurten.Count + 1, stats.Item1, stats.Item2));

                bord[rijZet, kolomZet] = AandeBeurt;
                WisselBeurt();
            }


            else if (ZetMogelijk(rijZet, kolomZet))
            {
                bord[rijZet, kolomZet] = AandeBeurt;
                WisselBeurt();
            }
            else
            {
                throw new Exception($"Zet ({rijZet},{kolomZet}) is niet mogelijk!");
            }
        }

        private Tuple<int,int> Stats()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < 8; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < 8; kolomZet++)
                {
                    if (Bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (Bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }
            return new Tuple<int, int>(aantalWit, aantalZwart);
        }

        private static Kleur GetKleurTegenstander(Kleur kleur)
        {
            if (kleur == Kleur.Wit)
                return Kleur.Zwart;
            else if (kleur == Kleur.Zwart)
                return Kleur.Wit;
            else
                return Kleur.Geen;
        }

        private bool IsErEenZetMogelijk(Kleur kleur)
        {
            if (kleur == Kleur.Geen)
                throw new Exception("Kleur mag niet gelijk aan Geen zijn!");
            // controleeer of er een zet mogelijk is voor kleur
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (ZetMogelijk(rijZet, kolomZet, kleur))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ZetMogelijk(int rijZet, int kolomZet, Kleur kleur)
        {
            // Check of er een richting is waarin een zet mogelijk is. Als dat zo is, return dan true.
            for (int i = 0; i < 8; i++)
            {
                {
                    if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet,
                                                             kleur,
                                                             richting[i, 0], richting[i, 1]))
                        return true;
                }
            }
            return false;
        }

        private void WisselBeurt()
        {
            if (AandeBeurt == Kleur.Wit)
                AandeBeurt = Kleur.Zwart;
            else
                AandeBeurt = Kleur.Wit;
        }

        private static bool PositieBinnenBordGrenzen(int rij, int kolom)
        {
            return (rij >= 0 && rij < bordOmvang &&
                    kolom >= 0 && kolom < bordOmvang);
        }

        private bool ZetOpBordEnNogVrij(int rijZet, int kolomZet)
        {
            // Als op het bord gezet wordt, en veld nog vrij, dan return true, anders false
            return (PositieBinnenBordGrenzen(rijZet, kolomZet) && Bord[rijZet, kolomZet] == Kleur.Geen);
        }

        private bool StenenInTeSluitenInOpgegevenRichting(int rijZet, int kolomZet,
                                                          Kleur kleurZetter,
                                                          int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            if (!ZetOpBordEnNogVrij(rijZet, kolomZet))
                return false;

            // Zet rij en kolom op de index voor het eerst vakje naast de zet.
            rij = rijZet + rijRichting;
            kolom = kolomZet + kolomRichting;

            int aantalNaastGelegenStenenVanTegenstander = 0;
            // Zolang Bord[rij,kolom] niet buiten de bordgrenzen ligt, en je in het volgende vakje 
            // steeds de kleur van de tegenstander treft, ga je nog een vakje verder kijken.
            // Bord[rij, kolom] ligt uiteindelijk buiten de bordgrenzen, of heeft niet meer de
            // de kleur van de tegenstander.
            // N.b.: deel achter && wordt alleen uitgevoerd als conditie daarvoor true is.
            while (PositieBinnenBordGrenzen(rij, kolom) && Bord[rij, kolom] == kleurTegenstander)
            {
                rij += rijRichting;
                kolom += kolomRichting;
                aantalNaastGelegenStenenVanTegenstander++;
            }

            // Nu kijk je hoe je geeindigt bent met bovenstaande loop. Alleen
            // als alle drie onderstaande condities waar zijn, zijn er in de
            // opgegeven richting stenen in te sluiten.
            return (PositieBinnenBordGrenzen(rij, kolom) &&
                    Bord[rij, kolom] == kleurZetter &&
                    aantalNaastGelegenStenenVanTegenstander > 0);
        }

        private bool DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(int rijZet, int kolomZet,
                                                                                     Kleur kleurZetter,
                                                                                     int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            bool stenenOmgedraaid = false;

            if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleurZetter, rijRichting, kolomRichting))
            {
                rij = rijZet + rijRichting;
                kolom = kolomZet + kolomRichting;

                // N.b.: je weet zeker dat je niet buiten het bord belandt,
                // omdat de stenen van de tegenstander ingesloten zijn door
                // een steen van degene die de zet doet.
                while (Bord[rij, kolom] == kleurTegenstander)
                {
                    Bord[rij, kolom] = kleurZetter;
                    rij += rijRichting;
                    kolom += kolomRichting;
                }
                stenenOmgedraaid = true;
            }
            return stenenOmgedraaid;
        }
    }
}
