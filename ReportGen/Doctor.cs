using System.ComponentModel;
using System.Xml.Serialization;

namespace ReportGen
{
    [XmlRoot("Doctor")]
    public class Doctor : INotifyPropertyChanged
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

        private int parentId;

        [XmlElement("HospitalId")]
        public int ParentId
        {
            get { return parentId; }
            set
            {
                if (parentId != value)
                {
                    parentId = value;
                    RaisePropertyChanged("ParentId");
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
