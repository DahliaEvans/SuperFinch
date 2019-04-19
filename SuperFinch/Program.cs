using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace SuperFinch
{

    //***********************
    // Title: Super Finch
    // Application Type: Console
    // Author: Dahlia Evans
    // Date Created: 4/14/2019
    // Last Modified: 4/14/2019
    // Level 3
    // **********************

    class Program
    {
        static Finch Sam = new Finch();
        
        public enum FinchShields
        {
            HIGH,
            MEDIUM,
            LOW,
            FAILED
        }

        static double ambientTemperature;

        static void Main(string[] args)
        {
            Sam.connect();
            DisplayWelcomeScreen();
            FinchShields shieldLevel = GetUserShieldLevel();
            int speed = 0;
            ambientTemperature = GetTempature();

            switch (shieldLevel)
            {
                case FinchShields.HIGH:
                    speed = 100;
                    break;
                case FinchShields.MEDIUM:
                    speed = 75;
                    break;
                case FinchShields.LOW:
                    speed = 50;
                    break;
                default:
                    break;
            }

            DisplayContinuePrompt();
            Console.Clear();
            MoveSamForward(speed);
            Console.WriteLine("Super Finch is marching forward into battle!");

            do
            {
                switch (shieldLevel)
                {
                    case FinchShields.HIGH:
                        Sam.setLED(0, 255, 0);
                        break;
                    case FinchShields.MEDIUM:
                        Sam.setLED(0, 0, 255);
                        break;
                    case FinchShields.LOW:
                        Sam.setLED(255, 0, 0);
                        break;
                    default:
                        break;
                }

                shieldLevel = HitDetection(shieldLevel);
            } while (shieldLevel != FinchShields.FAILED);

            Sam.setMotors(0, 0);
            Console.WriteLine("Game Over!");
            DisplayContinuePrompt();
        }

        static void Hit()
        {
            Console.WriteLine("Hit!");
            for (int i = 0; i < 3; i++)
            {
                Sam.setLED(255, 255, 255);
                Sam.noteOn(500);
                Sam.wait(500);
                Sam.setLED(0, 0, 0);
                Sam.noteOff();
                Sam.wait(100);
            }
        }

        static FinchShields HitDetection(FinchShields shieldLevel)
        {
            const int laserLevel = 100;
            double freezeTemp = ambientTemperature - 0.5;

            switch (shieldLevel)
            {
                case FinchShields.HIGH:
                    if (GetAverageLight() >= laserLevel && GetTempature() <= freezeTemp)
                    {
                        Hit();
                        shieldLevel = FinchShields.MEDIUM;
                        Console.WriteLine("Shields at MEDIUM level.");

                    }
                    break;
                case FinchShields.MEDIUM:
                    if (GetAverageLight() >= laserLevel)
                    {
                        Hit();
                        shieldLevel = FinchShields.LOW;
                        Console.WriteLine("Shields at LOW level.");
                    }
                    break;
                case FinchShields.LOW:
                    if (GetAverageLight() >= laserLevel || GetTempature() <= freezeTemp)
                    {
                        Hit();
                        shieldLevel = FinchShields.FAILED;
                        Console.WriteLine("Shields at FAILED level.");
                    }
                    break;
                case FinchShields.FAILED:
                    break;
                default:
                    break;
            }
            return shieldLevel;
        }


        static double GetAverageLight()
        {
            double leftLightSensor = Sam.getLeftLightSensor();
            double rightLightSensor = Sam.getRightLightSensor();
            double averageLight = (leftLightSensor + rightLightSensor) / 2;

            return averageLight;
        }

        static double GetTempature()
        {
            double temperature = Sam.getTemperature();
            
            return temperature;
        }

        static void MoveSamForward(int speed)
        {
            Sam.setMotors(speed, speed);
        }

        static FinchShields GetUserShieldLevel()
        {
            string userShieldLevelString;
            FinchShields shieldLevel = 0;
            bool validInput = false;

            do
            {
                userShieldLevelString = Console.ReadLine().ToUpper();

                switch (userShieldLevelString)
                {
                    case "HIGH":
                        shieldLevel = FinchShields.HIGH;
                        validInput = true;
                        break;

                    case "MEDIUM":
                        shieldLevel = FinchShields.MEDIUM;
                        validInput = true;
                        break;

                    case "LOW":
                        shieldLevel = FinchShields.LOW;
                        validInput = true;
                        break;

                    default:
                        Console.WriteLine("Invalid input! Please enter HIGH, MEDIUM, or LOW.");
                        break;
                }
            } while (!validInput);

            return shieldLevel;

        }

        static void DisplayWelcomeScreen()
        {
            Console.WriteLine("Welcome to Super Finch!");
            Console.WriteLine("SuperFinch, the superhero robot, is going into battle.");
            Console.WriteLine("She will encounter both lasers and freeze rays.");
            Console.WriteLine();
            Console.WriteLine("Please enter the shield level for Super Finch.");
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
