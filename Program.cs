﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace conwaysgamesoflife
{
    class Program
    {
        private static readonly int WIELKOSC = 20;
        private static readonly int MILISEKUNDY_CZEKANIA = 1000;
        private static readonly char ZYWY = (char)164;
        private static readonly char MARTWY = ' ';

        static void Main(string[] args)
        {
            char[,] plansza = new char[WIELKOSC, WIELKOSC];
            List<Vector2D> umierajace = new List<Vector2D>();
            List<Vector2D> ozywajace = new List<Vector2D>();
            PrzypisanieDoPlanszy(plansza);
            do
            {
                PokazaniePlanszy(plansza);
                SprawdzenieKlockow(plansza, ref umierajace, ref ozywajace);
                UsuwanieAlboDodawanieKloskow(plansza, ref umierajace, ref ozywajace);
                Thread.Sleep(MILISEKUNDY_CZEKANIA);
                Console.Clear();
            } while (CzyCosJeszczeZyje(plansza));
        }

        static void PrzypisanieDoPlanszy(char[,] _plansza)
        {
            Przypisanie_Reczne(_plansza);
            //Przypisanie_Losowe(_plansza);
            //Przypisanie_WszystkoZyje(_plansza);
        }

        static void Przypisanie_Losowe(char[,] _plansza)
        {
            Random rnd = new Random();
            for (int i = 0; i < WIELKOSC; i++)
                for (int j = 0; j < WIELKOSC; j++)
                    _ = rnd.Next(100) % 5 == 0 ? _plansza[i, j] = ZYWY : _plansza[i, j] = MARTWY;
        }

        static void Przypisanie_WszystkoZyje(char[,] _plansza)
        {
            for (int i = 0; i < WIELKOSC; i++)
                for (int j = 0; j < WIELKOSC; j++)
                    _plansza[i, j] = ZYWY;
        }

        static void Przypisanie_Reczne(char[,] _plansza)
        {
            // Ustawia wszystko na martwe
            for (int i = 0; i < WIELKOSC; i++)
                for (int j = 0; j < WIELKOSC; j++)
                    _plansza[i, j] = MARTWY;
            int temp = 2; // Offset od lewego gornego rogu

            // Wyglad Krztalu 5x5 
            char[,] wzorek = new char[,] {
                { ZYWY, ' ', ' ', ZYWY, ' '},
                { ' ', ' ', ' ', ' ', ZYWY},
                { ZYWY, ' ', ' ', ' ', ZYWY},
                { ' ', ZYWY, ZYWY, ZYWY, ZYWY},
                { ' ', ' ', ' ', ' ', ' '}
            };

            // temp + (liczba) liczba to wielkosc krztalu
            // Dodaje wzorek na plansze mapy z offsetem z zmiennej temp
            for (int i = temp; i < temp + 5; i++)
            {
                for (int j = temp; j < temp + 5; j++)
                {
                    _plansza[i, j] = wzorek[i - temp, j - temp];
                }
            }
        }

        static void SprawdzenieKlockow(char[,] _plansza, ref List<Vector2D> _umierajace, ref List<Vector2D> _ozywiajace)
        {
            for (int i = 0; i < WIELKOSC; i++)
            {
                for (int j = 0; j < WIELKOSC; j++)
                {
                    // --------------------------------
                    int iloscZywych = 0;
                    for (int x = i - 1; x < i + 2; x++)
                    {
                        for (int y = j - 1; y < j + 2; y++)
                        {
                            if (x < 0 || x >= WIELKOSC || y < 0 || y >= WIELKOSC)
                                continue;
                            if (x == 0 + i && y == 0 + j)
                                continue;
                            if (_plansza[x, y] == ZYWY)
                                iloscZywych++;
                        }
                    }

                    if (_plansza[i, j] == ZYWY)
                    {
                        if (iloscZywych < 2 || iloscZywych > 3)
                            _umierajace.Add(new Vector2D(i, j));
                    }
                    else
                    {
                        if (iloscZywych == 3)
                            _ozywiajace.Add(new Vector2D(i, j));
                    }
                }
            }
        }

        static void UsuwanieAlboDodawanieKloskow(char[,] _plansza, ref List<Vector2D> _umierajace, ref List<Vector2D> _ozywiajace)
        {
            foreach (var klocek in _umierajace)
            {
                _plansza[klocek.x, klocek.y] = MARTWY;
            }

            foreach (var klocek in _ozywiajace)
            {
                _plansza[klocek.x, klocek.y] = ZYWY;
            }

            _umierajace = new List<Vector2D>();
            _ozywiajace = new List<Vector2D>();
        }

        static bool CzyCosJeszczeZyje(char[,] _plansza)
        {
            for (int i = 0; i < WIELKOSC; i++)
                for (int j = 0; j < WIELKOSC; j++)
                    if (_plansza[i, j] == ZYWY)
                        return true;

            return false;
        }

        static void PokazaniePlanszy(char[,] _plansza)
        {
            for (int i = 0; i < WIELKOSC; i++)
            {
                for (int j = 0; j < WIELKOSC; j++)
                {
                    Console.Write(_plansza[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class Vector2D
    {
        public int x;
        public int y;

        public Vector2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}