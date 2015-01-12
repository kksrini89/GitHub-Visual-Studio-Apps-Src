using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGen
{
    public class Report : INotifyPropertyChanged
    {
        #region Variables
        private DateTime? date;
       /* private ObservableCollection<Hospital> hospitals;
        private ObservableCollection<Doctor> doctors;
        private ObservableCollection<Test> tests;*/
       /* private string doctor;
        private string hospital;
        private string test;*/
        private Test testProperty=new Test();
        private Hospital hospitalProperty = new Hospital();
        private Doctor doctorProperty =new Doctor();
        private int quantity;
        //private double price;
        private double? discount;
        private double? total;

        #endregion

        #region Properties
        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

       /* public ObservableCollection<Hospital> Hospitals
        {
            get { return hospitals; }
            set
            {
                hospitals = value;
                RaisePropertyChanged("Hospitals");
            }
        }

        public ObservableCollection<Doctor> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                RaisePropertyChanged("Doctors");
            }
        }

        public ObservableCollection<Test> Tests
        {
            get { return tests; }
            set
            {
                tests = value;
                RaisePropertyChanged("Tests");
            }
        }*/

       /* public string Doctor
        {
            get { return doctor; }
            set
            {
                doctor = value;
                RaisePropertyChanged("Doctor");
            }
        }

        public string Hospital
        {
            get { return hospital; }
            set
            {
                hospital = value;
                RaisePropertyChanged("Hospital");
            }
        }

        public string Test
        {
            get { return test; }
            set
            {
                test = value;
                RaisePropertyChanged("Test");
            }
        }*/
        public Doctor DoctorProperty
        {
            get { return doctorProperty; }
            set
            {
                doctorProperty = value;
                RaisePropertyChanged("DoctorProperty");
            }
        }

        public Hospital HospitalProperty
        {
            get { return hospitalProperty; }
            set
            {
                hospitalProperty = value;
                RaisePropertyChanged("HospitalProperty");
            }
        }

        public Test TestProperty
        {
            get { return testProperty; }
            set
            {
                testProperty = value;
                RaisePropertyChanged("TestProperty");
            }
        }


        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

       /* public double Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChanged("Price");
            }
        }*/

        public double? Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                RaisePropertyChanged("Discount");
            }
        }

        public double? Total
        {
            get { return total; }
            set
            {
                total = value;
                RaisePropertyChanged("Total");
            }
        }

        #endregion

        #region Constructor
        public Report()
        {
            //Hospitals = new ObservableCollection<Hospital>();
            //Doctors = new ObservableCollection<Doctor>();
            //Tests = new ObservableCollection<Test>();
        }
        #endregion

        #region PropertyChangedNotification

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
