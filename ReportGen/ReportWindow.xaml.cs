using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace ReportGen
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region fields

        private bool? result;
        Report reportProp = null;
        AddWindow window = null;
        Hospital hosp = null;
        Doctor doc = null;
        Test test = null;
        Reports reportCollection = new Reports();

        //Test test = new Test();

        //ObservableCollection<Report> reportCollection = new ObservableCollection<Report>();
        //ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
        //ObservableCollection<Hospital> hospitals = new ObservableCollection<Hospital>();
        //ObservableCollection<Test> tests = new ObservableCollection<Test>();

        ObservableCollection<Report> reports = new ObservableCollection<Report>();

        #endregion

        #region Properties

        public Hospital SelectedHospital
        {
            get;
            set;
        }

        public Doctor SelectedDoctor
        {
            get;
            set;
        }

        public Test SelectedTest
        {
            get;
            set;
        }

        public Reports ReportCollection
        {
            get
            {
                return reportCollection;
            }
            set
            {
                reportCollection = value;
                RaisePropertyChanged("ReportCollection");
            }
        }
        public ObservableCollection<Report> Reports
        {
            get
            {
                return reports;
            }
            set
            {
                if (Reports != value)
                {
                    reports = value;
                    RaisePropertyChanged("Reports");
                }
            }
        }

        /*  private static readonly DependencyProperty ReportsProperty = DependencyProperty.Register("Reports",
                                                                                                      typeof(ObservableCollection<Report>),
                                                                                                      typeof(ReportWindow));

          public ObservableCollection<Report> Reports
          {
              get
              {
                  return this.GetValue(ReportsProperty) as ObservableCollection<Report>;
              }
              set
              {
                  SetValue(ReportsProperty, value);
              }
          }*/

        private static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate",
                                                                                                        typeof(DateTime?),
                                                                                                        typeof(ReportWindow),
                                                                                                        new FrameworkPropertyMetadata(DateTime.Now));

        private static readonly DependencyProperty TotalValueProperty = DependencyProperty.Register("TotalValue",
                                                                                                      typeof(double),
                                                                                                      typeof(ReportWindow));

        public DateTime? SelectedDate
        {
            get
            {
                return (DateTime?)this.GetValue(SelectedDateProperty);
            }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }

        public double TotalValue
        {
            get
            {
                return (double)this.GetValue(TotalValueProperty);
            }
            set
            {
                SetValue(TotalValueProperty, value);
            }
        }



        private int? quantityNumber;
        public int? QuantityNumber
        {
            get
            {
                return quantityNumber;
            }
            set
            {
                if (value != quantityNumber)
                {
                    quantityNumber = value;
                    RaisePropertyChanged("QuantityNumber");
                    if (quantityNumber.Value > 1)
                        TotalValue = CalculateTotal(TestPrice.Value, quantityNumber.Value);
                }
            }
        }

        private double? discountValue;
        public double? DiscountValue
        {
            get
            {
                return discountValue;
            }
            set
            {
                if (value != discountValue)
                {
                    discountValue = value;
                    RaisePropertyChanged("DiscountValue");
                    if (discountValue.Value > 1.0)
                        TotalValue = CalculateTotal(TestPrice.Value, QuantityNumber.Value, discountValue.Value);
                }
            }
        }

        private double? testPrice;
        public double? TestPrice
        {
            get
            {
                return testPrice;
            }
            set
            {
                if (value != testPrice)
                {
                    testPrice = value;
                    RaisePropertyChanged("TestPrice");
                }
            }
        }

        /*private static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("QuantityNumber",
                                                                                                      typeof(int?),
                                                                                                      typeof(ReportWindow)); //new FrameworkPropertyMetadata(1)

          private static readonly DependencyProperty DiscountProperty = DependencyProperty.Register("DiscountValue",
                                                                                                      typeof(double?),
                                                                                                      typeof(ReportWindow));//new FrameworkPropertyMetadata(1.0)

          private static readonly DependencyProperty TestPriceProperty = DependencyProperty.Register("TestPrice",
                                                                                                     typeof(double?),
                                                                                                     typeof(ReportWindow));
          public int? QuantityNumber
          {
              get { return (int?)this.GetValue(QuantityProperty); }
              set
              {
                  SetValue(QuantityProperty, value);
                  // TotalValue = CalculateTotal(TestPrice.Value, DiscountValue.Value, QuantityNumber.Value);
              }
          }
          public double? DiscountValue
          {
              get { return (double?)this.GetValue(DiscountProperty); }
              set
              {
                  SetValue(DiscountProperty, value);
                  //TotalValue = CalculateTotal(TestPrice.Value, DiscountValue.Value, QuantityNumber.Value);
              }
          }

          public double? TestPrice
          {
              get { return (double)this.GetValue(TestPriceProperty); }
              set
              {
                  SetValue(TestPriceProperty, value);
              }
          }*/

        public ObservableCollection<Hospital> Hospitals
        {
            get
            {
                return this.GetValue(HospitalsProperty) as ObservableCollection<Hospital>;
            }
            set
            {
                this.SetValue(HospitalsProperty, value);
            }
        }

        private static readonly DependencyProperty HospitalsProperty = DependencyProperty.Register("Hospitals",
                                                                                                    typeof(ObservableCollection<Hospital>),
                                                                                                    typeof(ReportWindow));

        public ObservableCollection<Doctor> Doctors
        {
            get
            {
                return this.GetValue(DoctorsProperty) as ObservableCollection<Doctor>;
            }
            set
            {
                this.SetValue(DoctorsProperty, value);
            }
        }

        private static readonly DependencyProperty DoctorsProperty = DependencyProperty.Register("Doctors",
                                                                                                    typeof(ObservableCollection<Doctor>),
                                                                                                    typeof(ReportWindow));

        [XmlElement("Tests")]
        public ObservableCollection<Test> Tests
        {
            get
            {
                return this.GetValue(TestsProperty) as ObservableCollection<Test>;
            }
            set
            {
                SetValue(TestsProperty, value);
            }
        }

        public static readonly DependencyProperty TestsProperty = DependencyProperty.Register("Tests",
                                                                                                typeof(ObservableCollection<Test>),
                                                                                                typeof(ReportWindow));

        #endregion

        #region Constructor

        public ReportWindow()
        {
            InitializeComponent();
            //Tests = GetTestList();
            FillCombo();
            QuantityNumber = 1;
            DiscountValue = 1.0;
            this.DataContext = this;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Async method to get the Tests Collection
        /// </summary>
        /// <returns>ObservableCollection of test types</returns>
        private async Task FillCombo()
        {
            Hospitals = await Task.Run(() => GetHospitalList());
            Tests = await Task.Run(() => GetTestList());
        }

        private ObservableCollection<Hospital> GetHospitalList()
        {
            XmlSerializer deSerializer = new XmlSerializer(typeof(ObservableCollection<Hospital>));
            TextReader reader = new StreamReader(@"D:\Karthick - App\ReportGen\ReportGen\XML\HospitalList.xml");
            object obj = deSerializer.Deserialize(reader);
            ObservableCollection<Hospital> hospitalList = (ObservableCollection<Hospital>)obj;
            reader.Close();
            return hospitalList;
        }

        private ObservableCollection<Test> GetTestList()
        {
            XmlSerializer deSerializer = new XmlSerializer(typeof(ObservableCollection<Test>));
            TextReader reader = new StreamReader(@"D:\Karthick - App\ReportGen\ReportGen\XML\TestList.xml");
            object obj = deSerializer.Deserialize(reader);
            ObservableCollection<Test> testList = (ObservableCollection<Test>)obj;
            reader.Close();
            return testList;
        }

        private double CalculateTotal(double Price, int Quantity = 1, double Discount = 1.0)
        {
            return (Quantity * (Price * (Discount / 100)));
        }

        #endregion

        #region Event Handlers

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportProp != null && reportProp.Date != ReportDate.SelectedDate.ToString())
                reportProp.Date = ReportDate.SelectedDate.ToString();
        }

        private void AddHospitalClickHandler(object sender, RoutedEventArgs e)
        {
            //if (reportProp != null && Hospitals != null)
            //{
            if (Hospitals == null)
                Hospitals = new ObservableCollection<Hospital>();
            hosp = new Hospital();
            doc = new Doctor();
            //var hospital = new HospitalName();
            window = new AddWindow();
            window.SetHospitalVisibility = true;
            //window.SetTestVisibility = false;
            //window.SomethingHappened += FillDoctorsValues();

            result = window.ShowDialog();
            if (result == false)
                return;

            if (Hospitals != null)
            {
                if (Hospitals.Count > 0)
                    hosp.Id = Hospitals.Count + 1;
                else
                    hosp.Id = 1;
            }
            hosp.Name = window.NameTxt.Text;
            if (window.doctors != null && window.doctors.Count != 0)
            {
                hosp.Doctors = window.doctors;
                hosp.Doctors.ToList().ForEach(c => c.ParentId = hosp.Id);
            }
            //hosp.Doctors.Add();

            /* if (Hospitals.Count == 0)
             {
                 /*reportProp.HospitalProperty.Id = 1;
                 reportProp.HospitalProperty.Name = window.NameTxt.Text;                

             } */

            Hospitals.Add(hosp);
            //Hospitals.Add(reportProp.HospitalProperty);
            //IsolatedStorageFile.GetStore(IsolatedStorageScope.User |IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain,typeof(System.Security.Policy.Url), typeof(System.Security.Policy.Url));

            XmlSerializer serializerHospital = new XmlSerializer(typeof(ObservableCollection<Hospital>));
            using (TextWriter writer = new StreamWriter(@"D:\Karthick - App\ReportGen\ReportGen\XML\HospitalList.xml"))
            {
                serializerHospital.Serialize(writer, Hospitals);

            }
            //}

        }

        private void AddDoctorClickHandler(object sender, RoutedEventArgs e)
        {
            /* if (reportProp != null && Doctors != null)
             {
                 //var doctor = new DoctorName();
                 window = new AddWindow();
                 result = window.ShowDialog();
                 if (result == false)
                     return;
                 if (reportProp.DoctorProperty != null)
                 {
                     reportProp.DoctorProperty.Id = 1;
                     reportProp.DoctorProperty.Name = window.NameTxt.Text;
                 }
                 Doctors.Add(reportProp.DoctorProperty);
                 if (Hospitals != null && Hospitals.Count > 0)
                     reportProp.HospitalProperty.Doctors = Doctors;

                 XmlSerializer serializerDoctor = new XmlSerializer(typeof(ObservableCollection<Doctor>));
                 using (TextWriter writer = new StreamWriter(@"D:\Karthick - App\ReportGen\ReportGen\XML\DoctorList.xml"))
                 {
                     serializerDoctor.Serialize(writer, Doctors);
                 }
             }*/
        }

        private void AddTestClickHandler(object sender, RoutedEventArgs e)
        {
            if (Tests == null)
                Tests = new ObservableCollection<Test>();
            test = new Test();
            /* if (reportProp != null && Tests != null)
             {
                 //var test = new TestName();
                 window = new AddWindow();
                 window.SetTestVisibility = true;
                 result = window.ShowDialog();
                 if (result == false)
                     return;

                 if (Tests.Count > 0)
                     reportProp.TestProperty.Id = Tests.Count + 1;
                 else
                     reportProp.TestProperty.Id = 1;

                 reportProp.TestProperty.Name = window.NameTxt.Text;
                 reportProp.TestProperty.Price = Convert.ToDouble(window.PriceTxt.Text.ToString());
                 Tests.Add(reportProp.TestProperty);

                 XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Test>));
                 using (TextWriter writer = new StreamWriter(@"D:\Karthick - App\ReportGen\ReportGen\XML\TestList.xml"))
                 {
                     serializer.Serialize(writer, Tests);
                 }
             }*/

            window = new AddWindow();
            //window.SetHospitalVisibility = false;
            window.SetTestVisibility = true;
            result = window.ShowDialog();
            if (result == false)
                return;

            if (Tests.Count > 0)
                test.Id = Tests.Count + 1;
            else
                test.Id = 1;

            test.Name = window.NameTxt.Text;
            test.Price = Convert.ToDouble(window.PriceTxt.Text.ToString());
            Tests.Add(test);

            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Test>));
            using (TextWriter writer = new StreamWriter(@"D:\Karthick - App\ReportGen\ReportGen\XML\TestList.xml"))
            {
                serializer.Serialize(writer, Tests);
            }
        }

        private void QueueClickHandler(object sender, RoutedEventArgs e)
        {
            if (ReportCollection == null)
                ReportCollection = new Reports();
            reportProp = new Report();
            if (reportProp != null && SelectedHospital != null && SelectedDoctor != null && SelectedTest != null && SelectedDate.HasValue
                    && QuantityNumber.HasValue && DiscountValue.HasValue && TestPrice != null)
            {
                reportProp.Date = SelectedDate.Value.ToShortDateString();
                reportProp.Hospital = SelectedHospital.Name;
                reportProp.Doctor = SelectedDoctor.Name;
                reportProp.Test = SelectedTest.Name;
                reportProp.Quantity = QuantityNumber.Value;
                reportProp.Discount = DiscountValue;
                reportProp.Price = TestPrice.Value;
                TotalValue = CalculateTotal(TestPrice.Value, QuantityNumber.Value, DiscountValue.Value);
                reportProp.Total = TotalValue;
            }
            ReportCollection.Add(reportProp);
            QuantityNumber = 1;
            DiscountValue = 1.0;
        }
        private void GenerateClickHandler(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Report, Reports> s = new ExportToExcel<Report, Reports>();
            ICollectionView view = CollectionViewSource.GetDefaultView(ReportDataGrid.ItemsSource);
            s.dataToPrint = (Reports)view.SourceCollection;
            s.GenerateReport();
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
                PerformValidation();
            }
        }

        private void PerformValidation()
        {
            Task asyncValidate = new Task(() => DataValidate());
            asyncValidate.Start();
        }

        private void DataValidate()
        {
            //Validation for Quantity
            List<string> errorsForQuantity;
            if (modelErrors.TryGetValue("QuantityNumber", out errorsForQuantity) == false)
                errorsForQuantity = new List<string>();
            else
                errorsForQuantity.Clear();

            if (QuantityNumber.HasValue && QuantityNumber.Value < 0)
                errorsForQuantity.Add("Provide the Quantity");
            modelErrors["QuantityNumber"] = errorsForQuantity;
            if (errorsForQuantity.Count > 0)
                PropertyErrorChanged("QuantityNumber");

            //Validation for Discount
            List<string> errorsForDiscount;
            if (modelErrors.TryGetValue("DiscountValue", out errorsForDiscount) == false)
                errorsForDiscount = new List<string>();
            else
                errorsForDiscount.Clear();
            if (DiscountValue.HasValue && DiscountValue.Value < 0)
                errorsForDiscount.Add("Provide the Discount");
            modelErrors["DiscountValue"] = errorsForDiscount;
            if (errorsForDiscount.Count > 0)
                PropertyErrorChanged("DiscountValue");
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void PropertyErrorChanged(string name)
        {
            if (ErrorsChanged != null)
                ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(name));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForProperties = new List<string>();
            if (propertyName != null)
            {
                modelErrors.TryGetValue(propertyName, out errorsForProperties);
                return errorsForProperties;
            }
            else
                return null;
        }

        Dictionary<string, List<string>> modelErrors = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get
            {
                try
                {
                    var errInfo = modelErrors.Values.FirstOrDefault(rec => rec.Count > 0);
                    if (errInfo != null)
                        return true;
                    else
                        return false;
                }
                catch
                {

                }
                return true;
            }
        }

        #endregion


    }

    public class Reports : ObservableCollection<Report> { }
}
