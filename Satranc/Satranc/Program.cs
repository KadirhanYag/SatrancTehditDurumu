using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Satranc
{
    class Program
    {
        private static double ToplamSiyahPuani ;
        private static int Pozisyon_X ;
        private static double ToplamBeyazPuani ;
        private static int Pozisyon_Y;
        public static string SatrancTahtasindaBostaOlanKisimlarinIfadesi = "--";
        private static string KaredeBulunanItem = string.Empty;

        static public Dictionary<char, int> SatrancTaslariVePuanlariTablosu = new Dictionary<char, int>();
        static Dictionary<(int, int), string> SatrancTaslarininKonumlariVeIsimleri = new Dictionary<(int, int), string>();
        static void Main(string[] args)
        {
            // Txt dosyası okunup bir diziye kaydediliyor.
            string[] TxtDosyasininIcerigi = File.ReadAllLines("Board.txt");
            
           // Tasların puanlarının olduğu tablo oluşturuluyor.
            SatrancTaslariVePuanlariTablosu.Add('p', 1);
            SatrancTaslariVePuanlariTablosu.Add('a', 3);
            SatrancTaslariVePuanlariTablosu.Add('f', 3);
            SatrancTaslariVePuanlariTablosu.Add('k', 5);
            SatrancTaslariVePuanlariTablosu.Add('v', 9);
            SatrancTaslariVePuanlariTablosu.Add('s', 100);

            for (int i = 0; i < TxtDosyasininIcerigi.Length; i++)
            {
                Pozisyon_Y = 0; 
                for (int Z = 0; Z < TxtDosyasininIcerigi[i].ToCharArray().Length; Z++)
                {
                    // txt dosyasında bulunan boşluklar ayrılıyor.
                    if (TxtDosyasininIcerigi[i].ToCharArray()[Z].Equals(' '))
                    {                        
                        DosyadakiIceriklerinAyrildigiMetot(); // Santraç tahtası üzerindeki ifadeler konumları ile kaydediliyor. Siyah ve Beyaz taşların tehdit durumu hesaba katılmadan puanı hesaplanıyor.
                    }
                    else
                    {
                        KaredeBulunanItem = KaredeBulunanItem + TxtDosyasininIcerigi[i].ToCharArray()[Z].ToString();
                    }

                }
                DosyadakiIceriklerinAyrildigiMetot();
                Pozisyon_X++;
            }

            TaslarVeHareketler _TaslarVeHareketler = new TaslarVeHareketler();

            _TaslarVeHareketler.TaslarinAyrimi(SatrancTaslarininKonumlariVeIsimleri);
            Console.WriteLine("Beyaz Taşların Puanı : {0}",ToplamBeyazPuani + _TaslarVeHareketler.BeyazTaraftanDusulecekPuan);
            Console.WriteLine("Siyah Taşların Puanı : {0}", ToplamSiyahPuani + _TaslarVeHareketler.SiyahTaraftanDusulecekPuan);
            Console.ReadKey();

        }

        private static void DosyadakiIceriklerinAyrildigiMetot()
        {
            SatrancTaslarininKonumlariVeIsimleri.Add((Pozisyon_X, Pozisyon_Y), KaredeBulunanItem); //Santraç tahtası üzerindeki ifadeler konumları ile kaydediliyor.
            if (!KaredeBulunanItem.Equals(SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                SiyahBeyazAyrımı(KaredeBulunanItem);
            }
            KaredeBulunanItem = string.Empty;
            Pozisyon_Y++;
        }

        private static void SiyahBeyazAyrımı(string _karedeBulunanItem) // Siyah ve Beyaz taşların tehdit durumu hesaba katılmadan puanı hesaplanıyor.
        {
            if (_karedeBulunanItem.EndsWith("s"))
            {
                ToplamSiyahPuani = ToplamSiyahPuani + SatrancTaslariVePuanlariTablosu[_karedeBulunanItem.First()];

            }
            if (_karedeBulunanItem.EndsWith("b"))
            {
                ToplamBeyazPuani = ToplamBeyazPuani + SatrancTaslariVePuanlariTablosu[_karedeBulunanItem.First()];

            }

        }
    }

    internal class TaslarVeHareketler
    {
        public int Tasin_X_Pozisyonu { get; private set; }
        public int Tasin_Y_Pozisyonu { get; private set; }
        public double BeyazTaraftanDusulecekPuan { get; private set; }
        public double SiyahTaraftanDusulecekPuan { get; private set; }

        public List<string> TehditEdilenTaslarinPozisyonu = new List<string>();
        internal void TaslarinAyrimi(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri) // Karede bulunan taşlarin ayrimi yapilarak tehdit durumları kontrol edilicek.
        {
            for (int i = 0; i <= satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1; i++)
            {
                for (int j = 0; j <= satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2; j++)
                {
                    if (!satrancTaslarininKonumlariVeIsimleri[(i, j)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                    {
                        switch (satrancTaslarininKonumlariVeIsimleri[(i, j)].First())
                        {
                            case 'k':
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                KALE(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                break;

                            case 'a':
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                AT(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                break;

                            case 'f':
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                FIL(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                break;

                            case 'v': // Vezir fil ve kalenin hareklerini uyguladıgından onlara ait metotlar ile kontrol ediliyor.
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                KALE(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                FIL(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);

                                break;

                            case 's': // Sahın çapraz hareketini piyon metodunda kontrol edildiğinden eksik kısımlar için sah metotu oluşturuldu.
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                PIYON(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                SAH(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                break;

                            case 'p':
                                Tasin_X_Pozisyonu = i;
                                Tasin_Y_Pozisyonu = j;
                                PIYON(satrancTaslarininKonumlariVeIsimleri, Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu);
                                break;
                        }
                    }
                }
            }
        }

        private void SAH(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1 , tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].First()] , satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu);
            }

            if (tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu, tasin_Y_Pozisyonu + 1);
            }

            if (tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu, tasin_Y_Pozisyonu - 1);
            }
        }

        private void PIYON(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].First()] , satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1);
            }

            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1);
            }
        }

        private void FIL(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            while (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1 , tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last())  && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].First()] , satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1);
                    break;
                }

                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }

                tasin_X_Pozisyonu++;
                tasin_Y_Pozisyonu++;
            }

            tasin_X_Pozisyonu = Tasin_X_Pozisyonu;
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu;

            while (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1);
                    break;
                }

                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }

                tasin_X_Pozisyonu--;
                tasin_Y_Pozisyonu--;
            }

            tasin_X_Pozisyonu = Tasin_X_Pozisyonu;
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu;

            while (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1);
                    break;
                }

                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }

                tasin_X_Pozisyonu--;
                tasin_Y_Pozisyonu++;
            }

            tasin_X_Pozisyonu = Tasin_X_Pozisyonu;
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu;

            while (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1);
                    break;
                }

                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }

                tasin_X_Pozisyonu++;
                tasin_Y_Pozisyonu--;
            }

            tasin_X_Pozisyonu = Tasin_X_Pozisyonu;
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu;
        }

        private void AT(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 - 1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 2)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu )].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 2)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 2)].First()] , satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu + 2);
            }

            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 - 1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 2)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 2)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 2)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu + 2);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 + 1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 2)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 2)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 2)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu - 2);
            }

            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 + 1 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 2)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 2)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 2)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu - 2);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 + 1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu + 1);
            }

            if (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item1 + 1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 2, tasin_Y_Pozisyonu - 1);
            }

            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 - 1 && tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu + 1);
            }

            if (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Last().Key.Item1 - 1 && tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.First().Key.Item2 && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last()) && !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
            {
                DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu, tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 2, tasin_Y_Pozisyonu - 1);
            }
        }

        private void KALE(Dictionary<(int, int), string> satrancTaslarininKonumlariVeIsimleri, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            while (tasin_X_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Keys.Last().Item1)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) && 
                    !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu);
                    break;
                }
                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu + 1, tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }
                tasin_X_Pozisyonu++;

            }

            tasin_X_Pozisyonu = Tasin_X_Pozisyonu; // while  da artırdığım için tekrar eski haline döndürdum.

            while (tasin_X_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.Keys.First().Item1)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) &&
                    !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu);
                    break;
                }
                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu - 1, tasin_Y_Pozisyonu)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }
                tasin_X_Pozisyonu--;

            }
            tasin_X_Pozisyonu = Tasin_X_Pozisyonu; // while  da artırdığım için tekrar eski haline döndürdum.

            while (tasin_Y_Pozisyonu < satrancTaslarininKonumlariVeIsimleri.Keys.Last().Item2)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) &&
                    !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu, tasin_Y_Pozisyonu + 1);
                    break;
                }
                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu + 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }
                tasin_Y_Pozisyonu++;

            }
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu; // while  da artırdığım için tekrar eski haline döndürdum

            while (tasin_Y_Pozisyonu > satrancTaslarininKonumlariVeIsimleri.Keys.First().Item1)
            {
                if (!satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()) &&
                    !satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].Equals(Program.SatrancTahtasindaBostaOlanKisimlarinIfadesi))
                {
                    DusulecekPuanHesaplanmasi(Program.SatrancTaslariVePuanlariTablosu[satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].First()], satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last(), tasin_X_Pozisyonu, tasin_Y_Pozisyonu - 1);
                    break;
                }
                if (satrancTaslarininKonumlariVeIsimleri[(tasin_X_Pozisyonu , tasin_Y_Pozisyonu - 1)].Last().Equals(satrancTaslarininKonumlariVeIsimleri[(Tasin_X_Pozisyonu, Tasin_Y_Pozisyonu)].Last()))
                {
                    break;
                }
                tasin_Y_Pozisyonu--;
                
            }
            tasin_Y_Pozisyonu = Tasin_Y_Pozisyonu;
        }

        private void DusulecekPuanHesaplanmasi(double Dusulecek_Puan, char Tas_Harfi, int tasin_X_Pozisyonu, int tasin_Y_Pozisyonu)
        {
            
            if (!TehditEdilenTaslarinPozisyonu.Contains(tasin_X_Pozisyonu.ToString() + tasin_Y_Pozisyonu.ToString())) // Bir kere tehdit edilen taşı tekrardan puan olarak düşmemek için list oluşturuldu. 
            {
                TehditEdilenTaslarinPozisyonu.Add(tasin_X_Pozisyonu.ToString() + tasin_Y_Pozisyonu.ToString());
                if (Tas_Harfi.Equals('s'))
                {
                    BeyazTaraftanDusulecekPuan = BeyazTaraftanDusulecekPuan - (Dusulecek_Puan / 2);
                }
                else
                {
                    SiyahTaraftanDusulecekPuan = SiyahTaraftanDusulecekPuan - (Dusulecek_Puan / 2);
                }
            }
            
        }
    }
}
