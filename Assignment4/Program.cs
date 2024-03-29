using System;
using System.Collections.Generic;

public enum SeatLabel
{
    A,
    B,
    C,
    D
}

public class Seat
{
    public bool IsBooked { get; set; }
    public Passenger Passenger { get; set; }
    public SeatLabel Label { get; set; }
    public int Row { get; set; }

    public Seat(int row, SeatLabel label)
    {
        IsBooked = false;
        Row = row;
        Label = label;
    }

    public override string ToString()
    {
        if (IsBooked)
            return $"{Passenger.Initials}";
        else
            return $"{Label}";
    }
}

public class Passenger
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public SeatLabel SeatPreference { get; set; }
    public Seat Seat { get; set; }
    public string Initials => $"{FirstName[0]}{LastName[0]}";

    public Passenger(string firstName, string lastName, SeatLabel seatPreference)
    {
        FirstName = firstName;
        LastName = lastName;
        SeatPreference = seatPreference;
    }
}

public class Plane
{
    public List<Seat> Seats { get; set; }

    public Plane(int numRows)
    {
        Seats = new List<Seat>();
        for (int i = 1; i <= numRows; i++)
        {
            foreach (SeatLabel label in Enum.GetValues(typeof(SeatLabel)))
            {
                Seats.Add(new Seat(i, label));
            }
        }
    }

    public Seat FindAvailableSeat(SeatLabel preference)
    {
        foreach (var seat in Seats)
        {
            if (!seat.IsBooked && (preference == SeatLabel.A || preference == SeatLabel.D) && (seat.Label == SeatLabel.A || seat.Label == SeatLabel.D))
            {
                return seat;
            }
            else if (!seat.IsBooked && (preference == SeatLabel.B || preference == SeatLabel.C) && (seat.Label == SeatLabel.B || seat.Label == SeatLabel.C))
            {
                return seat;
            }
        }
        return null;
    }
}

class Program
{
    static Plane plane = new Plane(12);

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Please enter 1 to book a ticket.");
            Console.WriteLine("Please enter 2 to see seating chart.");
            Console.WriteLine("Please enter 3 to exit the application.");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    BookTicket();
                    break;
                case "2":
                    DisplaySeatingChart();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                    break;
            }
        }
    }

    static void BookTicket()
    {
        Console.WriteLine("Please enter the passenger's first name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Please enter the passenger's last name:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Please enter 1 for a Window seat preference, 2 for an Aisle seat preference, or hit enter to pick first available seat:");
        string preferenceInput = Console.ReadLine();
        SeatLabel preference = SeatLabel.A; // Default preference
        if (!string.IsNullOrEmpty(preferenceInput))
        {
            int preferenceIndex = int.Parse(preferenceInput);
            preference = (SeatLabel)(preferenceIndex - 1);
        }

        Seat availableSeat = plane.FindAvailableSeat(preference);

        if (availableSeat != null)
        {
            availableSeat.IsBooked = true;
            Passenger passenger = new Passenger(firstName, lastName, preference);
            passenger.Seat = availableSeat;
            availableSeat.Passenger = passenger;
            Console.WriteLine($"The seat located in {availableSeat.Row} {availableSeat.Label} has been booked.");
        }
        else
        {
            Console.WriteLine("Sorry, the plane is fully booked.");
        }
    }

    static void DisplaySeatingChart()
    {
        foreach (var seat in plane.Seats)
        {
            Console.Write(seat.ToString() + " ");
            if (seat.Label == SeatLabel.D)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
    }
}
