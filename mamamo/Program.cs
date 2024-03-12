using System;
using System.ComponentModel.Design;
using System.Threading;

class HotelUnta
{
    static Room[] availableRooms = new Room[]
    {
        new Room("Double Standard", 3),
        new Room("Double Deluxe", 3),
        new Room("Twin Bed", 3),
        new Room("Superior Deluxe", 3)
    };

    static Reservation[] reservations = new Reservation[100]; // limit max - 100
    //para ni ma check kung ok pa ba mag add ug reservation, di pareha nimo na wa gi check kung ok pa
    static int reservationCount = 0;

    static string customerName, mobileNumber, roomType; // mobile number into data type int > make a try parse for its input

    static void Main(string[] args)
    {
        display("Log in........");

        while (true)
        {
            Console.WriteLine("\nWelcome to the Hotel Reservation System");
            Console.WriteLine("1. Create Reservation");
            Console.WriteLine("2. Search for Availability");
            Console.WriteLine("3. Display Room Details");
            Console.WriteLine("5. Cancel Reservation");
            Console.WriteLine("6. Check Price Calculation");
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
                    logOff();
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
                    CalculateReservationPrice();
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
        string customerName = Console.ReadLine();

        Console.Write("Enter Mobile Number: ");
        string mobileNumber = Console.ReadLine();

        string roomTypeInput;
        bool validRoomType = false;
        do
        {
            Console.Write("Enter Room Type (Twin Bed, Superior Deluxe, Double Standard, Double Deluxe): ");
            roomTypeInput = Console.ReadLine().Trim(); // Trim to remove leading/trailing whitespaces

            // Check if the input matches any of the available room types
            if (roomTypeInput.Equals("Twin Bed", StringComparison.OrdinalIgnoreCase) ||
                roomTypeInput.Equals("Double Standard", StringComparison.OrdinalIgnoreCase) ||
                roomTypeInput.Equals("Superior Deluxe", StringComparison.OrdinalIgnoreCase) ||
                roomTypeInput.Equals("Double Deluxe", StringComparison.OrdinalIgnoreCase))
            {
                validRoomType = true;
            }
            else
            {
                Console.WriteLine("Invalid room type, please provide a valid type.");
            }
        } while (!validRoomType);

        Room room = null;
        foreach (var availableRoom in availableRooms)
        {
            // Compare room types ignoring case and check availability
            if (availableRoom.Type.Equals(roomTypeInput, StringComparison.OrdinalIgnoreCase) && availableRoom.Availability > 0)
            {
                room = availableRoom;
                break;
            }
        }

        if (room != null)
        {
            Console.Write("Enter Check-in Date (MM/dd/yyyy): ");
            DateTime checkIn;
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out checkIn))
            {
                Console.WriteLine("Invalid date format. Please enter the date in MM/dd/yyyy format.");
                Console.Write("Enter Check-in Date (MM/dd/yyyy): ");
            }

            Console.Write("Enter Check-out Date (MM/dd/yyyy): ");
            DateTime checkOut;
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out checkOut) || checkOut <= checkIn)
            {
                Console.WriteLine("Invalid date or check-out date must be after check-in date. Please enter a valid check-out date.");
                Console.Write("Enter Check-out Date (MM/dd/yyyy): ");
            }

            Reservation newReservation = new Reservation(customerName, mobileNumber, roomTypeInput, checkIn, checkOut);

            reservations[reservationCount++] = newReservation;

            room.Availability--;
            
            Console.WriteLine("Reservation created successfully!");
            Thread.Sleep(3000); //para naay time mabasa ang console message before ma clear
            Console.Clear();
        }
        else
        {
            // Display message if room type is not available
            Console.WriteLine("Sorry, there are no available rooms for the room type " + roomTypeInput + ". Reservation cannot be made.");
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

    static void logOff()
    {
        Console.SetCursorPosition(0, 0);
        Console.Clear();
        string letters = "Logging Off.....";
        for (int i = 0; i < letters.Length; i++)
        {
            for (int j = 1; j < 3; j++)
            {
                Console.SetCursorPosition(5 + i, j);
                Console.Write(letters[i]);
                Console.SetCursorPosition(5 + i, j - 1);
                Console.Write(' ');
                System.Threading.Thread.Sleep(200);

            }
        }
        Environment.Exit(0);
    }

    static void CalculateReservationPrice()
    {
        Console.WriteLine("\nCalculating Reservation Price...");

        Console.Write("Enter Customer Name to calculate reservation price: ");
        string customerName = Console.ReadLine();

        // Find the reservation based on customer name
        Reservation reservation = null;
        foreach (var res in reservations)
        {
            if (res != null && res.CustomerName.Equals(customerName, StringComparison.OrdinalIgnoreCase))
            {
                reservation = res;
                break;
            }
        }

        if (reservation != null)
        {
            // Find the corresponding room type
            Room room = null;
            foreach (var availableRoom in availableRooms)
            {
                if (availableRoom.Type.Equals(reservation.RoomType, StringComparison.OrdinalIgnoreCase))
                {
                    room = availableRoom;
                    break;
                }
            }

            // Calculate duration and price
            TimeSpan duration = reservation.CheckOut - reservation.CheckIn;
            double price = duration.TotalDays * GetPriceByRoomType(reservation.RoomType);

            Console.WriteLine($"Total price for the reservation of {reservation.RoomType} for {duration.TotalDays} days is: {price} PHP");
        }
        else
        {
            Console.WriteLine("No reservation found for customer: " + customerName);
        }
    }

    static double GetPriceByRoomType(string roomType)
    {
        switch (roomType.ToLower())
        {
            case "double standard":
                return 1200;
            case "double deluxe":
                return 1500;
            case "twin bed":
                return 1750;
            case "superior deluxe":
                return 2500;
            default:
                return 0;
        }
    }



    static void display(string text) //Style Naay logging in animation ang program :3
    {
        Console.SetCursorPosition(0, 0);
        var letters = text;
        for (int i = 0; i < letters.Length; i++)
        {
            for (int j = 1; j < 2; j++)
            {
                Console.SetCursorPosition(5 + i, j);
                Console.Write(letters[i]);
                Console.SetCursorPosition(5 + i, j - 1);
                Console.Write(' ');
                Thread.Sleep(100);
            }
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


