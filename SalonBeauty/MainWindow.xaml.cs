using AutoLotModel;

using System;

using System.Collections.Generic;

using System.Data;

using System.Data.Entity;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows;

using System.Windows.Controls;

using System.Windows.Data;

using System.Windows.Documents;

using System.Windows.Input;

using System.Windows.Media;

using System.Windows.Media.Imaging;

using System.Windows.Navigation;

using System.Windows.Shapes;





namespace SalonBeauty

{

    /// <summary> 

    /// Interaction logic for MainWindow.xaml 

    /// </summary> 

    enum ActionState

    {

        New,

        Edit,

        Delete,

        Nothing

    }



    public partial class MainWindow : Window

    {

        ActionState action = ActionState.Nothing;

        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();

        CollectionViewSource customerVSource;

        CollectionViewSource serviceVSource;

        CollectionViewSource customerAppointmentsVSource;





        public MainWindow()

        {

            InitializeComponent();

            DataContext = this;

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)

        {

            customerVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));

            customerVSource.Source = ctx.Customers.Local; 

            ctx.Customers.Load();



            serviceVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("serviceViewSource")));

            serviceVSource.Source = ctx.Services.Local;

            ctx.Services.Load();



            customerAppointmentsVSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerAppointmentsViewSource")));

            //customerAppointmentsVSource.Source = ctx.Appointments.Local; 

            ctx.Appointments.Load();





            BindDataGrid();



            ctx.Services.Load();



            cmbCustomers.ItemsSource = ctx.Customers.Local;

            //cmbCustomers.DisplayMemberPath = "FirstName"; 

            cmbCustomers.SelectedValuePath = "custId";



            cmbServices.ItemsSource = ctx.Services.Local;

            //cmbServices.DisplayMemberPath = "Name"; 

            cmbServices.SelectedValuePath = "servId";

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)

        {

            action = ActionState.New;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            SetValidationBinding();


        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)

        {

            action = ActionState.Edit;
            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            SetValidationBinding();


        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)

        {

            action = ActionState.Delete;

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)

        {

            customerVSource.View.MoveCurrentToNext();

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)

        {

            customerVSource.View.MoveCurrentToPrevious();

        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)

        {

            serviceVSource.View.MoveCurrentToNext();

        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)

        {

            serviceVSource.View.MoveCurrentToPrevious();

        }



        private void SaveCustomers()

        {

            Customer customer = null;

            if (action == ActionState.New)

            {

                try

                {

                    //instantiem Customer entity 

                    customer = new Customer()

                    {

                        FirstName = firstNameTextBox.Text.Trim(),

                        LastName = lastNameTextBox.Text.Trim(),

                        PhoneNumber = int.Parse(phoneNumberTextBox.Text.Trim())

                    };

                    //adaugam entitatea nou creata in context 

                    ctx.Customers.Add(customer);

                    customerVSource.View.Refresh();

                    //salvam modificarile 

                    ctx.SaveChanges();

                }

                //using System.Data; 

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            else if (action == ActionState.Edit)

            {

                try

                {

                    customer = (Customer)customerDataGrid.SelectedItem;

                    customer.FirstName = firstNameTextBox.Text.Trim();

                    customer.LastName = lastNameTextBox.Text.Trim();

                    customer.PhoneNumber = int.Parse(phoneNumberTextBox.Text.Trim());

                    //salvam modificarile 

                    ctx.SaveChanges();

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            else if (action == ActionState.Delete)

            {

                try

                {

                    customer = (Customer)customerDataGrid.SelectedItem;

                    ctx.Customers.Remove(customer);

                    ctx.SaveChanges();

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

                customerVSource.View.Refresh();

            }



        }

        private void SaveServices()

        {

            Service service = null;

            if (action == ActionState.New)

            {

                try

                {

                    //instantiem Customer entity 

                    service = new Service()

                    {

                        Name = nameTextBox.Text.Trim(),

                        Description = descriptionTextBox.Text.Trim(),

                        Price = int.Parse(priceTextBox.Text.Trim())

                    };

                    //adaugam entitatea nou creata in context 

                    ctx.Services.Add(service);

                    serviceVSource.View.Refresh();

                    //salvam modificarile 

                    ctx.SaveChanges();

                }

                //using System.Data; 

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            else if (action == ActionState.Edit)

            {

                try

                {

                    service = (Service)serviceDataGrid.SelectedItem;

                    service.Name = nameTextBox.Text.Trim();

                    service.Description = descriptionTextBox.Text.Trim();

                    service.Price = int.Parse(priceTextBox.Text.Trim());

                    //salvam modificarile 

                    ctx.SaveChanges();

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            else if (action == ActionState.Delete)

            {

                try

                {

                    service = (Service)serviceDataGrid.SelectedItem;

                    ctx.Services.Remove(service);

                    ctx.SaveChanges();

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

                serviceVSource.View.Refresh();

            }



        }



        private void gbOperations_Click(object sender, RoutedEventArgs e)

        {

            Button SelectedButton = (Button)e.OriginalSource;

            Panel panel = (Panel)SelectedButton.Parent;



            foreach (Button B in panel.Children.OfType<Button>())

            {

                if (B != SelectedButton)

                    B.IsEnabled = false;

            }

            gbActions.IsEnabled = true;

        }



        private void ReInitialize()

        {



            Panel panel = gbOperations.Content as Panel;

            foreach (Button B in panel.Children.OfType<Button>())

            {

                B.IsEnabled = true;

            }

            gbActions.IsEnabled = false;

        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)

        {

            ReInitialize();

        }



        private void btnSave_Click(object sender, RoutedEventArgs e)

        {

            TabItem ti = tbCtrlAutoLot.SelectedItem as TabItem;



            switch (ti.Header)

            {

                case "Customers":

                    SaveCustomers();

                    break;

                case "Services":

                    SaveServices();

                    break;

                case "Appointments":

                    SaveAppointments();

                    break;

            }

            ReInitialize();

        }



        private void SaveAppointments()

        {

            Appointment appointment = null;

            if (action == ActionState.New)

            {

                try

                {

                    Customer customer = (Customer)cmbCustomers.SelectedItem;

                    Service service = (Service)cmbServices.SelectedItem;

                    //instantiem Order entity 

                    appointment = new Appointment()

                    {



                        custId = customer.custId,

                        servId = service.servId

                    };

                    //adaugam entitatea nou creata in context 

                    ctx.Appointments.Add(appointment);

                    //salvam modificarile 

                    ctx.SaveChanges();

                    BindDataGrid();

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

            else if (action == ActionState.Edit)

            {

                dynamic selectedAppointment = appointmentsDataGrid.SelectedItem;

                try

                {

                    int curr_id = selectedAppointment.appId;

                    var editedAppointment = ctx.Appointments.FirstOrDefault(s => s.appId == curr_id);

                    if (editedAppointment != null)

                    {

                        editedAppointment.custId = Int32.Parse(cmbCustomers.SelectedValue.ToString());

                        editedAppointment.servId = Convert.ToInt32(cmbServices.SelectedValue.ToString());

                        //salvam modificarile 

                        ctx.SaveChanges();

                    }

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

                BindDataGrid();

                // pozitionarea pe item-ul curent 

                customerAppointmentsVSource.View.MoveCurrentTo(selectedAppointment);

            }

            else if (action == ActionState.Delete)

            {

                try

                {

                    dynamic selectedAppointment = appointmentsDataGrid.SelectedItem;

                    int curr_id = selectedAppointment.appId;

                    var deletedAppointment = ctx.Appointments.FirstOrDefault(s => s.appId == curr_id);

                    if (deletedAppointment != null)

                    {

                        ctx.Appointments.Remove(deletedAppointment);

                        ctx.SaveChanges();

                        MessageBox.Show("Appointment Deleted Successfully", "Message");

                        BindDataGrid();

                    }

                }

                catch (DataException ex)

                {

                    MessageBox.Show(ex.Message);

                }

            }

        }



        private void BindDataGrid()

        {

            var queryOrder = from app in ctx.Appointments

                             join cust in ctx.Customers on app.custId equals

                             cust.custId

                             join serv in ctx.Services on app.servId

                 equals serv.servId

                             select new

                             {

                                 app.appId,

                                 app.servId,

                                 app.custId,

                                 cust.FirstName,

                                 cust.LastName,

                                 serv.Name,

                                 serv.Description

                             };

            customerAppointmentsVSource.Source = queryOrder.ToList();

        }

        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerVSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);

            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerVSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding); //setare binding nou
        }

        
    }

}