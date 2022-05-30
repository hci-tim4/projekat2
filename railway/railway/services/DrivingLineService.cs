using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using railway.database;
using railway.exception;
using railway.model;

namespace railway.services
{
    public class DrivingLineService
    {
        public void updateBasicDrivingService(string name, Train train, int drivingLineId)
        {
            if (name.Trim() == "")
            { 
                throw new NotDefinedException("Mrežna linija mora da ima ime");
            }

            if (train == null)
            {
                throw new NotDefinedException("Mrežna linija mora da ima voz");
            }


            using (var db = new RailwayContext())
            {
                var dl = (from d in db.drivingLines where d.Id == drivingLineId select d).SingleOrDefault();

                if (dl == null)
                {
                    throw new NotDefinedException("Nepostojeća mrežna linija");
                }

                dl.Name = name;
                dl.TrainId = train.Id;

                db.SaveChanges();            
            }
        }

        
        public void saveDrivingService(string name, Train train, DateTime startSelectedDate,
            ObservableCollection<Station> stations)
        {
            if (name.Trim() == "")
            { 
                throw new NotDefinedException("Mrežna linija mora da ima ime");
            }

            if (train == null)
            {
                throw new NotDefinedException("Mrežna linija mora da ima voz");
            }


            using (var db = new RailwayContext())
            {
                var checkForDrivingLineWithTheSameName =
                    (from d in db.drivingLines where d.Name == name select d).SingleOrDefault();

                if (checkForDrivingLineWithTheSameName != null)
                {
                    throw new AlreadyDefinedException("Već imate mrežnu liniju sa istom imenom");
                }
                
                DrivingLine dl = new DrivingLine()
                {
                    Name = name,
                    TrainId = train.Id,
                    startDate = startSelectedDate
                };
                db.drivingLines.Add(dl);

                if (stations != null)
                    saveStations(db, dl, stations);

                db.SaveChanges();            
            }
        }

        private void saveStations(RailwayContext db, DrivingLine drivingLine, ObservableCollection<Station> stations)
        {
            int serialNumber = 1;
            foreach (Station s in stations)
            {
                StationSchedule ss = new StationSchedule()
                {
                    SerialNumber = serialNumber,
                    StationId = s.Id,
                    DrivingLineId = drivingLine.Id,
                };
                serialNumber++;
                db.stationsSchedules.Add(ss);
            }
        }
    }
}