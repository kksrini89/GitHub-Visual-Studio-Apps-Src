using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ReportGen
{
    [XmlRoot("Hospital")]
    public class Hospital : INotifyPropertyChanged
    {
        private int id;

        [XmlAttribute("ID")]
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        private string name;

        [XmlElement("Name")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();

        [XmlElement("Doctors")]
        public ObservableCollection<Doctor> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                RaisePropertyChanged("Doctors");
            }
        }

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
