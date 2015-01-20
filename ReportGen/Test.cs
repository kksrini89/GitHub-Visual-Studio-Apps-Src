using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;

namespace ReportGen
{
    [XmlRoot("Test")]
    public class Test : INotifyPropertyChanged //INotifyDataErrorInfo
    {
        Dictionary<string, List<string>> modelErrors = new Dictionary<string, List<string>>();

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
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
               // PerformValidation();
            }
        }        

       /* private void PerformValidation()
        {
            Task validate = new Task(() => DataValidation());
            validate.Start();
            //return Task.Run(() => DataValidation()); // if Task as return type then this line will be suitable one.
        }

        private void DataValidation()
        {
            //Validation for Price
            List<string> errorsForTestName;
            if (modelErrors.TryGetValue(Name, out errorsForTestName) == false)
            {
                errorsForTestName = new List<string>();
            }
            else
                errorsForTestName.Clear();
            if (string.IsNullOrEmpty(Price.ToString()))
                errorsForTestName.Add("Oops!!! You missed data for Price");
            modelErrors["Price"] = errorsForTestName;
            if (errorsForTestName.Count > 0)
                PropertyErrorsChanged("Price");
        }*/

        #endregion

        #region INotifyDataErrorInfo Implementation

        /*  public event System.EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// The ErrorChaged handler which will be on each property.
        /// </summary>
        /// <param name="propertyName"></param>
        public void PropertyErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                var evtargs = new DataErrorsChangedEventArgs(propertyName);
                ErrorsChanged.Invoke(this, evtargs);
            }
        }


        /// <summary>
        /// Method to get errors on a specific property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
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
        } */
        #endregion
    }
}
