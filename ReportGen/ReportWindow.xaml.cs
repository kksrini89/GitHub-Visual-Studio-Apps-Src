using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Linq;

namespace ReportGen
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        Report reportProp = new Report();

        Test test = new Test();
        ObservableCollection<Report> reportCollection = new ObservableCollection<Report>();
        ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
        //ObservableCollection<Hospital> hospitals = new ObservableCollection<Hospital>();
        ObservableCollection<Test> tests = new ObservableCollection<Test>();
        ObservableCollection<Report> Reports = new ObservableCollection<Report>();

        private bool? result;
        public ReportWindow()
        {
            InitializeComponent();
            //Tests = GetTestList();
            FillTestCombo();
            this.DataContext = this;
        }

        private async void FillTestCombo()
        {
            Tests = await Task.Run(() => GetTestList());
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

        private static readonly DependencyProperty HospitalsProperty = DependencyProperty.Register("Hospitals", typeof(ObservableCollection<Hospital>), typeof(ReportWindow));

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

        private static readonly DependencyProperty DoctorsProperty = DependencyProperty.Register("Doctors", typeof(ObservableCollection<Doctor>), typeof(ReportWindow));

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

        public static readonly DependencyProperty TestsProperty = DependencyProperty.Register("Tests", typeof(ObservableCollection<Test>), typeof(ReportWindow));

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reportProp != null && reportProp.Date != ReportDate.SelectedDate)
                reportProp.Date = ReportDate.SelectedDate;
        }
        AddWindow window = null;
        Doctor doc = null;
        private void AddHospitalClickHandler(object sender, RoutedEventArgs e)
        {
            //if (reportProp != null && Hospitals != null)
            //{
            if (Hospitals == null)
                Hospitals = new ObservableCollection<Hospital>();
            Hospital hosp = new Hospital();
            doc = new Doctor();
            //var hospital = new HospitalName();
            window = new AddWindow();
            window.SetHospitalVisibility = true;
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
            if (reportProp != null && Doctors != null)
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
            }
        }

        private void AddTestClickHandler(object sender, RoutedEventArgs e)
        {
            if (reportProp != null && Tests != null)
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
            }
        }

        private void GenerateClickHandler(object sender, RoutedEventArgs e)
        {

        }

        private void HospitalCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reportProp.HospitalProperty = HospitalCombo.SelectedItem as Hospital;
        }

        private void DoctorsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TestCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
