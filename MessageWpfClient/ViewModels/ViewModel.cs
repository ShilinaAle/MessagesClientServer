using MessageWpfClient.Commands;
using MessageWpfClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MessageWpfClient.ViewModels
{
    internal class ViewModel
    {
        static HttpClient client = new HttpClient();
        ObservableCollection<Message> _messageList; //отображаемый лист
        ICommand _SendCommand;
        //bool _canSend = false; //доступность кнопки
        public ViewModel()
        {
            _messageList = new ObservableCollection<Message>(); 
        }

        public ObservableCollection<Message> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //кнопка отправки
        public ICommand SendCommand
        {
            get
            {
                if (_SendCommand == null)
                {
                    _SendCommand = new MyCommand(SendExecute, CanSendExecute);
                }
                return _SendCommand;
            }
        }

        private void SendExecute(object parameter)
        {
            PostAsync().GetAwaiter();
        }

        private bool CanSendExecute(object parameter)
        {
            //if (_canSend == false)
            //    return false;
            return true;
        }

        private async Task PostAsync()
        {
            client.BaseAddress = new Uri("http://localhost:5185/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                Message mes = new Message { MessageId = "12", Text = "2", UserId = new Guid() };
                await PostMessageAsync(mes);
                MessageBox.Show("Добавление прошло успешно");
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception in PostAsync");
            }
            Console.ReadLine();
        }

        static async Task PostMessageAsync(Message mes)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "/SaveMessage", mes);
            response.EnsureSuccessStatusCode();
        }
    }
}
