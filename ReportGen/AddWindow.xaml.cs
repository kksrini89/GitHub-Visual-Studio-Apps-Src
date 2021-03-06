﻿using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ReportGen
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
        Doctor doc = null;

        public AddWindow()
        {
            InitializeComponent();
            NameTxt.Focus();
            //SetVisibility = false;
            this.DataContext = this;
        }

        public static readonly DependencyProperty SetTestVisibilityProperty = DependencyProperty.Register("SetTestVisibility",
                                                                                        typeof(bool), typeof(AddWindow));

        //new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        private static void OnVisibiityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static readonly DependencyProperty SetHospitalVisibilityProperty = DependencyProperty.Register("SetHospitalVisibility", typeof(bool),
                                                                                                                typeof(AddWindow));

        public bool SetHospitalVisibility
        {
            get
            {
                return (bool) this.GetValue(SetHospitalVisibilityProperty);
            }
            set
            {
                SetValue(SetHospitalVisibilityProperty, value);
            }
        }

        public bool SetTestVisibility
        {
            get
            {
                return (bool)this.GetValue(SetTestVisibilityProperty);
            }
            set
            {
                this.SetValue(SetTestVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Acts as Enter Key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkClickHandler(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelClickHandler(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; //Before close the dialog set Dialog result to false since default value of dialog result is false ,
        }

        private void AddDoctorClickHandler(object sender, RoutedEventArgs e)
        {
            doc = new Doctor();
            if (doctors != null && doctors.Count == 0)
                doc.Id = 1;
            else
                doc.Id = doctors.Count + 1;
            if (!string.IsNullOrEmpty(DoctorTxt.Text))
                doc.Name = DoctorTxt.Text;

            doctors.Add(doc);
            DoctorTxt.Text = string.Empty;
            DoctorTxt.Focus();            
        }
    }
}
