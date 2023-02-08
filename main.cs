using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Schema;
using System.IO;

namespace Training_PBL2
{

    class Program
    {
        public static bool demo_mode = false;
        static void menu()
        {
            Console.SetCursorPosition(24, 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"");
            System.Threading.Thread.Sleep(500);
            Console.WriteLine(@" \ \      / __|  |      __|   _ \   \  |  __|   __ __| _ \    __ __| |  |  __|     __|  |  |  __|   __|   __|     __|    \     \  |  __|  ");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine(@"  \ \ \  /  _|   |     (     (   | |\/ |  _|       |  (   |      |   __ |  _|     (     __ |  _|  \__ \ \__ \    (_ |   _ \   |\/ |  _|    ");
            System.Threading.Thread.Sleep(100);
            Console.WriteLine(@"   \_/\_/  ___| ____| \___| \___/ _|  _| ___|     _| \___/      _|  _| _| ___|   \___| _| _| ___| ____/ ____/   \___| _/  _\ _|  _| ___|    ");
            System.Threading.Thread.Sleep(100);
            Console.WriteLine(@"                                                                                                                                          ");
            System.Threading.Thread.Sleep(100);

            Console.ForegroundColor = ConsoleColor.Cyan;
            System.Threading.Thread.Sleep(100);
            Console.SetCursorPosition(20, 15);
            Console.WriteLine("Which mode dou you want to play ?");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Threading.Thread.Sleep(160);
            Console.SetCursorPosition(20, 17);
            Console.WriteLine("(P)lay mode");
            System.Threading.Thread.Sleep(200);
            Console.SetCursorPosition(20, 18);
            Console.WriteLine("(D)emo mode");
            Console.SetCursorPosition(20, 19);
            Console.ForegroundColor = ConsoleColor.White;
            string mode = Console.ReadLine();
            if (mode == "P")
            {
                Console.Clear();
                Console.SetCursorPosition(20, 10);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter Red player's name:");
                Console.SetCursorPosition(20, 11);
                name1 = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(20, 15);
                Console.WriteLine("Please enter Blue player's name:");
                Console.SetCursorPosition(20, 16);
                name2 = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(20, 20);
                System.Threading.Thread.Sleep(100);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("3");
                System.Threading.Thread.Sleep(160);
                Console.Write("     2");
                System.Threading.Thread.Sleep(220);
                Console.Write("     1");
                Console.SetCursorPosition(30, 8);
                System.Threading.Thread.Sleep(220);
                Console.ForegroundColor = ConsoleColor.White;

            }
            if (mode == "D")
            {
                Console.Clear();
                Console.SetCursorPosition(20, 0);
                Console.WriteLine("Please enter game file that you want to read");
                filename = Console.ReadLine();
                demo_mode = true;
            }
        }

        public static string name1 = "bahadır", name2 = "gufran", filename = "saved_game";
        public static int check_count = 0;
        public static int turn_counter = 0;
        static string[,] boardarray = new string[8, 8];
        public static int bahadır = 0;
        public static string[] redpieces = {
      "Pr",
      "Rr",
      "Nr",
      "Br",
      "Qr",
      "Kr"
    };
        public static string[] bluepieces = {
      "Pb",
      "Rb",
      "Nb",
      "Bb",
      "Qb",
      "Kb"
    };
        public static bool capture = false;
        public static string redmove = "";
        public static string bluemove = "";
        public static string red_temp = "";
        public static string blue_temp = "";
        public static bool turn = false;
        public static bool gotpromoted = false;
        public static string promoted = "";
        public static bool enpassant_red = false;
        public static bool enpassant_blue = false;
        public static bool redkingsidecastle = true;
        public static bool redqueensidecastle = true;
        public static bool bluekingsidecastle = true;
        public static bool bluequeensidecastle = true;
        public static bool bluelongcastle = false;
        public static bool blueshortcastle = false;
        public static bool redlongcastle = false;
        public static bool redshortcastle = false;
        public static bool[] red_pawns = new bool[8];
        public static bool[] blue_pawns = new bool[8];
        public static string file_redmove = "";
        public static string file_bluemove = "";
        public static string filemoves = "";
        public static bool bishopsameplace = false;
        public static bool rooksameplace = false;
        public static string addnotation = "";
        public static string[] filemoves_splitted;
        public static bool matered = false, mateblue = false;

        static void Main(string[] args)
        {
            menu();
            Console.Clear();
            for (int i = 0; i < red_pawns.Length; i++)
            {
                red_pawns[i] = false;
            }

            for (int i = 0; i < blue_pawns.Length; i++)
            {
                blue_pawns[i] = false;
            }


            static void promotion(string[,] boardarray, int arraycx, int arraycy, string mp)
            {
                int cx = arraycx * 4 + 7;
                int cy = arraycy * 2 + 5;
                Console.SetCursorPosition(4, 24);
                Console.Write("Congratulations! You got promoted.Please enter the piece you want to continue with:");
                promoted = Console.ReadLine();
                Console.SetCursorPosition(4, 24);
                Console.Write("                                                                                        ");
                boardarray[arraycx, arraycy] = promoted + mp[1];
                Console.SetCursorPosition(cx, cy);
                if (mp[1] == 'b')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(promoted);

            }
            static void castling(string[,] boardarray, int mx, int my, int cx, int cy)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                if (arraymx == 4 && arraymy == 7 && arraycx == 7 && arraycy == 7 && boardarray[arraycx, arraycy] == "Rr" && boardarray[arraymx, arraymy] == "Kr" && boardarray[arraymx + 1, arraymy] == "." && boardarray[arraymx + 2, arraymy] == "." && redkingsidecastle == true)
                {
                    boardarray[5, 7] = "Rr";
                    boardarray[6, 7] = "Kr";
                    boardarray[4, 7] = ".";
                    boardarray[7, 7] = ".";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(27, 19);
                    Console.Write("R");
                    Console.SetCursorPosition(31, 19);
                    Console.Write("K");
                    Console.ResetColor();
                    Console.SetCursorPosition(23, 19);
                    Console.Write(".");
                    Console.SetCursorPosition(35, 19);
                    Console.Write(".");
                    redshortcastle = true;
                    Notation(boardarray, mx, my, cx, cy, "Kr", turn_counter);
                    turn_counter++;
                    turn = true;

                }

                else if (arraymx == 4 && arraymy == 7 && arraycx == 0 && arraycy == 7 && boardarray[arraycx, arraycy] == "Rr" && boardarray[arraymx, arraymy] == "Kr" && boardarray[arraymx - 1, arraymy] == "." && boardarray[arraymx - 2, arraymy] == "." && boardarray[arraymx - 3, arraymy] == "." && redqueensidecastle == true)
                {
                    boardarray[3, 7] = "Rr";
                    boardarray[2, 7] = "Kr";
                    boardarray[4, 7] = ".";
                    boardarray[0, 7] = ".";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(19, 19);
                    Console.Write("R");
                    Console.SetCursorPosition(15, 19);
                    Console.Write("K");
                    Console.ResetColor();
                    Console.SetCursorPosition(23, 19);
                    Console.Write(".");
                    Console.SetCursorPosition(7, 19);
                    Console.Write(".");
                    redlongcastle = true;
                    Notation(boardarray, mx, my, cx, cy, "Kr", turn_counter);
                    turn_counter++;
                    turn = true;

                }

                else if (arraymx == 4 && arraymy == 0 && arraycx == 7 && arraycy == 0 && boardarray[arraycx, arraycy] == "Rb" && boardarray[arraymx, arraymy] == "Kb" && boardarray[arraymx + 1, arraymy] == "." && boardarray[arraymx + 2, arraymy] == "." && bluekingsidecastle == true)
                {
                    boardarray[5, 0] = "Rb";
                    boardarray[6, 0] = "Kb";
                    boardarray[4, 0] = ".";
                    boardarray[7, 0] = ".";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(27, 5);
                    Console.Write("R");
                    Console.SetCursorPosition(31, 5);
                    Console.Write("K");
                    Console.ResetColor();
                    Console.SetCursorPosition(23, 5);
                    Console.Write(".");
                    Console.SetCursorPosition(35, 5);
                    Console.Write(".");
                    blueshortcastle = true;
                    Notation(boardarray, mx, my, cx, cy, "Kb", turn_counter);
                    turn_counter++;
                    turn = false;

                }
                else if (arraymx == 4 && arraymy == 0 && arraycx == 0 && arraycy == 0 && boardarray[arraycx, arraycy] == "Rb" && boardarray[arraymx, arraymy] == "Kb" && boardarray[arraymx - 1, arraymy] == "." && boardarray[arraymx - 2, arraymy] == "." && boardarray[arraymx - 3, arraymy] == "." && bluequeensidecastle == true)
                {
                    boardarray[3, 0] = "Rr";
                    boardarray[2, 0] = "Kr";
                    boardarray[4, 0] = ".";
                    boardarray[0, 0] = ".";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(19, 5);
                    Console.Write("R");
                    Console.SetCursorPosition(15, 5);
                    Console.Write("K");
                    Console.ResetColor();
                    Console.SetCursorPosition(23, 5);
                    Console.Write(".");
                    Console.SetCursorPosition(7, 5);
                    Console.Write(".");
                    bluelongcastle = true;
                    Notation(boardarray, mx, my, cx, cy, "Kb", turn_counter);
                    turn_counter++;
                    turn = false;

                }
                else
                {
                    Console.SetCursorPosition(6, 24);
                    Console.Write("Seems like you can't do that");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(6, 24);
                    Console.Write("                               ");

                }

            }

