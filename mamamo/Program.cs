﻿using System;
using System.ComponentModel.Design;


class HotelUnta
{
    static Room[] availableRooms = new Room[]
    {
        new Room("Double Standard", 3),
        new Room("Double Deluxe", 3),
        new Room("Twin Bed", 3),
        new Room("Superior Deluxe", 3)
    };

    static Reservation[] reservations = new Reservation[100];
    //para ni ma check kung ok pa ba mag add ug reservation, di pareha nimo na wa gi check kung ok pa
    static int reservationCount = 0;

    static string customerName, mobileNumber, roomType;
    static void Main(string[] args)
    {
       

        while (true)
        {
            Console.WriteLine("\nWelcome to the Hotel Reservation System");
            Console.WriteLine("1. Create Reservation");
            Console.WriteLine("2. Search for Availability");
            Console.WriteLine("3. Display Room Details");
            Console.WriteLine("5. Cancel Reservation");
            Console.WriteLine("6. Modify Reservation");
            Console.WriteLine("0. Exit");
            Console.WriteLine("Please enter your choice:");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Exiting program. Goodbye!");
                    return;
                case 1:
                    CreateReservation();
                    break;
                case 2:
                    SearchForAvailability();
                    break;
                case 3:
                    DisplayRoomDetails();
                    break;
                case 5:
                    CancelReservation();
                    break;
                case 6:
                    ModifyReservation();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
    }

    static void CreateReservation()
    {
        Console.WriteLine("\nCreating a new reservation...");

        Console.Write("Enter Customer Name: ");
        customerName = Console.ReadLine();

        Console.Write("Enter Mobile Number: ");
        mobileNumber = Console.ReadLine();

        Console.Write("Enter Room Type: ");
        roomType = Console.ReadLine();

        Room room = null;
        foreach (var availableRoom in availableRooms)
        {
            if (availableRoom.Type == roomType && availableRoom.Availability > 0)
            {
                room = availableRoom;
                break;
            }
        }

        if (room != null)
        {
            Console.Write("Enter Check-in Date (MM/dd/yyyy): ");
            DateTime checkIn;
            //System.Globalization.DateTimeStyles.None same ni sa kadtong birthyear na activity ni sir
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out checkIn))
            {
                Console.WriteLine("Invalid date format. Please enter the date in MM/dd/yyyy format.");
                Console.Write("Enter Check-in Date (MM/dd/yyyy): ");
            }

            Console.Write("Enter Check-out Date (MM/dd/yyyy): ");
            DateTime checkOut;
            //kani sad
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out checkOut) || checkOut <= checkIn)
            {
                Console.WriteLine("Invalid date or check-out date must be after check-in date. Please enter a valid check-out date.");
                Console.Write("Enter Check-out Date (MM/dd/yyyy): ");
            }

            Reservation newReservation = new Reservation(customerName, mobileNumber, roomType, checkIn, checkOut);

            reservations[reservationCount++] = newReservation;

            room.Availability--;

            Console.WriteLine("Reservation created successfully!");
        }
        else
        {
            Console.WriteLine("Sorry, there are no available rooms for the room type " + roomType +". Reservation cannot be made.");
        }
    }

    static void SearchForAvailability()
    {
        Console.WriteLine("Room Availability:");
        foreach (var room in availableRooms)
        {
            Console.WriteLine("Room Type: " + room.Type + ", Available Rooms: " + room.Availability);
        }
    }

    static void DisplayRoomDetails()
    {
        Console.WriteLine("\nDisplaying Room Types and Rates:");
        Console.WriteLine("\nDouble Standard - 1200 PHP");
        Console.WriteLine("Double Deluxe - 1500 PHP");
        Console.WriteLine("Twin Bed - 1800 PHP");
        Console.WriteLine("Superior Deluxe - 2500 PHP");
    }

    static void CancelReservation()
    {
        Console.WriteLine("\nCanceling Reservation...");
        Console.Write("Enter Customer Name to cancel reservation: ");
        string cancelCustomerName = Console.ReadLine();
        //para pa check kung naa paka sa iya heart
        bool reservationFound = false;

        for (int i = 0; i < reservationCount; i++)
        {
            if (reservations[i] != null && reservations[i].CustomerName.Equals(cancelCustomerName, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var availableRoom in availableRooms)
                {
                    if (availableRoom.Type == reservations[i].RoomType)
                    {
                        availableRoom.Availability++;
                        break;
                    }
                }

                reservations[i] = null;

                Console.WriteLine("Reservation canceled successfully for customer: " + cancelCustomerName);
                reservationFound = true;
                break;
            }
        }
        if (!reservationFound)
        {
            Console.WriteLine("No reservation found for customer: " + cancelCustomerName);
        }
    }


    static void ModifyReservation() //basura
    {
        Console.WriteLine("What do you wish to modify from your reservation?");
        Console.WriteLine("1 : Customer Name");
        Console.WriteLine("2 : Mobile Number");
        Console.WriteLine("3 : Room Type");
        Console.WriteLine("4 : Check in Time");
        Console.WriteLine("5 : Check out Time");
        int choice;
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.Write("Invalid input. Please enter a number.");
        }

        if (choice == 1)
        {
            Console.Write("Enter Customer Name: ");
            customerName = Console.ReadLine();
        }
        else if(choice == 2)
        {
            Console.Write("Enter Mobile Number: ");
            mobileNumber = Console.ReadLine();
        }
        else if(choice == 3)
        {
            Console.Write("Enter Room Type");
            roomType = Console.ReadLine();
            Room room = null;
            foreach (var availableRoom in availableRooms)
            {
                if (availableRoom.Type == roomType && availableRoom.Availability > 0)
                {
                    room = availableRoom;
                    break;
                }
            }
        }
        else if(choice == 4)
        {
            Console.Write("Enter Check-in Date (MM/dd/yyyy): ");

        }
    }
}

// tanan naa diri para na sa OOP approach
class Room
{
    public string Type;
    public int Availability;

    public Room(string type, int availability)
    {
        Type = type;
        Availability = availability;
    }
}

class Reservation
{
    public string CustomerName;
    public string MobileNumber;
    public string RoomType;
    public DateTime CheckIn;
    public DateTime CheckOut;

    public Reservation(string customerName, string mobileNumber, string roomType, DateTime checkIn, DateTime checkOut)
    {
        CustomerName = customerName;
        MobileNumber = mobileNumber;
        RoomType = roomType;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}
