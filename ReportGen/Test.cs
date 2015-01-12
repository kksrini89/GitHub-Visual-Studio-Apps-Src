using System.ComponentModel;
using System.Xml.Serialization;

namespace ReportGen
{
    [XmlRoot("Test")]
    public class Test : INotifyPropertyChanged
    {
        public Test()
        {

        }

        private int id;

        [XmlAttribute("ID")]
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string name;

        [XmlElement("Name")]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private double price;

        [XmlElement("Price")]
        public double Price
        {
            get { return price; }
            set
            {
                price = value;
                RaisePropertyChanged("Price");
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
