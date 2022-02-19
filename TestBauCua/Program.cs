using NLog;
using System;

namespace TestBauCua
{
    enum Animal
    {
        Bau = 1,
        Cua = 2,
        Tom = 3,
        Ca = 4,
        Ga = 5,
        Nai = 6
    }

    class Program
    {
        private const int TIEN_VON_GOC = 1_000_000;
        private static readonly ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static Animal dat_cua = Animal.Ga;

        private static int so_tien_dat_cuoc_default = 10_000; // 10 nghin (default)
        private static int tien_von_ban_dau = TIEN_VON_GOC; // 10 TRIEU
        private static bool? van_truoc_thang = null;
        private static int so_tien_cuoc_van_truoc;
        private static bool che_do_nga_o_dau_gap_doi_o_do = true;  // chế độ Ngã ở đâu gấp đôi ở đó 

        static int SoTienCuoc(int so_tien_hien_co)
        {
            if (che_do_nga_o_dau_gap_doi_o_do)
            {
                // van truoc thang, reset so tien
                if (van_truoc_thang == null || van_truoc_thang == true)
                {
                    if (so_tien_dat_cuoc_default <= so_tien_hien_co)
                    {
                        return so_tien_dat_cuoc_default;
                    }
                    else
                    {
                        return so_tien_hien_co;
                    }
                }
                else
                {
                    // van truoc thua, x2 tien cuoc                    
                    if ((so_tien_cuoc_van_truoc * 2) <= so_tien_hien_co)
                    {
                        return so_tien_cuoc_van_truoc * 2;
                    }
                    else
                    {
                        return so_tien_hien_co;
                    }
                }
            }
            else
            {
                while (true)
                {
                    var random = (int)Get64BitRandom(1, 5) * so_tien_dat_cuoc_default;
                    if (random <= so_tien_hien_co)
                    {
                        return random;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                int max_tien = 0;
                int max_tien_round = 0;

                var roundNumber = 1;
                var tien_von = tien_von_ban_dau;
                int? tien_von_50_rounds = null;

                int winx1 = 0;
                int winx2 = 0;
                int winx3 = 0;

                while (true)
                {
                    if (roundNumber == 51)
                    {
                        tien_von_50_rounds = tien_von;
                    }

                    var so_tien_dat_cuoc = SoTienCuoc(tien_von);
                    dat_cua = (Animal)(int)Get64BitRandom(1, 6);

                    var result1 = 0;
                    var result2 = 0;
                    var result3 = 0;

                    result1 = (int)Get64BitRandom(1, 6);
                    result2 = (int)Get64BitRandom(1, 6);
                    result3 = (int)Get64BitRandom(1, 6);

                    var isWinThisMatch = false;

                    var winxNumber = 0;
                    if ((int)dat_cua != result1 && (int)dat_cua != result2 && (int)dat_cua != result3)
                    {
                        tien_von = tien_von - so_tien_dat_cuoc;
                    }
                    else
                    {
                        isWinThisMatch = true;
                        if ((int)dat_cua == result1)
                        {
                            winxNumber++;
                            tien_von = tien_von + so_tien_dat_cuoc;
                        }

                        if ((int)dat_cua == result2)
                        {
                            winxNumber++;
                            tien_von = tien_von + so_tien_dat_cuoc;
                        }

                        if ((int)dat_cua == result3)
                        {
                            winxNumber++;
                            tien_von = tien_von + so_tien_dat_cuoc;
                        }
                    }

                    if (winxNumber == 1)
                    {
                        winx1++;
                    }

                    if (winxNumber == 2)
                    {
                        winx2++;
                    }

                    if (winxNumber == 3)
                    {
                        winx3++;
                    }

                    var msg = $"Luot choi\t[{roundNumber}] <{dat_cua.ToString()}>\t{((Animal)result1).ToString()} - {((Animal)result2).ToString()} - {((Animal)result3).ToString()}\t\t{(isWinThisMatch ? "THANG" : "THUA")}\tTien Cuoc: $ {so_tien_dat_cuoc:00,00.#}\tCon Lai: $ {tien_von:00,00.#}";
                    if (isWinThisMatch)
                    {
                        _logger.Info(msg);
                    }
                    else
                    {
                        _logger.Debug(msg);
                    }

                    if (tien_von > max_tien)
                    {
                        max_tien = tien_von;
                        max_tien_round = roundNumber;
                    }

                    if (tien_von <= 0)
                    {
                        _logger.Info("----------------------------------");
                        _logger.Info($"HET TIEN TAI LUOT CHOI <{roundNumber}>");
                        break;
                    }

                    van_truoc_thang = isWinThisMatch;
                    so_tien_cuoc_van_truoc = so_tien_dat_cuoc;

                    roundNumber++;
                }

                _logger.Info($"Tien von ban dau': $ {TIEN_VON_GOC:00,00.#}");
                _logger.Info($"Che do choi Nga o dau gap doi o do dang {(che_do_nga_o_dau_gap_doi_o_do ? "BAT" : "TAT")}");
                _logger.Info($"So tien thang cao nhat': $ {max_tien:00,00.#} tai luot choi [{max_tien_round}]");

                _logger.Info($"Win x1: {winx1} lan`");
                _logger.Info($"Win x2: {winx2} lan`");
                _logger.Info($"Win x3: {winx3} lan`");

                if (tien_von_50_rounds != null)
                {
                    _logger.Info($"Tien con` lai tai round 50: $ {tien_von_50_rounds:00,00.#}");
                }
                Console.WriteLine();
                Console.WriteLine("======================================================================");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static readonly Random rnd = new Random();
        private static ulong Get64BitRandom(ulong minValue, ulong maxValue)
        {
            // Get a random array of 8 bytes. 
            // As an option, you could also use the cryptography namespace stuff to generate a random byte[8]
            byte[] buffer = new byte[sizeof(ulong)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0) % (maxValue - minValue + 1) + minValue;
        }
    }
}