            static void cangosameplacerook(string[,] boardarray, int mx, int my, int cx, int cy, bool turn)
            {
                int cangosameplacex = -1;
                int cangosameplacey = -1;
                int notationboardcy = cy;
                int notationboardcx = cx;
                for (int i = notationboardcy + 1; i < boardarray.GetLength(0); i++)
                {
                    if (boardarray[notationboardcx, i] != ".")
                    {
                        if (boardarray[notationboardcx, i] == "Rr" && turn == true && my != i)
                        {
                            cangosameplacex = notationboardcx;
                            cangosameplacey = i;
                            break;
                        }
                        if (boardarray[notationboardcx, i] == "Rb" && turn == false && my != i)
                        {
                            cangosameplacex = notationboardcx;
                            cangosameplacey = i;
                            break;
                        }
                        break;
                    }

                }
                for (int i = notationboardcy - 1; i > 0; i--)
                {
                    if (i >= 0 && boardarray[notationboardcx, i] != ".")
                    {

                        if (boardarray[notationboardcx, i] == "Rr" && turn == true && my != i)
                        {
                            cangosameplacex = notationboardcx;
                            cangosameplacey = i;
                            break;
                        }
                        if (boardarray[notationboardcx, i] == "Rb" && turn == false && my != i)
                        {
                            cangosameplacex = notationboardcx;
                            cangosameplacey = i;
                            break;
                        }
                        break;
                    }
                }

                //yatay
                for (int i = notationboardcx + 1; i < boardarray.GetLength(1); i++)
                {

                    if (boardarray[i, notationboardcy] != ".")
                    {

                        if (boardarray[i, notationboardcy] == "Rr" && turn == true && my != i)
                        {
                            cangosameplacex = i;
                            cangosameplacey = notationboardcy;
                            break;

                        }
                        if (boardarray[i, notationboardcy] == "Rb" && turn == false && my != i)
                        {
                            cangosameplacex = i;
                            cangosameplacey = notationboardcy;
                            break;
                        }
                        break;
                    }

                }
                for (int i = notationboardcx - 1; i >= 0; i--)
                {

                    if (boardarray[i, notationboardcy] != ".")
                    {

                        if (boardarray[i, notationboardcy] == "Rr" && mx != i && turn == true)
                        {
                            cangosameplacex = i;
                            cangosameplacey = notationboardcy;
                            break;
                        }
                        if (boardarray[i, notationboardcy] == "Rb" && mx != i && turn == false)
                        {
                            cangosameplacex = i;
                            cangosameplacey = notationboardcy;
                            break;
                        }
                        break;
                    }
                }
                string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
                if (cangosameplacex > -1 && cangosameplacey > -1 &&
                    boardarray[cangosameplacex, cangosameplacey] != boardarray[cx, cy])
                {
                    if (cangosameplacex == mx)
                    {
                        addnotation += (8 - cangosameplacey).ToString();
                    }

                    else if (cangosameplacey == my)
                    {
                        addnotation += letters[mx];
                    }
                    else if (cangosameplacex != mx && cangosameplacey != my)
                    {
                        addnotation += letters[mx];
                    }
                }


            }

