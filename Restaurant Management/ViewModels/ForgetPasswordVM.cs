﻿using MongoDB.Driver;
using Restaurant_Management.Models;
using Restaurant_Management.Utilities;
using Restaurant_Management.Views.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Restaurant_Management.ViewModels.ComponentVM
{
    public class ForgetPasswordVM : ViewModelBase
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand SendPassCM { get; set; }
        
        public ICommand CloseWDCM { get; set; }
        
        public ICommand MinimizeWDCM { get; set; }
        
        public ICommand MoveWDCM { get; set; }

        private readonly IMongoCollection<Employees> _Employees;

        private IMongoCollection<Employees> GetEmployees()
        {
            string connectionString = "mongodb+srv://taint04:H20YQ9j6nvKXiaoA@tai-server.0x4tojd.mongodb.net/";

            string databaseName = "Restaurant_Management_Application";

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);

            return database.GetCollection<Employees>("Employees");
        }

        public ForgetPasswordVM()
        {
            _Employees = GetEmployees();

            InitializeCommand();
        }

        private void InitializeCommand()
        {
            SendPassCM = new RelayCommand<ForgetPasswordView>((p) => true, (p) => _SendPass(p));
           
            CloseWDCM = new RelayCommand<ForgetPasswordView>((p) => true, (p) => _CloseWD(p));
            
            MinimizeWDCM = new RelayCommand<ForgetPasswordView>((p) => true, (p) => _MinimizeWD(p));
            
            MoveWDCM = new RelayCommand<ForgetPasswordView>((p) => true, (p) => _MoveWD(p));
        }

        private string GetPass(string email)
        {
            var employee = _Employees.Find(a => a.Email == email).FirstOrDefault();

            if (employee != null)
            {
                Random rand = new Random();

                string newPass = rand.Next(100000, 999999).ToString();

                var update = Builders<Employees>.Update.Set("Password", newPass);

                _Employees.UpdateOne(a => a.Email == email, update);

                return newPass;
            }

            return null;
        }

        private void _SendPass(ForgetPasswordView parameter)
        {
            string email = parameter.Email.Text;

            var employee = _Employees.Find(a => a.Email == email).FirstOrDefault();

            if (employee == null)
            {
                MessageBox.Show("This email is not registered!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            string newPass = GetPass(email);

            if (!string.IsNullOrEmpty(newPass))
            {
                try
                {
                    string messageBody = "Please enter password " + newPass + " to log in.";
                    
                    MailMessage message = new MailMessage("0nlyfood.corporation@gmail.com", email, "Password retrieval", messageBody);

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                    smtpClient.EnableSsl = true;

                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Credentials = new NetworkCredential("0nlyfood.corporation@gmail.com", "yhrq sgwp oqgc umbe");

                    smtpClient.Send(message);

                    MessageBox.Show("Password sent to registration email!", "Notification");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while sending the email: " + ex.Message, "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("An error occurred while retrieving the new password!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _CloseWD(ForgetPasswordView paramater)
        {
            var window = Window.GetWindow(paramater);
            
            if (window != null)
            {
                window.Close();
            }
        }
        
        private void _MinimizeWD(ForgetPasswordView paramater)
        {
            var window = Window.GetWindow(paramater);
            
            if (window != null)
            {
                WindowState originalWindowState = window.WindowState;
                
                window.WindowState = WindowState.Minimized;
            }
        }
        
        private void _MoveWD(ForgetPasswordView paramater)
        {
            var window = Window.GetWindow(paramater);
            
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
