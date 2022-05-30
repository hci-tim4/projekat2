﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using railway.model;

namespace railway.database
{
    public class FillDatabase
    {
        public void fill()
        {
            using (var db = new RailwayContext())
            {
                db.Database.ExecuteSqlCommand("Insert into Users Values('Mika', 'Mikic', 1, 'miki', '123')");
                db.Database.ExecuteSqlCommand("Insert into Users Values('Jovan', 'Jokic', 1, 'joca', '123')");
                db.Database.ExecuteSqlCommand("Insert into Users Values('Đorđe', 'Simić', 1, 'đole', '123')");

                db.Database.ExecuteSqlCommand("Insert into SeatTypes Values('VIP', 200)");
                db.Database.ExecuteSqlCommand("Insert into SeatTypes Values('Biznis klasa', 120)");
                db.Database.ExecuteSqlCommand("Insert into SeatTypes Values('Regularan', 80)");

                db.Database.ExecuteSqlCommand("Insert into Trains Values('Sova', 'bela')");
                db.Database.ExecuteSqlCommand("Insert into Trains Values('Orao', 'crn')");
                db.Database.ExecuteSqlCommand("Insert into Trains Values('Soko', 'žuta')");

                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 1, 1)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 1, 2)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 1, 3)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 1, 4)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 1, 5)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 2, 1)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 2, 2)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 2, 3)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 2, 4)");
                db.Database.ExecuteSqlCommand("Insert into Seats (SeatTypeId, TrainId, Row, Col) Values(1, 1, 2, 5)");

                db.Database.ExecuteSqlCommand("Insert into Stations Values('Novi Sad', 45.25500000, 19.84472222)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Subotica', 46.10000000, 19.66361111)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Šid', 48.27472222, 19.88000000)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Beograd', 44.81583333, 20.45944444)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Zrenjanin', 45.38222222, 20.39055556)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Šabac', 44.75583333, 19.69416667)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kraljevo', 43.71666667, 20.68333333)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kruševac', 43.58250000, 21.32666667)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kragujevac', 44.01416667, 20.91166667)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Valjevo', 44.27416667, 19.89111111)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Sremska Mitrovica', 44.97222222, 19.60916667)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Sombor', 45.77361111, 19.11333333)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kikinda', 45.82583333, 20.46111111)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Bor', 44.07583333, 22.09888889)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Negotin', 44.22944444, 22.53111111)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kanjiža', 46.06222222, 20.05083333)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Kosovska Mitrovica', 42.89027778, 20.86638889)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Sjenica', 43.27055556, 19.99305556)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Niš', 43.32083333, 21.89583333)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Užice', 43.85583333, 19.84111111)");
                db.Database.ExecuteSqlCommand("Insert into Stations Values('Priboj', 43.58138889, 19.52638889)");

                db.Database.ExecuteSqlCommand("Insert into DrivingLines (Name, TrainId, deleted, startDate) Values('d1', 1, 0,'05/01/2022')");
                db.Database.ExecuteSqlCommand("Insert into DrivingLines (Name, TrainId, deleted, startDate) Values('d2', 1, 0,'05/01/2022')");
                db.Database.ExecuteSqlCommand("Insert into DrivingLines (Name, TrainId, deleted, startDate) Values('d3', 1, 0,'05/01/2022')");
                db.Database.ExecuteSqlCommand("Insert into DrivingLines (Name, TrainId, deleted, startDate) Values('d4', 1, 0,'05/01/2022')");
                //
                //With driving line
                ///*
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(1, 1, '17:00:00.00', '17:10:00.00', 1,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(11, 2, '17:20:00.00', '17:25:00.00', 1,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(4, 3, '17:30:00.00', '17:35:00.00', 1,0)"); 

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(20, 1, '16:00:00.00', '16:10:00.00', 2,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(10, 2, '16:20:00.00', '16:25:00.00', 2,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(4, 3, '16:30:00.00', '16:35:00.00', 2,0)"); 

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(4, 1, '11:00:00.00', '11:10:00.00', 3,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(1, 2, '11:20:00.00', '11:25:00.00', 3,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(12, 3, '11:30:00.00', '11:35:00.00', 3,0)"); 
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(2, 4, '11:50:00.00', '11:55:00.00', 3,0)"); 

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(5, 1, '11:00:00.00', '11:10:00.00', 4,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(1, 2, '11:20:00.00', '11:25:00.00', 4,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(4, 3, '11:30:00.00', '11:35:00.00', 4,0)"); 
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(8, 4, '11:50:00.00', '11:55:00.00', 4,0)");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime, DrivingLineId, deleted) Values" +
                    "(19, 5, '12:00:00.00', '12:55:00.00', 4,0)"); 
                //*/
                /*
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(1, 1, '17:00:00.00', '17:10:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(11, 2, '17:20:00.00', '17:25:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(4, 3, '17:30:00.00', '17:35:00.00')");

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(20, 1, '16:00:00.00', '16:10:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(10, 2, '16:20:00.00', '16:25:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(4, 3, '16:30:00.00', '16:35:00.00')");

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(4, 1, '11:00:00.00', '11:10:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(1, 2, '11:20:00.00', '11:25:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(12, 3, '11:30:00.00', '11:35:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(2, 4, '11:50:00.00', '11:55:00.00')");

                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(5, 1, '11:00:00.00', '11:10:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(1, 2, '11:20:00.00', '11:25:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(4, 3, '11:30:00.00', '11:35:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(8, 4, '11:50:00.00', '11:55:00.00')");
                db.Database.ExecuteSqlCommand("Insert into StationSchedules (StationId, SerialNumber, ArrivalTime, DepartureTime) Values" +
                    "(19, 5, '12:00:00.00', '12:55:00.00')");

                */
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(1, 1)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(1, 2)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(1, 3)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(1, 4)");

                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(2, 3)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(2, 4)");

                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(3, 3)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(3, 4)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(3, 5)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(3, 6)");

                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(4, 3)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(4, 4)");
                db.Database.ExecuteSqlCommand("Insert into TrafficDays Values(4, 1)");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/20/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/21/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/22/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/23/2022')");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/27/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/28/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/29/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(1, '05/30/2022')");


                db.Database.ExecuteSqlCommand("Insert into Schedules Values(2, '05/27/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(2, '05/28/2022')");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(2, '05/20/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(2, '05/21/2022')");


                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/20/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/21/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/22/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/23/2022')");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/27/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/28/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/29/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(3, '05/30/2022')");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/29/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/30/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/31/2022')");

                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/22/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/23/2022')");
                db.Database.ExecuteSqlCommand("Insert into Schedules Values(4, '05/24/2022')");


                db.Database.ExecuteSqlCommand("Insert into Tickets Values(1,1,1,4,1,400)");
                db.Database.ExecuteSqlCommand("Insert into TicketSeats Values(1,1)");
                db.Database.ExecuteSqlCommand("Insert into Tickets Values(0,1,10,4,21,500)");
                db.Database.ExecuteSqlCommand("Insert into TicketSeats Values(1,2)");
                db.Database.ExecuteSqlCommand("Insert into Tickets Values(0,1,8,1,20,800)");
                db.Database.ExecuteSqlCommand("Insert into TicketSeats Values(1,3)");
                db.SaveChanges();
            }
        }
    }

}