            static void cangosameplaceknight(string[,] boardarray, int mx, int my, int cx, int cy, bool turn)
            {
                int cangosameplacex = -1;
                int cangosameplacey = -1;
                int notationboardcy = cy;
                int notationboardcx = cx;


                if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx + 2, notationboardcy + 1] == "Nr" && turn == true &&
                    notationboardcx + 2 != mx && notationboardcy + 1 != my)
                {
                    cangosameplacex = notationboardcx + 2;
                    cangosameplacey = notationboardcy + 1;

                }
                if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx + 2, notationboardcy - 1] == "Nr" && turn == true &&
                    notationboardcx + 2 != mx && notationboardcy - 1 != my)
                {
                    cangosameplacex = notationboardcx + 2;
                    cangosameplacey = notationboardcy - 1;

                }
                if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx - 2, notationboardcy + 1] == "Nr" && turn == true &&
                    notationboardcx - 2 != mx && notationboardcy + 1 != my)
                {
                    cangosameplacex = notationboardcx - 2;
                    cangosameplacey = notationboardcy + 1;

                }
                if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx - 2, notationboardcy - 1] == "Nr" && turn == true &&
                    notationboardcx - 2 != mx && notationboardcy - 1 != my)
                {
                    cangosameplacex = notationboardcx - 2;
                    cangosameplacey = notationboardcy - 1;

                }
                if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx + 1, notationboardcy + 2] == "Nr" && turn == true &&
                    notationboardcx + 1 != mx && notationboardcy + 2 != my)
                {
                    cangosameplacex = notationboardcx + 1;
                    cangosameplacey = notationboardcy + 2;

                }
                if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx + 1, notationboardcy - 2] == "Nr" && turn == true &&
                    notationboardcx + 1 != mx && notationboardcy - 2 != my)
                {
                    cangosameplacex = notationboardcx + 1;
                    cangosameplacey = notationboardcy - 2;

                }
                if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx - 1, notationboardcy + 2] == "Nr" && turn == true &&
                    notationboardcx - 1 != mx && notationboardcy + 2 != my)
                {
                    cangosameplacex = notationboardcx - 1;
                    cangosameplacey = notationboardcy + 2;

                }
                if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx - 1, notationboardcy - 2] == "Nr" && turn == true &&
                    notationboardcx - 1 != mx && notationboardcy - 2 != my)
                {
                    cangosameplacex = notationboardcx - 1;
                    cangosameplacey = notationboardcy - 2;

                }

                if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx + 2, notationboardcy + 1] == "Nb" && turn == false &&
                    notationboardcx + 2 != mx && notationboardcy + 1 != my)
                {
                    cangosameplacex = notationboardcx + 2;
                    cangosameplacey = notationboardcy + 1;

                }
                if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx + 2, notationboardcy - 1] == "Nb" && turn == false &&
                    notationboardcx + 2 != mx && notationboardcy - 1 != my)
                {
                    cangosameplacex = notationboardcx + 2;
                    cangosameplacey = notationboardcy - 1;

                }
                if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx - 2, notationboardcy + 1] == "Nb" && turn == false &&
                    notationboardcx - 2 != mx && notationboardcy + 1 != my)
                {
                    cangosameplacex = notationboardcx - 2;
                    cangosameplacey = notationboardcy + 1;

                }
                if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx - 2, notationboardcy - 1] == "Nb" && turn == false &&
                    notationboardcx - 2 != mx && notationboardcy - 1 != my)
                {
                    cangosameplacex = notationboardcx - 2;
                    cangosameplacey = notationboardcy - 1;

                }
                if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx + 1, notationboardcy + 2] == "Nb" && turn == false &&
                    notationboardcx + 1 != mx && notationboardcy + 2 != my)
                {
                    cangosameplacex = notationboardcx + 1;
                    cangosameplacey = notationboardcy + 2;

                }
                if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx + 1, notationboardcy - 2] == "Nb" && turn == false &&
                    notationboardcx + 1 != mx && notationboardcy - 2 != my)
                {
                    cangosameplacex = notationboardcx + 1;
                    cangosameplacey = notationboardcy - 2;

                }
                if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx - 1, notationboardcy + 2] == "Nb" && turn == false &&
                    notationboardcx - 1 != mx && notationboardcy + 2 != my)
                {
                    cangosameplacex = notationboardcx - 1;
                    cangosameplacey = notationboardcy + 2;

                }

                if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx - 1, notationboardcy - 2] == "Nb" && turn == false &&
                    notationboardcx - 1 != mx && notationboardcy - 2 != my)
                {
                    cangosameplacex = notationboardcx - 1;
                    cangosameplacey = notationboardcy - 2;

                }

                string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
                if (cangosameplacex > -1 && cangosameplacey > -1 &&
                    boardarray[cangosameplacex, cangosameplacey] != boardarray[cx, cy])
                {
                    if (cangosameplacex == mx)
                    {
                        addnotation += (8 - cangosameplacey).ToString();
                    }

                    if (cangosameplacey == my)
                    {
                        addnotation += letters[mx];
                    }
                    else if (cangosameplacex != mx && cangosameplacey != my)
                    {
                        addnotation += letters[mx];
                    }
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(cangosameplacex);
                    Console.WriteLine(cangosameplacey);
                    Console.WriteLine(addnotation);
                }
            }

            static void pawn(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;

                if ((mp[1] == 'b' && arraycy == 7) || (mp[1] == 'r' && arraycy == 0))
                {
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    promotion(boardarray, arraycx, arraycy, mp);
                    if (mp[1] == 'b')
                    {
                        turn = false;
                    }
                    if (mp[1] == 'r') turn = true;
                    turn_counter++;
                    gotpromoted = true;

                }
                else
                {
                    if ((cx == mx) && Math.Abs(cy - my) == 2 && (boardarray[arraycx, arraycy] == "."))
                    {
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        if (mp[1] == 'b')
                        {
                            turn = false;
                        }

                        if (mp[1] == 'r') turn = true;
                        turn_counter++;
                    }

                    if (((cx == mx) && Math.Abs(cy - my) == 4 && (boardarray[arraycx, arraycy] == ".")) && (arraymy == 6 && mp[1] == 'r' || arraymy == 1 && mp[1] == 'b'))
                    {
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        if (mp[1] == 'b')
                        {
                            turn = false;
                            blue_pawns[arraycx] = true;
                            for (int i = 0; i < blue_pawns.Length; i++)
                            {
                                if (i != arraycx)
                                {
                                    blue_pawns[i] = false;
                                }
                            }
                        }

                        if (mp[1] == 'r')
                        {
                            turn = true;
                            red_pawns[arraycx] = true;
                            for (int i = 0; i < blue_pawns.Length; i++)
                            {
                                if (i != arraycx)
                                {
                                    red_pawns[i] = false;
                                }
                            }

                        }

                        turn_counter++;
                    }

                }

                if ((Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 4 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1] || Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]))
                {
                    capture = true;
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    {
                        turn = false;
                    }
                    if (mp[1] == 'r') turn = true;
                    turn_counter++;
                }
            }
            static void rook(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;

                if (jumpcontrol(boardarray, mx, my, cx, cy, mp) == false)
                {
                    if ((boardarray[arraycx, arraycy] == "." && (arraycy < arraymy || arraycy > arraymy)) || (boardarray[arraycx, arraycy] == "." && (arraycx < arraymx || arraycx > arraymx)))
                    {

                        if (mp[1] == 'b')
                        {
                            if (arraymx == 0 && arraymy == 0) bluequeensidecastle = false;
                            if (arraymx == 7 && arraymy == 0) bluekingsidecastle = false;
                            turn = false;

                        }

                        if (mp[1] == 'r')
                        {
                            turn = true;
                            if (arraymx == 0 && arraymy == 7) redqueensidecastle = false;
                            if (arraymx == 7 && arraymy == 7) redkingsidecastle = false;
                        }
                        cangosameplacerook(boardarray, arraymx, arraymy, arraycx, arraycy, turn);
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        turn_counter++;
                    }

                    if (((boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) && (arraycy < arraymy || arraycy > arraymy)) || ((boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) && (arraycx < arraymx || arraycx > arraymx)))
                    {
                        capture = true;
                        if (mp[1] == 'b')
                        {
                            if (arraymx == 0 && arraymy == 0) bluequeensidecastle = false;
                            if (arraymx == 7 && arraymy == 0) bluekingsidecastle = false;
                            turn = false;

                        }

                        if (mp[1] == 'r')
                        {
                            turn = true;
                            if (arraymx == 0 && arraymy == 7) redqueensidecastle = false;
                            if (arraymx == 7 && arraymy == 7) redkingsidecastle = false;
                        }
                        cangosameplacerook(boardarray, arraymx, arraymy, arraycx, arraycy, turn);
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        turn_counter++;

                    }
                }
            }
            static void knight(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                if (Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 4 && boardarray[arraycx, arraycy] == "." || Math.Abs(cx - mx) == 8 && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] == ".")
                {

                    if (mp[1] == 'b')
                    {
                        turn = false;
                    }

                    if (mp[1] == 'r') turn = true;
                    cangosameplaceknight(boardarray, arraymx, arraymy, arraycx, arraycy, turn);
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    turn_counter++;

                }

                if (Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 4 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1] || Math.Abs(cx - mx) == 8 && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1])
                {
                    capture = true;

                    if (mp[1] == 'b')
                    {
                        turn = false;
                    }

                    if (mp[1] == 'r') turn = true;
                    cangosameplaceknight(boardarray, arraymx, arraymy, arraycx, arraycy, turn);
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    turn_counter++;

                }

            }
            static void bishop(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                if (jumpcontrol(boardarray, mx, my, cx, cy, mp) == false)
                {
                    if (Math.Abs(arraycx - arraymx) == Math.Abs(arraycy - arraymy) && boardarray[arraycx, arraycy] == ".")
                    {
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        if (mp[1] == 'b')
                        {
                            turn = false;
                        }

                        if (mp[1] == 'r') turn = true;
                        turn_counter++;
                    }

                    if (Math.Abs(arraycx - arraymx) == Math.Abs(arraycy - arraymy) && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1])
                    {
                        capture = true;
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        if (mp[1] == 'b')
                        {
                            turn = false;
                        }

                        if (mp[1] == 'r') turn = true;
                        turn_counter++;
                    }
                }
            }
            static void queen(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                if (jumpcontrol(boardarray, mx, my, cx, cy, mp) == false)
                {
                    if ((Math.Abs(arraycx - arraymx) == Math.Abs(arraycy - arraymy) && boardarray[arraycx, arraycy] == ".") || ((boardarray[arraycx, arraycy] == "." && (arraycy < arraymy || arraycy > arraymy)) || (boardarray[arraycx, arraycy] == "." && (arraycx < arraymx || arraycx > arraymx))))
                    {
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        {
                            turn = false;
                        }
                        if (mp[1] == 'r') turn = true;
                        turn_counter++;
                    }

                    if (Math.Abs(arraycx - arraymx) == Math.Abs(arraycy - arraymy) && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1] || ((boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) && (arraycy < arraymy || arraycy > arraymy)) || ((boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) && (arraycx < arraymx || arraycx > arraymx)))
                    {
                        capture = true;
                        boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                        if (mp[1] == 'b')
                        {
                            turn = false;
                        }

                        if (mp[1] == 'r') turn = true;
                        turn_counter++;
                    }
                }
            }
            static void king(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                if ((Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] == ".") || (cx == mx && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] == ".") || (cy == my && Math.Abs(cx - mx) == 4 && boardarray[arraycx, arraycy] == "."))
                {
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    if (mp[1] == 'b')
                    {
                        turn = false;
                        bluekingsidecastle = false;
                        bluekingsidecastle = false;
                    }

                    if (mp[1] == 'r')
                    {
                        turn = true;
                        redkingsidecastle = false;
                        redqueensidecastle = false;
                    }
                    turn_counter++;

                }
                if ((Math.Abs(cx - mx) == 4 && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) || (cx == mx && Math.Abs(cy - my) == 2 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]) || (cy == my && Math.Abs(cx - mx) == 4 && boardarray[arraycx, arraycy] != "." && boardarray[arraycx, arraycy][1] != mp[1]))
                {
                    capture = true;
                    boardarray = placer(boardarray, mx, my, cx, cy, mp, turn_counter);
                    if (mp[1] == 'b')
                    {
                        turn = false;
                    }
                    if (mp[1] == 'r')
                    {
                        turn = true;

                    }
                    turn_counter++;
                }
            }
            static void hint(string[,] boardarray)
            {
                int notationboardcx, notationboardmx = -1;
                int notationboardcy, notationboardmy = -1;
                string hintpiece = " ";
                string mp;
                char[] letters = {
          'a',
          'b',
          'c',
          'd',
          'e',
          'f',
          'g',
          'h'
        };

                for (int f = 0; f < boardarray.GetLength(0); f++)
                {
                    for (int f2 = 0; f2 < boardarray.GetLength(1); f2++)
                    {

                        notationboardcx = -1;
                        notationboardmx = -1;
                        if (boardarray[f, f2] != ".")
                        {
                            notationboardcx = f;
                            notationboardcy = f2;
                            mp = boardarray[f, f2];
                            if (mp[0] == 'R' || mp[0] == 'Q')
                            {
                                //dikey
                                for (int i = notationboardcy + 1; i < boardarray.GetLength(0); i++)
                                {
                                    if (boardarray[notationboardcx, i] != ".")
                                    {
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx, i][1] == 'r' && turn == true)
                                        {
                                            notationboardmx = notationboardcx;
                                            notationboardmy = i;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx] + (8 - i);
                                            break;
                                        }
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx, i][1] == 'b' && turn == false)
                                        {
                                            notationboardmx = notationboardcx;
                                            notationboardmy = i;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx] + (8 - i);
                                            break;
                                        }
                                        break;
                                    }

                                }
                                for (int i = notationboardcy - 1; i > 0; i--)
                                {
                                    if (boardarray[notationboardcx, i] != ".")
                                    {
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx, i][1] == 'r' && turn == true)
                                        {
                                            notationboardmx = notationboardcx;
                                            notationboardmy = i;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx] + (8 - i);
                                            break;
                                        }
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx, i][1] == 'b' && turn == false)
                                        {
                                            notationboardmx = notationboardcx;
                                            notationboardmy = i;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx] + (8 - i);
                                            break;
                                        }
                                        break;
                                    }
                                }

                                //yatay
                                for (int i = notationboardcx + 1; i < boardarray.GetLength(1); i++)
                                {
                                    if (boardarray[i, notationboardcy] != ".")
                                    {
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[i, notationboardcy][1] == 'r' && turn == true)
                                        {
                                            notationboardmx = i;
                                            notationboardmy = notationboardcy;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[i] + (8 - i);
                                            break;

                                        }
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[i, notationboardcy][1] == 'b' && turn == false)
                                        {
                                            notationboardmx = i;
                                            notationboardmy = notationboardcy;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[i] + (8 - i);
                                            break;
                                        }
                                        break;
                                    }

                                }
                                for (int i = notationboardcx - 1; i >= 0; i--)
                                {
                                    if (boardarray[i, notationboardcy] != ".")
                                    {
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[i, notationboardcy][1] == 'r' && turn == true)
                                        {
                                            notationboardmx = i;
                                            notationboardmy = notationboardcy;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[i] + (8 - i);
                                            break;
                                        }
                                        if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[i, notationboardcy][1] == 'b' && turn == false)
                                        {
                                            notationboardmx = i;
                                            notationboardmy = notationboardcy;
                                            hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[i] + (8 - i);
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }

                            /////////////////////
                            ///

                            if (mp[0] == 'B' || mp[0] == 'Q')
                            {
                                int axis_y = notationboardcy;
                                int axis_x = notationboardcx;
                                if (notationboardcx + 1 <= boardarray.GetLength(0))
                                {
                                    // sağ aşşağı 
                                    for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                                    {
                                        axis_x++;
                                        if (axis_x < boardarray.GetLength(1))
                                        {
                                            if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                            {
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[axis_x, i][1] == 'r' && turn == true)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[axis_x, i][1] == 'b' && turn == false)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    // sol aşşağı
                                    axis_y = notationboardcy;
                                    axis_x = notationboardcx;
                                    for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                                    {
                                        axis_x--;
                                        if (axis_x >= 0)
                                        {
                                            if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                            {
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[axis_x, i][1] == 'r' && turn == true)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[axis_x, i][1] == 'b' && turn == false)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                break;
                                            }
                                        }
                                    }

                                }
                                axis_y = notationboardcy;
                                axis_x = notationboardcx;
                                if (notationboardcy + 1 < boardarray.GetLength(1))
                                {
                                    // sol yukarı
                                    for (int i = notationboardcy - 1; i >= 0; i--)
                                    {
                                        axis_x--;
                                        if (0 <= axis_x && Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                        {
                                            if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[axis_x, i][1] == 'r' && turn == true)
                                            {
                                                notationboardmx = axis_x;
                                                notationboardmy = i;
                                                hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                break;
                                            }
                                            if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[axis_x, i][1] == 'b' && turn == false)
                                            {
                                                notationboardmx = axis_x;
                                                notationboardmy = i;
                                                hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                break;
                                            }
                                            break;
                                        }

                                    }
                                    axis_y = notationboardcy;
                                    axis_x = notationboardcx;
                                    // sağ yukarı
                                    for (int i = notationboardcy - 1; i >= 0; i--)
                                    {
                                        axis_x++;
                                        if (axis_x < boardarray.GetLength(1))
                                        {
                                            if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                            {
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[axis_x, i][1] == 'r' && turn == true)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                if (boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[axis_x, i][1] == 'b' && turn == false)
                                                {
                                                    notationboardmx = axis_x;
                                                    notationboardmy = i;
                                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[axis_x] + (8 - i);
                                                    break;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            notationboardcx = f;
                            notationboardcy = f2;
                            if (mp[0] == 'N')
                            {
                                if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx + 2, notationboardcy + 1] != "." && boardarray[notationboardcx + 2, notationboardcy + 1][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx + 2;
                                    notationboardmy = notationboardcy + 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 2] + (8 - (notationboardcy + 1));
                                }
                                if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx + 2, notationboardcy - 1] != "." && boardarray[notationboardcx + 2, notationboardcy - 1][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx + 2;
                                    notationboardmy = notationboardcy - 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 2] + (8 - (notationboardcy - 1));

                                }
                                if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx - 2, notationboardcy + 1] != "." && boardarray[notationboardcx - 2, notationboardcy + 1][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx - 2;
                                    notationboardmy = notationboardcy + 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 2] + (8 - (notationboardcy + 1));
                                }
                                if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx - 2, notationboardcy - 1] != "." && boardarray[notationboardcx - 2, notationboardcy - 1][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx - 2;
                                    notationboardmy = notationboardcy - 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 2] + (8 - (notationboardcy - 1));
                                }
                                if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx + 1, notationboardcy + 2] != "." && boardarray[notationboardcx + 1, notationboardcy + 2][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx + 1;
                                    notationboardmy = notationboardcy + 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy + 2));
                                }
                                if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx + 1, notationboardcy - 2] != "." && boardarray[notationboardcx + 1, notationboardcy - 2][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx + 1;
                                    notationboardmy = notationboardcy - 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy - 2));
                                }
                                if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx - 1, notationboardcy + 2] != "." && boardarray[notationboardcx - 1, notationboardcy + 2][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx - 1;
                                    notationboardmy = notationboardcy + 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy + 2));
                                }
                                if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'b' && boardarray[notationboardcx - 1, notationboardcy - 2] != "." && boardarray[notationboardcx - 1, notationboardcy - 2][1] == 'r' && turn == true)
                                {
                                    notationboardmx = notationboardcx - 1;
                                    notationboardmy = notationboardcy - 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy - 2));
                                }
                                ////

                                if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx + 2, notationboardcy + 1] != "." && boardarray[notationboardcx + 2, notationboardcy + 1][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx + 2;
                                    notationboardmy = notationboardcy + 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 2] + (8 - (notationboardcy + 1));
                                }
                                if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx + 2, notationboardcy - 1] != "." && boardarray[notationboardcx + 2, notationboardcy - 1][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx + 2;
                                    notationboardmy = notationboardcy - 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 2] + (8 - (notationboardcy - 1));
                                }
                                if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx - 2, notationboardcy + 1] != "." && boardarray[notationboardcx - 2, notationboardcy + 1][1] == 'b' && turn == false)
                                {

                                    notationboardmx = notationboardcx - 2;
                                    notationboardmy = notationboardcy + 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 2] + (8 - (notationboardcy + 1));
                                }
                                if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx - 2, notationboardcy - 1] != "." && boardarray[notationboardcx - 2, notationboardcy - 1][1] == 'b' && turn == false)
                                {

                                    notationboardmx = notationboardcx - 2;
                                    notationboardmy = notationboardcy - 1;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 2] + (8 - (notationboardcy - 1));
                                }
                                if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx + 1, notationboardcy + 2] != "." && boardarray[notationboardcx + 1, notationboardcy + 2][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx + 1;
                                    notationboardmy = notationboardcy + 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy + 2));
                                }
                                if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx + 1, notationboardcy - 2] != "." && boardarray[notationboardcx + 1, notationboardcy - 2][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx + 1;
                                    notationboardmy = notationboardcy - 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy - 2));
                                }
                                if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx - 1, notationboardcy + 2] != "." && boardarray[notationboardcx - 1, notationboardcy + 2][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx - 1;
                                    notationboardmy = notationboardcy + 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy + 2));
                                }
                                if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx, notationboardcy][1] == 'r' && boardarray[notationboardcx - 1, notationboardcy - 2] != "." && boardarray[notationboardcx - 1, notationboardcy - 2][1] == 'b' && turn == false)
                                {
                                    notationboardmx = notationboardcx - 1;
                                    notationboardmy = notationboardcy - 2;
                                    hintpiece += " " + boardarray[notationboardcx, notationboardcy][0] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy - 2));
                                }

                            }
                            if (mp[0] == 'P')
                            {
                                if (notationboardcx + 1 < 8 && turn == true && boardarray[notationboardcx, notationboardcy][1] == 'b')
                                {
                                    if (boardarray[notationboardcx + 1, notationboardcy + 1] != "." && boardarray[notationboardcx + 1, notationboardcy + 1][1] == 'r')
                                    {
                                        hintpiece += " " + letters[notationboardcx] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy + 1));
                                    }
                                }
                                if (notationboardcx - 1 > 0 && turn == true && boardarray[notationboardcx, notationboardcy][1] == 'b')
                                {
                                    if (boardarray[notationboardcx - 1, notationboardcy + 1] != "." && boardarray[notationboardcx - 1, notationboardcy + 1][1] == 'r')
                                    {
                                        hintpiece += " " + letters[notationboardcx] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy + 1));
                                    }

                                }

                                if (notationboardcx + 1 < 8 && turn == false && boardarray[notationboardcx, notationboardcy][1] == 'r')
                                {
                                    if (boardarray[notationboardcx + 1, notationboardcy - 1] != "." && boardarray[notationboardcx + 1, notationboardcy - 1][1] == 'b')
                                    {
                                        hintpiece += " " + letters[notationboardcx] + "x" + letters[notationboardcx + 1] + (8 - (notationboardcy - 1));
                                    }
                                }
                                if (notationboardcx - 1 > 0 && turn == false && boardarray[notationboardcx, notationboardcy][1] == 'r')
                                {
                                    if (boardarray[notationboardcx - 1, notationboardcy - 1] != "." && boardarray[notationboardcx - 1, notationboardcy - 1][1] == 'b')
                                    {
                                        hintpiece += " " + letters[notationboardcx] + "x" + letters[notationboardcx - 1] + (8 - (notationboardcy - 1));
                                    }
                                }


                            }

                        }


                    }

                }
                Console.SetCursorPosition(45, 45);
                Console.WriteLine(hintpiece);



            }
            static string[,] placer(string[,] boardarray, int mx, int my, int cx, int cy, string mp, int counter)
            {
                Console.SetCursorPosition(mx, my);
                Console.Write(".");
                Console.SetCursorPosition(cx, cy);
                if (mp != "." && mp[1] == 'b')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine(mp[0]);
                Console.ForegroundColor = ConsoleColor.White;

                string[,] str = new string[8, 8];
                str = boardarray;
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                str[arraycx, arraycy] = mp;
                str[arraymx, arraymy] = ".";

                return str;
            }
            static void Notation(string[,] boardarray, int mx, int my, int cx, int cy, string mp, int turn_counter)
            {
                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;

                string[,,] notationarray = new string[8, 8, 2];
                for (int i = 0; i < notationarray.GetLength(0); i++)
                {
                    for (int j = 0; j < notationarray.GetLength(1); j++)
                    {
                        notationarray[i, j, 0] = boardarray[i, j];

                    }
                }

                string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
                for (int i = 0; i < notationarray.GetLength(0); i++)
                {

                    for (int j = 0; j < notationarray.GetLength(1); j++)
                    {

                        notationarray[i, j, 1] = letters[i] + (8 - j);

                    }
                }

                for (int i = 0; i < turn_counter; i++)
                {
                    Console.SetCursorPosition(60, 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(name1);
                    Console.SetCursorPosition(73, 2);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(name2);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.SetCursorPosition(58, turn_counter + 1);
                    Console.WriteLine("|");
                    Console.SetCursorPosition(85, turn_counter + 1);
                    Console.WriteLine("|");
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(65, 1);
                Console.WriteLine("Notations");

                if (turn_counter % 2 == 1 && mp[1] == 'r')
                {
                    if (redshortcastle == true)
                    {
                        redmove += "O-O";
                        redshortcastle = false;
                    }
                    else if (redlongcastle == true)
                    {
                        redmove += "O-O-O";
                        redlongcastle = false;
                    }
                    else
                    {
                        if (mp[0] == 'P')
                        {
                            if (capture == true)
                            {
                                redmove += letters[arraymx] + "x";
                            }
                            redmove += notationarray[arraycx, arraycy, 1];
                            if (enpassant_red == true)
                            {
                                redmove += "e.p.";
                            }
                        }

                        if (mp[0] != 'P')
                        {
                            redmove += mp[0];
                            if (addnotation != "")
                            {
                                redmove += addnotation;
                            }
                            if (capture == true)
                            {
                                redmove += "x";
                            }

                            redmove += notationarray[arraycx, arraycy, 1];
                        }

                    }

                    if (check_count > 0)
                    {
                        redmove += "+";
                    }

                    red_temp = (turn_counter / 2 + 1).ToString() + ") " + redmove;
                    if (turn_counter == 1)
                    {
                        filemoves += red_temp;
                    }
                    else
                    {
                        filemoves += "  " + red_temp;
                    }
                    Console.SetCursorPosition(60, 3 + bahadır);
                    Console.Write(red_temp);
                    bluemove = "";
                    red_temp = "";
                }
                else if (turn_counter % 2 == 0 && mp[1] == 'b')
                {
                    if (blueshortcastle == true)
                    {
                        bluemove += "O-O";
                        blueshortcastle = false;
                    }
                    else if (bluelongcastle == true)
                    {
                        bluemove += "O-O-O";
                        bluelongcastle = false;
                    }
                    else
                    {
                        if (mp[0] == 'P')
                        {
                            if (capture == true)
                            {
                                bluemove += letters[arraymx] + "x";
                            }
                            bluemove += notationarray[arraycx, arraycy, 1];
                            if (enpassant_blue == true)
                            {
                                bluemove += "e.p.";
                            }
                        }

                        if (mp[0] != 'P')
                        {
                            bluemove += mp[0];
                            if (addnotation != "")
                            {
                                bluemove += addnotation;
                            }
                            if (capture == true)
                            {
                                bluemove += "x";
                            }

                            bluemove += notationarray[arraycx, arraycy, 1];
                        }
                    }
                    if (gotpromoted == true)
                    {
                        bluemove += promoted;
                    }
                    if (check_count > 0)
                    {
                        bluemove += "+";
                    }
                    blue_temp += bluemove;
                    filemoves += " " + bluemove;
                    Console.SetCursorPosition(73, 3 + bahadır);
                    Console.Write(bluemove);
                    bahadır++;
                    redmove = "";


                }

                addnotation = "";

            }
            static bool jumpcontrol(string[,] boardarray, int mx, int my, int cx, int cy, string mp)
            {

                int arraymx = (mx - 7) / 4;
                int arraycx = (cx - 7) / 4;
                int arraymy = (my - 5) / 2;
                int arraycy = (cy - 5) / 2;
                bool atla = false;
                if (mp[0] == 'R' || mp[0] == 'Q')
                {
                    if (my == cy)
                    {
                        int temp;
                        if (arraycx < arraymx)
                        {
                            temp = arraymx;
                            arraymx = arraycx;
                            arraycx = temp;
                        }

                        for (int i = arraymx + 1; i < arraycx; i++)
                        {
                            if (boardarray[i, arraymy] != ".")
                            {
                                atla = true;
                            }
                        }
                    }

                    if (mx == cx)
                    {
                        int temp;
                        if (arraymy < arraycy)
                        {
                            temp = arraymy;
                            arraymy = arraycy;
                            arraycy = temp;
                        }

                        for (int i = arraycy + 1; i < arraymy; i++)
                        {
                            if (boardarray[arraymx, i] != ".")
                            {
                                atla = true;
                            }
                        }
                    }

                }

                if (mp[0] == 'B' || mp[0] == 'Q')
                {

                    if (cx > mx && my > cy) // sağ yukarı
                    {
                        for (int i = (((mx) - 7) / 4) + 1; i < ((cx) - 7) / 4; i++)
                        {
                            arraymy--;
                            if (boardarray[i, arraymy] != ".") atla = true;
                        }
                    }

                    if (cx < mx && my > cy) // sol yukarı 
                    {
                        for (int i = arraymy - 1; i > arraycy; i--)
                        {
                            arraymx--;
                            if (boardarray[arraymx, i] != ".") atla = true;
                        }
                    }

                    if (cx > mx && my < cy) //sağ aşağı
                    {
                        for (int i = (((mx) - 7) / 4) + 1; i < ((cx) - 7) / 4; i++)
                        {

                            arraymy++;

                            if (boardarray[i, arraymy] != ".") atla = true;

                        }
                    }

                    if (cx < mx && my < cy) // sol aşağı
                    {
                        for (int i = (((cx) - 7) / 4) + 1; i > (((mx) - 7) / 4); i--)
                        {
                            arraymy++;

                            if (boardarray[i, arraymy] != ".") atla = true;

                        }
                    }
                }

                return atla;

            }
            static int check_mate(string[,] boardarray, bool turn)
            {
                int K_x = 0;
                int K_y = 0;
                int axis_x = 0;
                int axis_y = 0;
                string a = "";
                string b = "";
                if (turn == false)
                {
                    a = "r";
                    b = "b";
                }
                else
                {
                    a = "b";
                    b = "r";
                }

                for (int i = 0; i < boardarray.GetLength(0); i++)
                {
                    for (int j = 0; j < boardarray.GetLength(1); j++)
                    {
                        if (boardarray[i, j] == "K" + a)
                        {
                            K_x = i;
                            K_y = j;
                            break;
                        }
                    }
                }

                if (boardarray[K_x, K_y] == "K" + a)
                {
                    //dikey
                    for (int i = K_y + 1; i < boardarray.GetLength(0); i++)
                    {
                        if (boardarray[K_x, i][0] == 'P' || boardarray[K_x, i][0] == 'N' || boardarray[K_x, i][0] == 'B' || boardarray[K_x, i][0] == 'K')
                        {
                            break;
                        }

                        if (boardarray[K_x, i] == "R" + b || boardarray[K_x, i] == "Q" + b)
                        {
                            check_count++;
                            break;

                        }
                    }

                    for (int i = K_y - 1; i > 0; i--)
                    {
                        if (boardarray[K_x, i][0] == 'P' || boardarray[K_x, i][0] == 'N' || boardarray[K_x, i][0] == 'B' || boardarray[K_x, i][0] == 'K')
                        {
                            break;
                        }

                        if (boardarray[K_x, i] == "R" + b || boardarray[K_x, i] == "Q" + b)
                        {
                            check_count++;
                            break;

                        }
                    }

                    //yatay
                    for (int i = K_x + 1; i < boardarray.GetLength(1); i++)
                    {
                        if (boardarray[i, K_y][0] == 'P' || boardarray[i, K_y][0] == 'N' || boardarray[i, K_y][0] == 'B' || boardarray[i, K_y][0] == 'K')
                        {
                            break;
                        }

                        if (boardarray[i, K_y] == "R" + b || boardarray[i, K_y] == "Q" + b)
                        {
                            check_count++;
                            break;

                        }

                    }

                    for (int i = K_x - 1; i > 0; i--)
                    {
                        if (boardarray[i, K_y][0] == 'P' || boardarray[i, K_y][0] == 'N' || boardarray[i, K_y][0] == 'B' || boardarray[i, K_y][0] == 'K')
                        {
                            break;
                        }

                        if (boardarray[i, K_y] == "R" + b || boardarray[i, K_y] == "Q" + b)
                        {
                            check_count++;
                            break;

                        }
                    }

                    axis_y = K_y;
                    axis_x = K_x;
                    if (K_x + 1 < boardarray.GetLength(0))
                    {
                        //sağ aşağı
                        for (int i = K_y + 1; i < boardarray.GetLength(1); i++)
                        {
                            axis_x++;
                            if (axis_x < boardarray.GetLength(1))
                            {
                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i][0] == 'P' || boardarray[axis_x, i][0] == 'N' || boardarray[axis_x, i][0] == 'R' || boardarray[axis_x, i][0] == 'K'))
                                {
                                    break;
                                }

                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i] == "Q" + b || boardarray[axis_x, i] == "B" + b))
                                {
                                    check_count++;
                                    break;
                                }
                            }

                        }

                        //sol aşağı
                        axis_y = K_y;
                        axis_x = K_x;
                        for (int i = K_y + 1; i < boardarray.GetLength(1); i++)
                        {
                            axis_x--;
                            if (axis_x > 0)
                            {
                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i][0] == 'P' || boardarray[axis_x, i][0] == 'N' || boardarray[axis_x, i][0] == 'R' || boardarray[axis_x, i][0] == 'K'))
                                {
                                    break;
                                }

                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i] == "Q" + b || boardarray[axis_x, i] == "B" + b))
                                {
                                    check_count++;
                                    break;
                                }
                            }
                        }
                    }

                    axis_y = K_y;
                    axis_x = K_x;
                    if (K_y + 1 < boardarray.GetLength(1))
                    {
                        //sol yukarı
                        for (int i = K_y - 1; i > 0; i--)
                        {
                            axis_x--;
                            if (axis_x > 0)
                            {
                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i][0] == 'P' || boardarray[axis_x, i][0] == 'N' || boardarray[axis_x, i][0] == 'R' || boardarray[axis_x, i][0] == 'K'))
                                {
                                    break;
                                }

                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i] == "Q" + b || boardarray[axis_x, i] == "Bb" + b))
                                {
                                    check_count++;
                                    break;
                                }
                            }
                        }

                        axis_y = K_y;
                        axis_x = K_x;
                        //sağ yukarı
                        for (int i = K_y - 1; i > 0; i--)
                        {
                            axis_x++;
                            if (axis_x < boardarray.GetLength(1))
                            {
                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i][0] == 'P' || boardarray[axis_x, i][0] == 'N' || boardarray[axis_x, i][0] == 'R' || boardarray[axis_x, i][0] == 'K'))
                                {
                                    break;
                                }

                                if (Math.Abs(K_y - i) == Math.Abs(K_x - axis_x) && (boardarray[axis_x, i] == "Q" + b || boardarray[axis_x, i] == "Bb" + b))
                                {
                                    check_count++;
                                    break;
                                }
                            }
                        }
                    }
                    //piyon 

                    if ((K_x + 1 < 8 && K_y + 1 < 8 && boardarray[K_x + 1, K_y + 1] == "P" + b) || (K_x - 1 >= 0 && K_y - 1 >= 0 && boardarray[K_x - 1, K_y - 1] == "P" + b) || (K_x - 1 < 8 && K_y + 1 < 8 && boardarray[K_x - 1, K_y + 1] == "P" + b) || (K_x + 1 < 8 && K_y - 1 >= 0 && boardarray[K_x + 1, K_y - 1] == "P" + b))
                    {
                        check_count++;
                    }

                    //at

                    if ((K_x + 2 < 8 && K_y + 1 < 8 && boardarray[K_x + 2, K_y + 1] == "N" + b) || (K_x + 2 < 8 && K_y - 1 >= 0 && boardarray[K_x + 2, K_y - 1] == "N" + b) || (K_x - 2 >= 0 && K_y + 1 < 8 && boardarray[K_x - 2, K_y + 1] == "N" + b) || (K_x - 2 >= 0 && K_y - 1 >= 0 && boardarray[K_x - 2, K_y - 1] == "N" + b) || (K_x + 1 < 8 && K_y + 2 < 8 && boardarray[K_x + 1, K_y + 2] == "N" + b) || (K_x + 1 < 8 && K_y - 2 >= 0 && boardarray[K_x + 1, K_y - 2] == "N" + b) || (K_x - 1 >= 0 && K_y + 2 < 8 && boardarray[K_x - 1, K_y + 2] == "N" + b) || (K_x - 1 >= 0 && K_y - 2 >= 0 && boardarray[K_x - 1, K_y - 2] == "N" + b))
                    {
                        check_count++;
                    }

                }

                return check_count;
            }
            static string[,] strmove(string[,] boardarray, string mp, bool turn)
            {
                int notationboardcx = 0, notationboardcy = 0, notationboardmx = 0, notationboardmy = 0, turncounter = 0;
                string movednotationp;
                char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
                // gidilen yerin x ve y si piyonla gidilmişse

                if (mp.Length == 2) // hareket eden piyonsa 
                {
                    if (letters.Contains(mp[0]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[0])
                            {
                                notationboardcx = i;
                            }
                        }
                    }
                    notationboardcy = (8 - (mp[1] - '0'));
                    notationboardmx = notationboardcx;
                    if (turn == true) // mavi hareket
                    {
                        if (boardarray[notationboardcx, notationboardcy - 1][0] == 'P')
                        {
                            notationboardmy = notationboardcy - 1;
                        }
                        else if (boardarray[notationboardcx, notationboardcy - 2][0] == 'P')
                        {
                            notationboardmy = notationboardcy - 2;
                        }
                        mp = "Pb";
                    }
                    if (turn == false) // kırmızı hareket
                    {
                        if (boardarray[notationboardcx, notationboardcy + 1][0] == 'P')
                        {
                            notationboardmy = notationboardcy + 1;
                        }
                        else if (boardarray[notationboardcx, notationboardcy + 2][0] == 'P')
                        {
                            notationboardmy = notationboardcy + 2;
                        }
                        mp = "Pr";
                    }
                }
                // buraya 3 karekter ifi
                if (mp.Length == 3 || mp[1] == 'x')
                {
                    if (mp.Length == 3)
                    {
                        if (letters.Contains(mp[1]))
                        {
                            for (int i = 0; i < letters.Length; i++)
                            {
                                if (letters[i] == mp[1])
                                {
                                    notationboardcx = i;
                                }
                            }
                        }
                        notationboardcy = (8 - (mp[2] - '0'));
                    }
                    if (mp[1] == 'x' && !letters.Contains(mp[0]))
                    {
                        if (letters.Contains(mp[2]))
                        {
                            for (int i = 0; i < letters.Length; i++)
                            {
                                if (letters[i] == mp[2])
                                {
                                    notationboardcx = i;
                                }
                            }
                        }
                        notationboardcy = (8 - (mp[3] - '0'));

                    }
                    if (mp[1] == 'x' && letters.Contains(mp[0]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[2])
                            {
                                notationboardcx = i;
                            }
                        }
                        notationboardcy = (8 - (mp[3] - '0'));
                        // pawn yeme 
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[0])
                            {
                                notationboardmx = i;
                            }
                        }
                        if (boardarray[notationboardmx, notationboardcy - 1] != "." && boardarray[notationboardmx, notationboardcy - 1][1] == 'b')
                        {
                            notationboardmy = notationboardcy - 1;
                        }
                        if (boardarray[notationboardmx, notationboardcy + 1] != "." && boardarray[notationboardmx, notationboardcy + 1][1] == 'r')
                        {
                            notationboardmy = notationboardcy + 1;
                        }

                    }
                    if (mp[0] == 'Q')
                    {
                        //dikey
                        for (int i = notationboardcy + 1; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[notationboardcx, i] != ".")
                            {
                                if (boardarray[notationboardcx, i] == "Qr" && turn == false)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                if (boardarray[notationboardcx, i] == "Qb" && turn == true)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                break;
                            }

                        }
                        for (int i = notationboardcy - 1; i > 0; i--)
                        {
                            if (boardarray[notationboardcx, i] != ".")
                            {
                                if (boardarray[notationboardcx, i] == "Qr" && turn == false)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                if (boardarray[notationboardcx, i] == "Qb" && turn == true)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                break;
                            }
                        }
                        for (int i = notationboardcx + 1; i < boardarray.GetLength(1); i++)
                        {
                            if (boardarray[i, notationboardcy] != ".")
                            {
                                if (boardarray[i, notationboardcy] == "Qr" && turn == false)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;

                                }
                                if (boardarray[i, notationboardcy] == "Qb" && turn == true)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                break;
                            }

                        }
                        for (int i = notationboardcx - 1; i >= 0; i--)
                        {
                            if (boardarray[i, notationboardcy] != ".")
                            {
                                if (boardarray[i, notationboardcy] == "Qr" && turn == false)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                if (boardarray[i, notationboardcy] == "Qb" && turn == true)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                break;
                            }
                        }
                        int axis_y = notationboardcy;
                        int axis_x = notationboardcx;
                        if (notationboardcx + 1 <= boardarray.GetLength(0))
                        {
                            // sağ aşşağı 
                            for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                            {
                                axis_x++;
                                if (axis_x < boardarray.GetLength(1))
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                    {
                                        if (boardarray[axis_x, i] == "Qr" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        if (boardarray[axis_x, i] == "Qb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                            // sol aşşağı
                            axis_y = notationboardcy;
                            axis_x = notationboardcx;
                            for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                            {
                                axis_x--;
                                if (axis_x >= 0)
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                    {
                                        if (boardarray[axis_x, i] == "Qr" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;

                                        }
                                        if (boardarray[axis_x, i] == "Qb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }

                                }
                            }
                        }
                        axis_y = notationboardcy;
                        axis_x = notationboardcx;
                        if (notationboardcy + 1 < boardarray.GetLength(1))
                        {
                            // sol yukarı
                            for (int i = notationboardcy - 1; i >= 0; i--)
                            {
                                axis_x--;
                                if (0 <= axis_x && Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                {
                                    if (boardarray[axis_x, i] == "Qr" && turn == false)
                                    {
                                        notationboardmx = axis_x;
                                        notationboardmy = i;
                                        break;
                                    }
                                    if (boardarray[axis_x, i] == "Qb" && turn == true)
                                    {
                                        notationboardmx = axis_x;
                                        notationboardmy = i;
                                        break;
                                    }
                                    break;
                                }

                            }
                            axis_y = notationboardcy;
                            axis_x = notationboardcx;
                            // sağ yukarı
                            for (int i = notationboardcy - 1; i >= 0; i--)
                            {
                                axis_x++;
                                if (axis_x < boardarray.GetLength(1))
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                    {
                                        if (boardarray[axis_x, i] == "Qr" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        if (boardarray[axis_x, i] == "Qb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    if (mp[0] == 'R')
                    {
                        //dikey
                        for (int i = notationboardcy + 1; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[notationboardcx, i] != ".")
                            {
                                if (boardarray[notationboardcx, i] == "Rr" && turn == false)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                if (boardarray[notationboardcx, i] == "Rb" && turn == true)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                break;
                            }

                        }
                        for (int i = notationboardcy - 1; i > 0; i--)
                        {
                            if (boardarray[notationboardcx, i] != ".")
                            {
                                if (boardarray[notationboardcx, i] == "Rr" && turn == false)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                if (boardarray[notationboardcx, i] == "Rb" && turn == true)
                                {
                                    notationboardmx = notationboardcx;
                                    notationboardmy = i;
                                    break;
                                }
                                break;
                            }
                        }

                        //yatay
                        for (int i = notationboardcx + 1; i < boardarray.GetLength(1); i++)
                        {
                            if (boardarray[i, notationboardcy] != ".")
                            {
                                if (boardarray[i, notationboardcy] == "Rr" && turn == false)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;

                                }
                                if (boardarray[i, notationboardcy] == "Rb" && turn == true)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                break;
                            }

                        }
                        for (int i = notationboardcx - 1; i >= 0; i--)
                        {
                            if (boardarray[i, notationboardcy] != ".")
                            {
                                if (boardarray[i, notationboardcy] == "Rr" && turn == false)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                if (boardarray[i, notationboardcy] == "Rb" && turn == true)
                                {
                                    notationboardmx = i;
                                    notationboardmy = notationboardcy;
                                    break;
                                }
                                break;
                            }
                        }

                    }

                    if (mp[0] == 'B')
                    {
                        int axis_y = notationboardcy;
                        int axis_x = notationboardcx;
                        if (notationboardcx + 1 <= boardarray.GetLength(0))
                        {
                            // sağ aşşağı 
                            for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                            {
                                axis_x++;
                                if (axis_x < boardarray.GetLength(1))
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                    {
                                        if (boardarray[axis_x, i] == "Br" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        if (boardarray[axis_x, i] == "Bb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                            // sol aşşağı
                            axis_y = notationboardcy;
                            axis_x = notationboardcx;
                            for (int i = notationboardcy + 1; i < boardarray.GetLength(1); i++)
                            {
                                axis_x--;
                                if (axis_x >= 0)
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                    {
                                        if (boardarray[axis_x, i] == "Br" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;

                                        }
                                        if (boardarray[axis_x, i] == "Bb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }

                                }

                            }

                        }
                        axis_y = notationboardcy;
                        axis_x = notationboardcx;
                        if (notationboardcy + 1 < boardarray.GetLength(1))
                        {
                            // sol yukarı
                            for (int i = notationboardcy - 1; i >= 0; i--)
                            {
                                axis_x--;
                                if (0 <= axis_x && Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && (boardarray[axis_x, i] != "."))
                                {
                                    if (boardarray[axis_x, i] == "Br" && turn == false)
                                    {
                                        notationboardmx = axis_x;
                                        notationboardmy = i;
                                        break;
                                    }
                                    if (boardarray[axis_x, i] == "Bb" && turn == true)
                                    {
                                        notationboardmx = axis_x;
                                        notationboardmy = i;
                                        break;
                                    }
                                    break;
                                }

                            }
                            axis_y = notationboardcy;
                            axis_x = notationboardcx;
                            // sağ yukarı
                            for (int i = notationboardcy - 1; i >= 0; i--)
                            {
                                axis_x++;
                                if (axis_x < boardarray.GetLength(1))
                                {
                                    if (Math.Abs(notationboardcy - i) == Math.Abs(notationboardcx - axis_x) && boardarray[axis_x, i] != ".")
                                    {
                                        if (boardarray[axis_x, i] == "Br" && turn == false)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        if (boardarray[axis_x, i] == "Bb" && turn == true)
                                        {
                                            notationboardmx = axis_x;
                                            notationboardmy = i;
                                            break;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (mp[0] == 'N')
                    {
                        if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx + 2, notationboardcy + 1] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx + 2;
                            notationboardmy = notationboardcy + 1;
                        }
                        if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx + 2, notationboardcy - 1] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx + 2;
                            notationboardmy = notationboardcy - 1;
                        }
                        if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx - 2, notationboardcy + 1] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx - 2;
                            notationboardmy = notationboardcy + 1;
                        }
                        if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx - 2, notationboardcy - 1] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx - 2;
                            notationboardmy = notationboardcy - 1;
                        }
                        if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx + 1, notationboardcy + 2] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx + 1;
                            notationboardmy = notationboardcy + 2;
                        }
                        if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx + 1, notationboardcy - 2] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx + 1;
                            notationboardmy = notationboardcy - 2;
                        }
                        if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx - 1, notationboardcy + 2] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx - 1;
                            notationboardmy = notationboardcy + 2;
                        }
                        if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx - 1, notationboardcy - 2] == "Nr" && turn == false)
                        {
                            notationboardmx = notationboardcx - 1;
                            notationboardmy = notationboardcy - 2;
                        }


                        if (notationboardcx + 2 < 8 && notationboardcy + 1 < 8 && boardarray[notationboardcx + 2, notationboardcy + 1] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx + 2;
                            notationboardmy = notationboardcy + 1;
                        }
                        if (notationboardcx + 2 < 8 && notationboardcy - 1 >= 0 && boardarray[notationboardcx + 2, notationboardcy - 1] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx + 2;
                            notationboardmy = notationboardcy - 1;
                        }
                        if (notationboardcx - 2 >= 0 && notationboardcy + 1 < 8 && boardarray[notationboardcx - 2, notationboardcy + 1] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx - 2;
                            notationboardmy = notationboardcy + 1;
                        }
                        if (notationboardcx - 2 >= 0 && notationboardcy - 1 >= 0 && boardarray[notationboardcx - 2, notationboardcy - 1] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx - 2;
                            notationboardmy = notationboardcy - 1;
                        }
                        if (notationboardcx + 1 < 8 && notationboardcy + 2 < 8 && boardarray[notationboardcx + 1, notationboardcy + 2] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx + 1;
                            notationboardmy = notationboardcy + 2;
                        }
                        if (notationboardcx + 1 < 8 && notationboardcy - 2 >= 0 && boardarray[notationboardcx + 1, notationboardcy - 2] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx + 1;
                            notationboardmy = notationboardcy - 2;
                        }
                        if (notationboardcx - 1 >= 0 && notationboardcy + 2 < 8 && boardarray[notationboardcx - 1, notationboardcy + 2] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx - 1;
                            notationboardmy = notationboardcy + 2;
                        }
                        if (notationboardcx - 1 >= 0 && notationboardcy - 2 >= 0 && boardarray[notationboardcx - 1, notationboardcy - 2] == "Nb" && turn == true)
                        {
                            notationboardmx = notationboardcx - 1;
                            notationboardmy = notationboardcy - 2;
                        }
                    }

                }
                if (mp.Length == 4 && mp[1] != 'x')
                {
                    if (letters.Contains(mp[2]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[2])
                            {
                                notationboardcx = i;
                            }
                        }
                    }
                    notationboardcy = (8 - (mp[3] - '0'));
                    if (letters.Contains(mp[1]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[1])
                            {
                                notationboardmx = i;
                            }
                        }
                        for (int i = 0; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[notationboardmx, i][0] == mp[0] && boardarray[notationboardmx, i][1] == 'r' && turn == false)
                                notationboardmy = i;
                            if (boardarray[notationboardmx, i][0] == mp[0] && boardarray[notationboardmx, i][1] == 'b' && turn == true)
                                notationboardmy = i;
                        }

                    }
                    else
                    {
                        notationboardmy = 8 - (mp[1] - '0');
                        for (int i = 0; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[i, notationboardmy][0] == mp[0] && boardarray[i, notationboardmy][1] == 'r' && turn == false)
                            {
                                notationboardmx = i;
                            }
                            if (boardarray[i, notationboardmy][0] == mp[0] && boardarray[i, notationboardmy][1] == 'b' && turn == true)
                            {
                                notationboardmx = i;
                            }

                        }
                    }
                }
                if (mp.Length == 5 && mp[2] == 'x')
                {
                    if (letters.Contains(mp[3]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[3])
                            {
                                notationboardcx = i;
                            }
                        }
                    }
                    notationboardcy = (8 - (mp[4] - '0'));
                    if (letters.Contains(mp[1]))
                    {
                        for (int i = 0; i < letters.Length; i++)
                        {
                            if (letters[i] == mp[1])
                            {
                                notationboardmx = i;
                            }
                        }
                        for (int i = 0; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[notationboardmx, i][0] == mp[0] && boardarray[notationboardmx, i][1] == 'r' && turn == false)
                                notationboardmy = i;
                            if (boardarray[notationboardmx, i][0] == mp[0] && boardarray[notationboardmx, i][1] == 'b' && turn == true)
                                notationboardmy = i;
                        }
                    }
                    else
                    {
                        notationboardmy = mp[1] - '0';
                        notationboardmy = 8 - notationboardmy;
                        for (int i = 0; i < boardarray.GetLength(0); i++)
                        {
                            if (boardarray[i, notationboardmy][0] == mp[0] && boardarray[i, notationboardmy][1] == 'r' && turn == false)
                            {
                                notationboardmx = i;
                            }
                            if (boardarray[i, notationboardmy][0] == mp[0] && boardarray[i, notationboardmy][1] == 'b' && turn == true)
                            {
                                notationboardmx = i;
                            }
                        }
                    }

                }

                Console.WriteLine(notationboardmx);
                Console.WriteLine(notationboardmy);
                movednotationp = boardarray[notationboardmx, notationboardmy];
                notationboardcx = notationboardcx * 4 + 7;
                notationboardcy = notationboardcy * 2 + 5;
                notationboardmx = notationboardmx * 4 + 7;
                notationboardmy = notationboardmy * 2 + 5;



                boardarray = placer(boardarray, notationboardmx, notationboardmy, notationboardcx, notationboardcy, movednotationp, turncounter);


                if (turn == true)
                {
                    Console.SetCursorPosition(42, 6);
                    Console.WriteLine("          ");
                    Console.SetCursorPosition(42, 18);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your Turn");
                    Console.ForegroundColor = ConsoleColor.White;

                }
                else
                {
                    Console.SetCursorPosition(42, 18);
                    Console.WriteLine("          ");
                    Console.SetCursorPosition(42, 6);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Your Turn");
                    Console.ForegroundColor = ConsoleColor.White;
                }


                return boardarray;

            }
            static void reading(bool turn)
            {
                string line = " ";
                StreamReader f = File.OpenText(filename + ".txt");
                for (int i = 0; i < turn_counter / 2; i++)
                {
                    line = f.ReadLine();
                }
                line = f.ReadLine();


                if (line == null)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("End of the saved game");
                }
                else
                {
                    int space = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                        {
                            space = i;
                            break;
                        }
                    }
                    line = line.Substring(space + 1);
                    if (!line.Contains(' '))
                    {

                    }

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                        {
                            space = i;
                            break;
                        }

                    }
                    string move1 = line.Substring(0, space);
                    string move2 = "";
                    if (line.Contains(' '))
                    {
                        move2 = line.Substring(space + 1);
                    }



                    move1 = string.Concat(move1.Where(c => !Char.IsWhiteSpace(c)));
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(move1 + "k");
                    Console.WriteLine(move2 + "k");
                    if (turn == false)
                    {
                        strmove(boardarray, move1, turn);
                        Console.SetCursorPosition(60, 3 + bahadır);
                        Console.WriteLine((turn_counter / 2 + 1).ToString() + ") " + move1);
                        Console.SetCursorPosition(58, turn_counter);
                        Console.WriteLine("|");
                        Console.SetCursorPosition(85, turn_counter);
                        Console.WriteLine("|");

                    }
                    else if (line.Contains(' '))
                    {
                        Console.SetCursorPosition(73, 3 + bahadır);
                        Console.WriteLine(move2);
                        strmove(boardarray, move2, turn);
                        bahadır++;
                        Console.SetCursorPosition(58, turn_counter);
                        Console.WriteLine("|");
                        Console.SetCursorPosition(85, turn_counter);
                        Console.WriteLine("|");

                    }

                }


            }

            char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            string[,] tempboard = new string[8, 8];
            int movedx = 0,
                movedy = 0;
            string movedpiece = " ";
            bool control = false;


            Console.SetCursorPosition(5, 3);

            Console.Write("         CHESS BOARD  ");

            for (int b = 0; b < 8; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    boardarray[b, c] = ".";

                }
            }

            Console.WriteLine("\n");

            for (int d = 0; d < 8; d++)
            {
                Console.Write("   " + (8 - d) + "   ");
                for (int e = 0; e < 8; e++)
                    Console.Write(boardarray[d, e] + "   ");

                Console.WriteLine("\n");
            }

            for (int f = 3; f < 38; f++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(f, 4);
                Console.Write("-");
                Console.SetCursorPosition(f, 20);
                Console.Write("-");

            }

            for (int i = 3; i < 20; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(4, i);
                Console.Write("|");

                Console.SetCursorPosition(38, i);
                Console.Write("|");

            }
            Console.ResetColor();
            Console.SetCursorPosition(42, 18);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Your Turn");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(7, 21);
            boardarray[0, 0] = "Rb";
            boardarray[1, 0] = "Nb";
            boardarray[2, 0] = "Bb";
            boardarray[3, 0] = "Qb";
            boardarray[4, 0] = "Kb";
            boardarray[5, 0] = "Bb";
            boardarray[6, 0] = "Nb";
            boardarray[7, 0] = "Rb";
            boardarray[0, 7] = "Rr";
            boardarray[1, 7] = "Nr";
            boardarray[2, 7] = "Br";
            boardarray[3, 7] = "Qr";
            boardarray[4, 7] = "Kr";
            boardarray[5, 7] = "Br";
            boardarray[6, 7] = "Nr";
            boardarray[7, 7] = "Rr";
            Console.WriteLine("a" + "   " + "b" + "   " + "c" + "   " + "d" + "   " + "e" + "   " + "f" + "   " + "g" + "   " + "h");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(7, 5);
            Console.WriteLine("R" + "   " + "N" + "   " + "B" + "   " + "Q" + "   " + "K" + "   " + "B" + "   " + "N" + "   " + "R");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(7, 19);
            Console.WriteLine("R" + "   " + "N" + "   " + "B" + "   " + "Q" + "   " + "K" + "   " + "B" + "   " + "N" + "   " + "R");
            for (int i = 0;
            i < 8;
            i++) // array atama ve boarda takımı yazdırma // sol en üst 0 ıncı karekter diye başlıyor sol yukarıdan.
            {
                boardarray[i, 1] = "Pb";
                boardarray[i, 6] = "Pr";
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(7 + (i * 4), 7);
                if (i != 7) Console.WriteLine("P" + " " + " " + " ");
                else Console.WriteLine("P");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(7 + (i * 4), 17);
                if (i != 7) Console.WriteLine("P" + " " + " " + " ");
                else Console.WriteLine("P");

            }

            Console.SetCursorPosition(4, 30);
            Console.ResetColor();
            Console.WriteLine("How to play the game?");
            Console.WriteLine("You can use arrow keys to move around the board.");
            Console.WriteLine("Press space key to pick pieces.");
            Console.WriteLine("Press enter key to place the pieces. ");
            Console.WriteLine("You have to place the piece that you pick.");
            Console.WriteLine("If you want to play with entering string notations press N");
            Console.WriteLine("If you want to save the game press S");
            Console.WriteLine("Press H for hints");
            Console.WriteLine("For castling: pick the king and place it on the rook that you want.");

            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = true;
            ConsoleKeyInfo cki;
            int cursorx = 7,
            cursory = 5;
            int notationboardmx = 0,
            notationboardmy = 0,
            notationboardcx = 0,
            notationboardcy = 0;
            string movednotationp = " ";
            while (true)
            {
                if (Console.KeyAvailable)

                {
                    tempboard = boardarray;
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.D && demo_mode == true)
                    {
                        reading(turn);
                        turn = !turn;
                        turn_counter++;
                    }
                    if (cki.Key == ConsoleKey.H)
                    {
                        hint(boardarray);

                    }
                    if (cki.Key == ConsoleKey.N)
                    {
                        Console.SetCursorPosition(0, 22);
                        Console.WriteLine("Please enter move notation");
                        Console.SetCursorPosition(0, 24);
                        string move = Console.ReadLine();

                        // aşşağısı fonsiyon
                        strmove(boardarray, move, turn);
                        turn = !turn;
                        turn_counter++;

                    }
                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 35)
                    {
                        // key and boundary control
                        Console.SetCursorPosition(cursorx, cursory);
                        cursorx += 4;
                    }
                    if (cki.Key == ConsoleKey.LeftArrow && cursorx > 8)
                    {
                        Console.SetCursorPosition(cursorx, cursory);
                        cursorx -= 4;
                    }

                    if (cki.Key == ConsoleKey.UpArrow && cursory > 6)
                    {
                        Console.SetCursorPosition(cursorx, cursory);
                        cursory -= 2;
                    }

                    if (cki.Key == ConsoleKey.DownArrow && cursory < 18)
                    {
                        Console.SetCursorPosition(cursorx, cursory);
                        cursory += 2;
                    }
                    if (cki.Key == ConsoleKey.S)
                    {
                        StreamWriter f = File.CreateText("saved_game.txt");
                        filemoves_splitted = filemoves.Split("  ");
                        for (int i = 0; i < filemoves_splitted.Length; i++)
                        {
                            f.WriteLine(filemoves_splitted[i]);
                        }
                        f.Close();
                        Console.SetCursorPosition(6, 24);
                        Console.Write("Game's saved.");
                    }

                    int board_i = (cursorx - 7) / 4;
                    int board_j = (cursory - 5) / 2;
                    capture = false;
                    if (cki.Key == ConsoleKey.Spacebar && control == false && boardarray[board_i, board_j] != "." && ((boardarray[board_i, board_j][1] == 'r' && turn == false) || (boardarray[board_i, board_j][1] == 'b' && turn == true))) // ilk space basışta hareket edilecek taş seçiliyor. 
                    {
                        control = true; // burada oynatılmak istenen taşın tipini ve koordinatını kayıt altında tutuyorum. 

                        movedx = cursorx;
                        movedy = cursory;
                        movedpiece = boardarray[board_i, board_j];
                    }

                    if (cki.Key == ConsoleKey.Enter && control == true) // sonraki space basışta taş koyulacaksa yerine koyuluyor şu an piyonlar bir öne ve iki öne gidebiliyor.
                    {
                        control = false;
                        if (movedpiece[0] == 'P')
                        {
                            pawn(boardarray, movedx, movedy, cursorx, cursory, movedpiece);
                        }

                        if (((movedy - 5) / 2 == 4 || (movedy - 5) / 2 == 3) && movedpiece[0] == 'P' && (boardarray[board_i, board_j + 1][0] == 'P' && boardarray[board_i, board_j + 1][1] != movedpiece[1] || boardarray[board_i, board_j - 1][0] == 'P' && boardarray[board_i, board_j - 1][1] != movedpiece[1])) //en passant
                        {
                            if (movedpiece[1] == 'b' && red_pawns[board_i] == true)
                            {
                                boardarray = placer(boardarray, movedx, movedy, cursorx, cursory, movedpiece, turn_counter);
                                boardarray[board_i + 1, board_j] = ".";
                                Console.SetCursorPosition(cursorx, cursory - 2);
                                Console.Write(".");
                                turn_counter++;
                                turn = false;
                                enpassant_blue = true;
                                capture = true;
                            }
                            else if (movedpiece[1] == 'r' && blue_pawns[board_i] == true)
                            {
                                boardarray = placer(boardarray, movedx, movedy, cursorx, cursory, movedpiece, turn_counter);
                                boardarray[board_i + 1, board_j] = ".";
                                Console.SetCursorPosition(cursorx, cursory + 2);
                                Console.Write(".");
                                turn_counter++;
                                turn = false;
                                enpassant_red = true;
                                capture = true;
                            }
                        }

                        //şah
                        if (movedpiece[0] == 'K')
                        {
                            king(boardarray, movedx, movedy, cursorx, cursory, movedpiece);

                        }

                        if (movedpiece[0] == 'K' && boardarray[board_i, board_j][0] == 'R')
                        {

                            castling(boardarray, movedx, movedy, cursorx, cursory);
                        }

                        //rook
                        if (movedpiece[0] == 'R')
                        {
                            rook(boardarray, movedx, movedy, cursorx, cursory, movedpiece);
                        }

                        //bishop
                        if (movedpiece[0] == 'B')
                        {
                            bishop(boardarray, movedx, movedy, cursorx, cursory, movedpiece);
                        }

                        //vezir
                        if (movedpiece[0] == 'Q')
                        {
                            queen(boardarray, movedx, movedy, cursorx, cursory, movedpiece);
                        }

                        //at
                        if (movedpiece[0] == 'N')
                        {
                            knight(boardarray, movedx, movedy, cursorx, cursory, movedpiece);
                        }

                        if (turn == false)
                        {
                            Console.SetCursorPosition(42, 6);
                            Console.WriteLine("          ");
                            Console.SetCursorPosition(42, 18);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Your Turn");
                            Console.ForegroundColor = ConsoleColor.White;

                        }
                        else
                        {
                            Console.SetCursorPosition(42, 18);
                            Console.WriteLine("          ");
                            Console.SetCursorPosition(42, 6);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Your Turn");
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                        file_redmove = redmove;
                        file_bluemove = bluemove;
                        check_count = check_mate(boardarray, turn);
                        Notation(boardarray, movedx, movedy, cursorx, cursory, movedpiece, turn_counter);



                        capture = false;
                        gotpromoted = false;
                        file_redmove = "";
                        file_bluemove = "";
                        promoted = "";

                        if (mateblue == true)
                        {
                            check_mate(boardarray, false);
                        }
                        if (matered == true)
                        {
                            check_mate(boardarray, true);
                        }

                        if (check_count > 0 && mateblue == true && turn == true)
                        {
                            Console.SetCursorPosition(10, 25);
                            Console.WriteLine("GAME OVER BLUE WON");
                        }

                        if (check_count > 0 && turn == false && mateblue == false)
                        {
                            mateblue = true;
                        }
                        if (check_count > 0 && matered == true && turn == false)
                        {
                            Console.SetCursorPosition(10, 25);
                            Console.WriteLine("GAME OVER RED WON");
                        }

                        if (check_count > 0 && turn == true && matered == false)
                        {
                            matered = true;
                        }

                        if (check_count == 0)
                        {
                            mateblue = false;
                            matered = false;
                        }
                        check_count = 0;
                        bool redwon = true;
                        bool bluewon = true;
                        for (int i = 0; i < boardarray.GetLength(0); i++)
                        {
                            for (int i2 = 0; i2 < boardarray.GetLength(0); i2++)
                            {
                                if (boardarray[i, i2] == "Kr")
                                    redwon = false;
                                if (boardarray[i, i2] == "Kb")
                                    bluewon = false;
                            }

                        }
                        if (bluewon == true)
                        {
                            Console.SetCursorPosition(10, 25);
                            Console.WriteLine("GAME OVER RED WON");
                            break;
                        }
                        if (redwon == true)
                        {
                            Console.SetCursorPosition(10, 25);
                            Console.WriteLine("GAME OVER BLUE WON");
                            break;
                        }



                    }

                    Console.SetCursorPosition(cursorx, cursory); // refresh 
                }

            }

            Console.ReadLine();
        }

    }
}
