using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoxAndHounds.Models;

namespace FoxAndHounds.Controllers
{
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Friday).Include(s => s.Monday).Include(s => s.Saturday).Include(s => s.Sunday).Include(s => s.Thursday).Include(s => s.Tuesday).Include(s => s.Wednesday);
            return View(schedules.ToList());
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            List<List<Employee>> days = new List<List<Employee>>();
            var monday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.MondayId).Select(data => data.WorkerId).ToList();
            days = doStuff(monday, days);
            var tuesday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.TuesdayId).Select(data => data.WorkerId).ToList();
            days = doStuff(tuesday, days);
            var wednesday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.WednesdayId).Select(data => data.WorkerId).ToList();
            days = doStuff(wednesday, days);
            var thusday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.ThursdayId).Select(data => data.WorkerId).ToList();
            days = doStuff(thusday, days);
            var friday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.FridayId).Select(data => data.WorkerId).ToList();
            days = doStuff(friday, days);
            var saturday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.SaturdayId).Select(data => data.WorkerId).ToList();
            days = doStuff(saturday, days);
            var sunday = db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == schedule.SundayId).Select(data => data.WorkerId).ToList();
            days = doStuff(sunday, days);
            return View(days);
        }
        public List<List<Employee>> doStuff(IEnumerable<int> workers, List<List<Employee>> days)
        {
            List<Shift> exps = new List<Shift>();
            foreach (int expo in workers)
            {
                exps.Add(db.Shifts.Where(data => data.Id == expo).First());
            }
            List<Employee> exs = new List<Employee>();
            foreach (Shift ex in exps)
            {
                exs.Add(db.Employees.Where(data => data.Id == ex.EmployeeId).First());
            }
            days.Add(exs);
            return days;
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DatePickerModel date)
        {
            if (ModelState.IsValid)
            {

                Schedule schedule = new Schedule();
                ScheduleDay Monday = new ScheduleDay();
                Monday.Day = date.dtmDate;
                db.ScheduleDays.Add(Monday);
                db.SaveChanges();
                Monday = ScheduleWeek(Monday);
                db.SaveChanges();
                schedule.MondayId = Monday.Id;

                ScheduleDay Tuesday = new ScheduleDay();
                Tuesday.Day = date.dtmDate.AddDays(1);
                db.ScheduleDays.Add(Tuesday);
                db.SaveChanges();
                Tuesday = ScheduleWeek(Tuesday);
                db.SaveChanges();
                schedule.TuesdayId = Tuesday.Id;

                ScheduleDay Wednesday = new ScheduleDay();
                Wednesday.Day = date.dtmDate.AddDays(2);
                db.ScheduleDays.Add(Wednesday);
                db.SaveChanges();
                Wednesday = ScheduleWeek(Wednesday);
                db.SaveChanges();
                schedule.WednesdayId = Wednesday.Id;

                ScheduleDay Thursday = new ScheduleDay();
                Thursday.Day = date.dtmDate.AddDays(3);
                db.ScheduleDays.Add(Thursday);
                db.SaveChanges();
                Thursday = ScheduleWeek(Thursday);
                db.SaveChanges();
                schedule.ThursdayId = Thursday.Id;

                ScheduleDay Friday = new ScheduleDay();
                Friday.Day = date.dtmDate.AddDays(4);
                db.ScheduleDays.Add(Friday);
                db.SaveChanges();
                Friday = ScheduleFriday(Friday);
                db.SaveChanges();
                schedule.FridayId = Friday.Id;

                ScheduleDay Saturday = new ScheduleDay();
                Saturday.Day = date.dtmDate.AddDays(5);
                db.ScheduleDays.Add(Saturday);
                db.SaveChanges();
                Saturday = ScheduleSaturday(Saturday);
                db.SaveChanges();
                schedule.SaturdayId = Saturday.Id;

                ScheduleDay Sunday = new ScheduleDay();
                Sunday.Day = date.dtmDate.AddDays(6);
                db.ScheduleDays.Add(Sunday);
                db.SaveChanges();
                Sunday = ScheduleSunday(Sunday);
                db.SaveChanges();
                schedule.SundayId = Sunday.Id;

                db.Schedules.Add(schedule);
                db.SaveChanges();

                return RedirectToAction("Index");
            }


            return View();
        }

        private bool checkIfWorking(Employee employee, List<WorkerScheduleDayJunction> workers)
        {
            foreach(WorkerScheduleDayJunction worker in workers)
            {
                if(employee.Id == worker.Worker.EmployeeId)
                {
                    return true;
                }
            }
            return false;
        }
        private bool checkIfDayAskedOff(Employee employee, DateTime date)
        {
            var daysOff = db.DaysOff.Where(data => data.EmployeeId == employee.Id).ToList(); ;
            if(daysOff.Count < 1)
            {
                return false;
            }
            foreach(DayOff dayOff in daysOff)
            {
                if(dayOff.Day.Year == date.Year && dayOff.Day.Month == date.Month && dayOff.Day.Day == date.Day)
                {
                    return true;
                }
            }
            return false;
        }
        private void AddExpo(Expo expo, ScheduleDay Day, DateTime StartTime)
        {
            Shift expoShift = new Shift
            {
                Employee = expo.Employee,
                StartTime = StartTime,
                EndTime = null
            };
            db.Shifts.Add(expoShift);
            db.SaveChanges();
            WorkerScheduleDayJunction temp = new WorkerScheduleDayJunction
            {
                Worker = expoShift,
                ScheduleDay = Day
            };
            db.WorkerScheduleDayJuction.Add(temp);
            db.SaveChanges();
            ExpoScheduleDayJunction temp2 = new ExpoScheduleDayJunction
            {
                Expo = expoShift,
                ScheduleDay = Day
            };
            db.ExpoScheduleDayJunction.Add(temp2);
            db.SaveChanges();
        }
        private void AddFoodRunner(FoodRunner foodRunner, ScheduleDay Day, DateTime StartTime)
        {
            Shift foodRunnerShift = new Shift
            {
                Employee = foodRunner.Employee,
                StartTime = StartTime,
                EndTime = null
            };
            db.Shifts.Add(foodRunnerShift);
            db.SaveChanges();
            WorkerScheduleDayJunction temp = new WorkerScheduleDayJunction
            {
                Worker = foodRunnerShift,
                ScheduleDay = Day
            };
            db.WorkerScheduleDayJuction.Add(temp);
            db.SaveChanges();
            FoodRunnerScheduleDayJunction temp2 = new FoodRunnerScheduleDayJunction
            {
                FoodRunner = foodRunnerShift,
                ScheduleDay = Day
            };
            db.FoodRunnerScheduleDayJunction.Add(temp2);
            db.SaveChanges();
        }
        private void AddHost(Host host, ScheduleDay Day, DateTime StartTime)
        {
            Shift hostShift = new Shift
            {
                Employee = host.Employee,
                StartTime = StartTime,
                EndTime = null
            };
            db.Shifts.Add(hostShift);
            db.SaveChanges();
            WorkerScheduleDayJunction temp = new WorkerScheduleDayJunction
            {
                Worker = hostShift,
                ScheduleDay = Day
            };
            db.WorkerScheduleDayJuction.Add(temp);
            db.SaveChanges();
            HostScheduleDayJunction temp2 = new HostScheduleDayJunction
            {
                Host = hostShift,
                ScheduleDay = Day
            };
            db.HostScheduleDayJunction.Add(temp2);
            db.SaveChanges();
        }
        private void AddBusser(Busser busser, ScheduleDay Day, DateTime StartTime)
        {
            Shift busserShift = new Shift
            {
                Employee = busser.Employee,
                StartTime = StartTime,
                EndTime = null
            };
            db.Shifts.Add(busserShift);
            db.SaveChanges();
            WorkerScheduleDayJunction temp = new WorkerScheduleDayJunction
            {
                Worker = busserShift,
                ScheduleDay = Day
            };
            db.WorkerScheduleDayJuction.Add(temp);
            db.SaveChanges();
            BusserScheduleDayJunction temp2 = new BusserScheduleDayJunction
            {
                Busser = busserShift,
                ScheduleDay = Day
            };
            db.BusserScheduleDayJunction.Add(temp2);
            db.SaveChanges();
        }
        private ScheduleDay ScheduleWeek(ScheduleDay Day)
        {
            Random rnd = new Random();
            
            while(db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if(!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee,Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.FoodRunnerScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 30, 0);
                var list = db.FoodRunners.ToList();
                FoodRunner foodRunner = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == foodRunner.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == foodRunner.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddFoodRunner(foodRunner, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 18, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.MondayId || data.Id == employeeAvailability.TuesdayId || data.Id == employeeAvailability.WednesdayId || data.Id == employeeAvailability.ThursdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }

            return Day;
        }

        private ScheduleDay ScheduleFriday(ScheduleDay Day)
        {
            Random rnd = new Random();

            while (db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.FoodRunnerScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 0, 0);
                var list = db.FoodRunners.ToList();
                FoodRunner foodRunner = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == foodRunner.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == foodRunner.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddFoodRunner(foodRunner, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 3)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 4)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 4)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 6)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.FridayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }

            return Day;
        }
        private ScheduleDay ScheduleSaturday(ScheduleDay Day)
        {
            Random rnd = new Random();

            while (db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 15, 30, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 00, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.FoodRunnerScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.FoodRunners.ToList();
                FoodRunner foodRunner = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == foodRunner.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == foodRunner.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddFoodRunner(foodRunner, Day, newDate);

                    }
                }
            }
            while (db.FoodRunnerScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.FoodRunners.ToList();
                FoodRunner foodRunner = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == foodRunner.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == foodRunner.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddFoodRunner(foodRunner, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 15, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 3)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 4)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 3)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 5)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 17, 30, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SaturdayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }

            return Day;
        }

        private ScheduleDay ScheduleSunday(ScheduleDay Day)
        {
            Random rnd = new Random();

            while (db.ExpoScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Expos.ToList();
                Expo expo = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == expo.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == expo.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddExpo(expo, Day, newDate);

                    }
                }
            }
            while (db.FoodRunnerScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.FoodRunners.ToList();
                FoodRunner foodRunner = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == foodRunner.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == foodRunner.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddFoodRunner(foodRunner, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 2)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 10, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 3)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 10, 30, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 4)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 15, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 5)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.HostScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 6)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Hosts.ToList();
                Host host = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == host.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == host.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddHost(host, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 1)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 10, 30, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 3)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 11, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 5)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 0, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }
            while (db.BusserScheduleDayJunction.Where(data => data.ScheduleDayId == Day.Id).ToList().Count < 6)
            {
                DateTime newDate = new DateTime(Day.Day.Year, Day.Day.Month, Day.Day.Day, 16, 30, 0);
                var list = db.Bussers.ToList();
                Busser busser = list.ElementAt(rnd.Next(list.Count));
                Employee employee = db.Employees.Where(data => data.Id == busser.EmployeeId).First();
                if (!checkIfWorking(employee, db.WorkerScheduleDayJuction.Where(data => data.ScheduleDayId == Day.Id).ToList()) && !checkIfDayAskedOff(employee, Day.Day))
                {
                    Availability employeeAvailability = db.Availabilities.Where(data => data.EmployeeId == busser.EmployeeId).First();
                    AvailabilityDay employeeDay = db.AvailabilityDays.Where(data => data.Id == employeeAvailability.SundayId).First();
                    if (employeeDay.AbleToWork && (employeeDay.StartTime.Value.Hour < newDate.Hour || (newDate.Hour == employeeDay.StartTime.Value.Hour && newDate.Minute >= employeeDay.StartTime.Value.Minute)))
                    {
                        AddBusser(busser, Day, newDate);

                    }
                }
            }

            return Day;
        }




        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.FridayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.FridayId);
            ViewBag.MondayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.MondayId);
            ViewBag.SaturdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.SaturdayId);
            ViewBag.SundayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.SundayId);
            ViewBag.ThursdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.ThursdayId);
            ViewBag.TuesdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.TuesdayId);
            ViewBag.WednesdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.WednesdayId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MondayId,TuesdayId,WednesdayId,ThursdayId,FridayId,SaturdayId,SundayId")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = schedule.Id });
            }
            ViewBag.FridayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.FridayId);
            ViewBag.MondayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.MondayId);
            ViewBag.SaturdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.SaturdayId);
            ViewBag.SundayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.SundayId);
            ViewBag.ThursdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.ThursdayId);
            ViewBag.TuesdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.TuesdayId);
            ViewBag.WednesdayId = new SelectList(db.ScheduleDays, "Id", "Id", schedule.WednesdayId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
