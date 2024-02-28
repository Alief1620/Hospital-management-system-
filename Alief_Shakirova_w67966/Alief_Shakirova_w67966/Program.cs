using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alief_Shakirova_w67966
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual string GetInfo() => $"{FirstName} {LastName}, Data urodzenia: {BirthDate.ToShortDateString()}";
    }

    public class Patient : Person
    {
        public int PatientId { get; set; }

        public override string GetInfo() => $"Pacjent: {base.GetInfo()}, ID: {PatientId}";

        public override bool Equals(object obj) => obj is Patient patient && PatientId == patient.PatientId;
        public override int GetHashCode() => PatientId.GetHashCode();

        public override string ToString()
        {
            return $"{FirstName}, {LastName}, {BirthDate.ToShortDateString()}, {PatientId}";
        }
    }

    public class MedicalStaff : Person
    {
        public int StaffId { get; set; }

        public override string GetInfo() => $"Personel medyczny: {base.GetInfo()}, ID: {StaffId}";
    }

    public class Doctor : MedicalStaff
    {
        public string Specialization { get; set; }

        public override string GetInfo() => $"Lekarz: {base.GetInfo()}, Specjalizacja: {Specialization}";

        public override string ToString()
        {
            return $"{FirstName}, {LastName}, {BirthDate.ToShortDateString()}, {StaffId}, {Specialization}";
        }
    }

    public class Room : Doctor
    {
        public int RoomNumber { get; set; }
        public bool IsOccupied { get; set; }

        public string GetInfo() => $"Pokój nr {RoomNumber}, Zajęty: {IsOccupied}";

        public override bool Equals(object obj) => obj is Room room && RoomNumber == room.RoomNumber;
        public override int GetHashCode() => RoomNumber.GetHashCode();

        public override string ToString()
        {
            return $"{RoomNumber}, {IsOccupied}";
        }
    }

    public class MedicalEquipment : Room
    {
        public string Name { get; set; }
        public string Condition { get; set; }

        public string GetInfo(string additionalInfo) => $"Dodatkowa informacja: {additionalInfo}, Nazwa: {Name}, Stan: {Condition}";

        public override bool Equals(object obj) => obj is MedicalEquipment equipment && Name == equipment.Name;
        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString()
        {
            return $"{Name}, {Condition}";
        }
    }

    public class Visit : Doctor
    {
        public int VisitId { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime VisitDate { get; set; }

        public string GetInfo() => $"Wizyta dla: {Patient.GetInfo()}, Lekarz: {Doctor.GetInfo()}, Data: {VisitDate.ToShortDateString()}";

        public override bool Equals(object obj) => obj is Visit visit && VisitId == visit.VisitId;
        public override int GetHashCode() => VisitId.GetHashCode();

        public override string ToString()
        {
            return $"{VisitId}, {Patient.ToString()}, {Doctor.ToString()}, {VisitDate.ToShortDateString()}";
        }
    }

    public class DataManager
    {
        private const string PatientsFileName = "Patients.txt";
        private const string MedicalStaffFileName = "MedicalStaff.txt";
        private const string VisitsFileName = "Visits.txt";
        private const string DoctorsFileName = "Doctors.txt";
        private const string RoomsFileName = "Rooms.txt";
        private const string EquipmentFileName = "Equipment.txt";

        public List<Patient> Patients { get; set; }
        public List<MedicalStaff> MedicalStaff { get; set; }
        public List<Visit> Visits { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Room> Rooms { get; set; }
        public List<MedicalEquipment> Equipment { get; set; }

        public DataManager()
        {
            Patients = new List<Patient>();
            MedicalStaff = new List<MedicalStaff>();
            Visits = new List<Visit>();
            Doctors = new List<Doctor>();
            Rooms = new List<Room>();
            Equipment = new List<MedicalEquipment>();
        }

        public void AddPatient(Patient patient)
        {
            Patients.Add(patient);
            SaveDataToFile(PatientsFileName, Patients);
        }

        public List<Patient> GetPatients()
        {
            Patients = LoadDataFromFile<Patient>(PatientsFileName);
            return Patients;
        }

        public void UpdatePatient(Patient updatedPatient)
        {
            UpdateData(updatedPatient, Patients, PatientsFileName);
        }

        public void DeletePatient(int patientId)
        {
            DeleteData<Patient>(patientId, Patients, PatientsFileName);
        }

        public void AddDoctor(Doctor doctor)
        {
            Doctors.Add(doctor);
            SaveDataToFile(DoctorsFileName, Doctors);
        }

        public List<Doctor> GetDoctors()
        {
            Doctors = LoadDataFromFile<Doctor>(DoctorsFileName);
            return Doctors;
        }

        public void UpdateDoctor(Doctor updatedDoctor)
        {
            UpdateData(updatedDoctor, Doctors, DoctorsFileName);
        }

        public void DeleteDoctor(int doctorId)
        {
            DeleteData<Doctor>(doctorId, Doctors, DoctorsFileName);
        }

        public void AddRoom(Room room)
        {
            Rooms.Add(room);
            SaveDataToFile(RoomsFileName, Rooms);
        }

        public List<Room> GetRooms()
        {
            Rooms = LoadDataFromFile<Room>(RoomsFileName);
            return Rooms;
        }

        public void UpdateRoom(Room updatedRoom)
        {
            UpdateData(updatedRoom, Rooms, RoomsFileName);
        }

        public void DeleteRoom(int roomNumber)
        {
            DeleteData<Room>(roomNumber, Rooms, RoomsFileName);
        }

        public void AddMedicalEquipment(MedicalEquipment equipment)
        {
            Equipment.Add(equipment);
            SaveDataToFile(EquipmentFileName, Equipment);
        }

        public List<MedicalEquipment> GetMedicalEquipment()
        {
            Equipment = LoadDataFromFile<MedicalEquipment>(EquipmentFileName);
            return Equipment;
        }

        public void UpdateMedicalEquipment(MedicalEquipment updatedEquipment)
        {
            UpdateData(updatedEquipment, Equipment, EquipmentFileName);
        }

        public void DeleteMedicalEquipment(string equipmentName)
        {
            DeleteData<MedicalEquipment>(equipmentName, Equipment, EquipmentFileName);
        }

        public void AddVisit(Visit visit)
        {
            Visits.Add(visit);
            SaveDataToFile(VisitsFileName, Visits);
        }

        public List<Visit> GetVisits()
        {
            Visits = LoadDataFromFile<Visit>(VisitsFileName);
            return Visits;
        }

        public void UpdateVisit(Visit updatedVisit)
        {
            UpdateData(updatedVisit, Visits, VisitsFileName);
        }

        public void DeleteVisit(int visitId)
        {
            DeleteData<Visit>(visitId, Visits, VisitsFileName);
        }

        public void SaveData()
        {
            SaveDataToFile(PatientsFileName, Patients);
            SaveDataToFile(DoctorsFileName, Doctors);
            SaveDataToFile(RoomsFileName, Rooms);
            SaveDataToFile(EquipmentFileName, Equipment);
            SaveDataToFile(VisitsFileName, Visits);

            Console.WriteLine("Dane zapisane pomyślnie.");
        }

        public void LoadData()
        {
            Patients = LoadDataFromFile<Patient>(PatientsFileName);
            Doctors = LoadDataFromFile<Doctor>(DoctorsFileName);
            Rooms = LoadDataFromFile<Room>(RoomsFileName);
            Equipment = LoadDataFromFile<MedicalEquipment>(EquipmentFileName);
            Visits = LoadDataFromFile<Visit>(VisitsFileName);
        }

        public void SaveDataToFile<T>(string fileName, List<T> data) where T : class
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var item in data)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }

        public List<T> LoadDataFromFile<T>(string fileName) where T : class
        {
            List<T> data = new List<T>();

            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        T item = ParseCsvLine<T>(line);
                        if (item != null)
                        {
                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public T ParseCsvLine<T>(string line) where T : class
        {
            if (typeof(T) == typeof(Patient))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4 && int.TryParse(parts[3], out int patientId))
                {
                    return new Patient
                    {
                        FirstName = parts[0].Trim(),
                        LastName = parts[1].Trim(),
                        BirthDate = DateTime.Parse(parts[2].Trim()),
                        PatientId = patientId
                    } as T;
                }
            }
            else if (typeof(T) == typeof(Doctor))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 5 && int.TryParse(parts[3], out int staffId))
                {
                    return new Doctor
                    {
                        FirstName = parts[0].Trim(),
                        LastName = parts[1].Trim(),
                        BirthDate = DateTime.Parse(parts[2].Trim()),
                        StaffId = staffId,
                        Specialization = parts[4].Trim()
                    } as T;
                }
            }
            else if (typeof(T) == typeof(Room))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out int roomNumber))
                {
                    return new Room
                    {
                        RoomNumber = roomNumber,
                        IsOccupied = bool.Parse(parts[1].Trim())
                    } as T;
                }
            }
            else if (typeof(T) == typeof(MedicalEquipment))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    return new MedicalEquipment
                    {
                        Name = parts[0].Trim(),
                        Condition = parts[1].Trim()
                    } as T;
                }
            }
            else if (typeof(T) == typeof(Visit))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 11 && int.TryParse(parts[0], out int visitId))
                {
                    return new Visit
                    {
                        VisitId = visitId,
                        Patient = ParseCsvLine<Patient>($"{parts[1]}, {parts[2]}, {parts[3]}, {parts[4]}"),
                        Doctor = ParseCsvLine<Doctor>($"{parts[5]}, {parts[6]}, {parts[7]}, {parts[8]}, {parts[9]}"),
                        VisitDate = DateTime.Parse(parts[10].Trim())
                    } as T;
                }
            }

            return null;
        }

        private void UpdateData<T>(T updatedItem, List<T> dataList, string fileName) where T : class
        {
            var comparer = GetEqualityComparer<T>();

            var existingItem = dataList.FirstOrDefault(item => comparer.Equals(item, updatedItem));

            if (existingItem != null)
            {
                int index = dataList.IndexOf(existingItem);
                dataList[index] = updatedItem;
                SaveDataToFile(fileName, dataList);
            }
            else
            {
                Console.WriteLine($"{typeof(T).Name} nie znaleziony.");
            }
        }

        private void DeleteData<T>(object key, List<T> dataList, string fileName) where T : class
        {
            var comparer = GetEqualityComparer<T>();

            var itemToRemove = dataList.FirstOrDefault(item => comparer.Equals(item, (T)key));

            if (itemToRemove != null)
            {
                dataList.Remove(itemToRemove);
                SaveDataToFile(fileName, dataList);
            }
            else
            {
                Console.WriteLine($"{typeof(T).Name} nie znaleziony.");
            }
        }

        private IEqualityComparer<T> GetEqualityComparer<T>()
        {
            return EqualityComparer<T>.Default;
        }
    }

    class Program
    {
        private static DataManager dataManager = new DataManager();

        static void Main(string[] args)
        {
            dataManager.LoadData();

            while (true)
            {
                Console.WriteLine("1. Dodaj pacjenta");
                Console.WriteLine("2. Wyświetl listę pacjentów");
                Console.WriteLine("3. Dodaj lekarza");
                Console.WriteLine("4. Wyświetl listę lekarzy");
                Console.WriteLine("5. Dodaj pokój");
                Console.WriteLine("6. Wyświetl listę pokoi");
                Console.WriteLine("7. Dodaj sprzęt medyczny");
                Console.WriteLine("8. Wyświetl listę sprzętu medycznego");
                Console.WriteLine("9. Dodaj wizytę");
                Console.WriteLine("10. Wyświetl listę wizyt");
                Console.WriteLine("11. Zapisz dane");
                Console.WriteLine("12. Wyjdź");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPatient();
                        break;
                    case "2":
                        DisplayPatients();
                        break;
                    case "3":
                        AddDoctor();
                        break;
                    case "4":
                        DisplayDoctors();
                        break;
                    case "5":
                        AddRoom();
                        break;
                    case "6":
                        DisplayRooms();
                        break;
                    case "7":
                        AddMedicalEquipment();
                        break;
                    case "8":
                        DisplayMedicalEquipment();
                        break;
                    case "9":
                        AddVisit();
                        break;
                    case "10":
                        DisplayVisits();
                        break;
                    case "11":
                        dataManager.SaveData();
                        break;
                    case "12":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
            }
        }

        private static void AddPatient()
        {
            Console.WriteLine("Imię pacjenta:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Nazwisko pacjenta:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Data urodzenia pacjenta (w formacie YYYY-MM-DD):");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("ID pacjenta:");
            int patientId = int.Parse(Console.ReadLine());

            Patient newPatient = new Patient
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                PatientId = patientId
            };

            dataManager.AddPatient(newPatient);
            Console.WriteLine("Pacjent dodany pomyślnie.");
        }

        private static void DisplayPatients()
        {
            List<Patient> patients = dataManager.GetPatients();

            foreach (var patient in patients)
            {
                Console.WriteLine(patient.GetInfo());
            }
        }

        private static void AddDoctor()
        {
            Console.WriteLine("Imię lekarza:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Nazwisko lekarza:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Data urodzenia lekarza (w formacie YYYY-MM-DD):");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("ID lekarza:");
            int staffId = int.Parse(Console.ReadLine());

            Console.WriteLine("Specjalizacja lekarza:");
            string specialization = Console.ReadLine();

            Doctor newDoctor = new Doctor
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                StaffId = staffId,
                Specialization = specialization
            };

            dataManager.AddDoctor(newDoctor);
            Console.WriteLine("Lekarz dodany pomyślnie.");
        }

        private static void DisplayDoctors()
        {
            List<Doctor> doctors = dataManager.GetDoctors();

            foreach (var doctor in doctors)
            {
                Console.WriteLine(doctor.GetInfo());
            }
        }

        private static void AddRoom()
        {
            Console.WriteLine("Numer pokoju:");
            int roomNumber = int.Parse(Console.ReadLine());

            Console.WriteLine("Czy pokój jest zajęty? (true/false):");
            bool isOccupied = bool.Parse(Console.ReadLine());

            Room newRoom = new Room
            {
                RoomNumber = roomNumber,
                IsOccupied = isOccupied
            };

            dataManager.AddRoom(newRoom);
            Console.WriteLine("Pokój dodany pomyślnie.");
        }

        private static void DisplayRooms()
        {
            List<Room> rooms = dataManager.GetRooms();

            foreach (var room in rooms)
            {
                Console.WriteLine(room.GetInfo());
            }
        }

        private static void AddMedicalEquipment()
        {
            Console.WriteLine("Nazwa sprzętu medycznego:");
            string name = Console.ReadLine();

            Console.WriteLine("Stan sprzętu medycznego:");
            string condition = Console.ReadLine();

            MedicalEquipment newEquipment = new MedicalEquipment
            {
                Name = name,
                Condition = condition
            };

            dataManager.AddMedicalEquipment(newEquipment);
            Console.WriteLine("Sprzęt medyczny dodany pomyślnie.");
        }

        private static void DisplayMedicalEquipment()
        {
            List<MedicalEquipment> equipment = dataManager.GetMedicalEquipment();

            foreach (var item in equipment)
            {
                Console.WriteLine(item.GetInfo("Dodatkowa informacja"));
            }
        }

        private static void AddVisit()
        {
            Console.WriteLine("ID wizyty:");
            int visitId = int.Parse(Console.ReadLine());

            Console.WriteLine("Dane pacjenta (imię, nazwisko, data urodzenia, ID pacjenta):");
            string patientData = Console.ReadLine();
            Patient patient = dataManager.ParseCsvLine<Patient>(patientData);

            Console.WriteLine("Dane lekarza (imię, nazwisko, data urodzenia, ID lekarza, specjalizacja):");
            string doctorData = Console.ReadLine();
            Doctor doctor = dataManager.ParseCsvLine<Doctor>(doctorData);

            Console.WriteLine("Data wizyty (w formacie YYYY-MM-DD):");
            DateTime visitDate = DateTime.Parse(Console.ReadLine());

            Visit newVisit = new Visit
            {
                VisitId = visitId,
                Patient = patient,
                Doctor = doctor,
                VisitDate = visitDate
            };

            dataManager.AddVisit(newVisit);
            Console.WriteLine("Wizyta dodana pomyślnie.");
        }

        private static void DisplayVisits()
        {
            List<Visit> visits = dataManager.GetVisits();

            foreach (var visit in visits)
            {
                Console.WriteLine(visit.GetInfo());
            }
        }
    }
}
