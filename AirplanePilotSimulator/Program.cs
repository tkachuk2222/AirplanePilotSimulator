using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace AirplanePilotSimulator
{

    class Program
    {
        delegate int FlyingSpeed();

        public class AircraftCrashed : Exception
        {
            public AircraftCrashed() { }
            public AircraftCrashed(string str):base(str) { }

        }

        public class UnsuitableToFlights : Exception
        {
            public UnsuitableToFlights() { }
            public UnsuitableToFlights(string str):base(str) { }
        }

        class Airplane
        {
            public Airplane() { _pHeight = 0; _pSpeed = 0; }
            //
            //add new dispetcher
            public void setDispatcher(string name)
            {
                if (_pDispetchers == null)
                    _pDispetchers = new List<Dispatcher>();
                _pDispetchers.Add(new Dispatcher(name));
            }

            //
            // if dispetchers > 2 then can fly
            public bool canFly()
            {
                if (_pDispetchers.Count < 2)
                    return false;
                return true;
            }


            //
            // information from dispetchers
            public void infoFromDisp(int s)
            {
                foreach (var disp in _pDispetchers)
                {
                    Console.WriteLine($"recomended height: ____ {disp.recommendedHeight(s)}");
                    try
                    {
                        disp.PenaltyPoints(disp.recommendedHeight(s), _pHeight, _pSpeed);
                    }
                    catch (AircraftCrashed ex)
                    {
                        Console.WriteLine(ex);
                    }
                    catch (UnsuitableToFlights ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }


            //
            // adding speed and height in time flying
            public void flying(int speed, int height)
            {
                if (canFly() == true)
                {
                    _pSpeed += speed;
                    _pHeight += height;

                    if (_pSpeed < 0)
                        _pSpeed = 0;
                    if (_pHeight < 0)
                        _pHeight = 0;
                    Console.WriteLine($"{_pSpeed}\t\t{_pHeight}");
                    infoFromDisp(getSpeed());
                }
                else
                {
                    Console.WriteLine("Please, add more then 1 dispetcher");
                }
            }
            public int getSpeed() {
                return _pSpeed;
            }

            //
            // show penalty points
            public int getPenPoint()
            {
                int sum = 0;
                foreach (var el in _pDispetchers)
                    sum += el.PenPoints;
                return sum;
            }

            //
            // list of dispetchers
            private List<Dispatcher> _pDispetchers;

            // speed and height of airplane
            private int _pSpeed;
            private int _pHeight;
        }

        class Dispatcher
        {
            public Dispatcher(string name)
            {
                Name = name;
                PenPoints = 0;
            }
            protected string Name { set; get; }

            // correcting weather(random)
            public int Weather()
            {
                int rand = ((new Random()).Next(-200, 200));
                return rand;
            }

            //
            // return recomended height
            public int recommendedHeight(int speed)
            {
                if (speed > 50)
                {
                    int result = 7 * speed - Weather();
                    return result;
                }
                return 0;
            }


            public void PenaltyPoints(int rec, int height, int speed)
            {
                int difference = height - rec;
                if(speed > 1000)
                {
                    PenPoints += 100;
                    Console.WriteLine("Immediately reduce the speed!!");
                }

                if (difference > 300 && difference < 600)
                {
                    PenPoints += 25;
                }
                else if (difference > 600 && difference < 1000)
                {
                    PenPoints += 50;
                }

                else if (difference > 1000)
                    throw new AircraftCrashed("Aircraft is Crashed");
                else if (PenPoints >= 1000)
                    throw new UnsuitableToFlights("Unsuitable To Flights!");
            }

            //
            // penalty points
            public int PenPoints { set; get; }
        }

        static void Main(string[] args)
        {


            Airplane plane = new Airplane();


            
            // flsp.Invoke();


            while (true)
            {
                Console.WriteLine("1.Start fly\n2.Add dispetcher\n3.Show result");
                int m = int.Parse(Console.ReadLine());



                switch (m)
                {
                    case 1:
                        {
                            ConsoleKeyInfo cki;
                            Console.TreatControlCAsInput = true;

                            do
                            {

                                //Console.Clear();
                                Console.WriteLine("Press Right: +50km\\h, Left: –50km\\h, Shift-Right: +150km\\h, Shift - Left: –150km\\h");
                                Console.WriteLine("Press Up: +250 m, Down: –250 m, Shift-Up: +500 m, Shift-Down: –500m).");
                                Console.WriteLine("To exit press Escape");
                                cki = Console.ReadKey();
                              
                                    
                                    //
                                    // height
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) != 0) && cki.Key == ConsoleKey.UpArrow) plane.flying(0, 500);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) != 0) && cki.Key == ConsoleKey.DownArrow) plane.flying(0, -500);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) == 0) && cki.Key == ConsoleKey.UpArrow) plane.flying(0, 250);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) == 0) && cki.Key == ConsoleKey.DownArrow) plane.flying(0, -250);

                                    //
                                    // speed
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) != 0) && cki.Key == ConsoleKey.RightArrow) plane.flying(150, 0);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) != 0) && cki.Key == ConsoleKey.LeftArrow) plane.flying(-150, 0);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) == 0) && cki.Key == ConsoleKey.RightArrow) plane.flying(50, 0);
                                    if (((cki.Modifiers & ConsoleModifiers.Shift) == 0) && cki.Key == ConsoleKey.LeftArrow) plane.flying(-50, 0);
                               
                                
                            } while (cki.Key != ConsoleKey.Escape);


                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Input name of dispetcher");
                            plane.setDispatcher(Console.ReadLine());
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine($"Penalty points: \t {plane.getPenPoint()}");
                            break;
                        }
                }
            }
        }
    }
}
