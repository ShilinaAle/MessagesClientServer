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
        ObservableCollection<Message> _messageList;
        ICommand _SendCommand;
        ICommand _FilterCommand;
        string _textOfMessage;
        static Guid _guid = Guid.NewGuid();
        DateTime _startDate;
        DateTime _startDefaultDate;
        DateTime _endDate;  
        public ViewModel()
        {
            MessageList = new ObservableCollection<Message>();
            client.BaseAddress = new Uri("http://localhost:5185/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            GetAsync().GetAwaiter();
        }

        //обновление списка
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //получение данных с сервера
        async Task GetAsync()
        {
            try
            {
                List<Message> listOfMessages = await GetMessagesAsync("/GetAllMessages");
                foreach(var i in listOfMessages)
                {
                    MessageList.Add(i);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception in GetAsync: {e.Message}");
            }
        }

        async Task<List<Message>> GetMessagesAsync(string path)
        {
            List<Message> listOfMessages = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                listOfMessages = await response.Content.ReadAsAsync<List<Message>>();
            }
            return listOfMessages;
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
            if (TextOfMessage != null && TextOfMessage.Length > 0)
                return true;
            return false;
        }

        //отправка данных на сервер
        private async Task PostAsync()
        {
            try
            {
                Message mes = new Message { MessageId = DateTime.Now.ToString(), Text = TextOfMessage, UserId = _guid };
                await PostMessageAsync(mes);
                MessageList.Clear();
                GetAsync().GetAwaiter();
                MessageBox.Show("Добавление прошло успешно");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Exception in PostAsync: {e.Message}");
            }
            Console.ReadLine();
        }

        static async Task PostMessageAsync(Message mes)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "/SaveMessage", mes);
            response.EnsureSuccessStatusCode();
        }

        //фильтрация
        public ICommand FilterCommand
        {
            get
            {
                if (_FilterCommand == null)
                {
                    _FilterCommand = new MyCommand(FilterExecute, CanFilterExecute);
                }
                return _FilterCommand;
            }
        }

        private void FilterExecute(object parameter)
        {
            FilterMessages();
        }

        private bool CanFilterExecute(object parameter)
        {
            //TODO
            //if (TextOfMessage != null && TextOfMessage.Length > 0)
                return true;
            //return false;
        }

        private void FilterMessages()
        {
            ObservableCollection<Message> newMessageList = new ObservableCollection<Message>();
            foreach (Message message in MessageList)
            {
                if (DateTime.Parse(message.MessageId) > StartDate && DateTime.Parse(message.MessageId) < EndDate)
                    newMessageList.Add(message);
            }
            MessageList = newMessageList;   

        }

        //свойства
        public ObservableCollection<Message> MessageList
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                OnPropertyChanged();
            }
        }

        public string TextOfMessage
        {
            get { return _textOfMessage; }
            set { _textOfMessage = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

       //DateTime _startDefaultDate
        public DateTime StartDefaultDate
        {
            get {
                if (_startDefaultDate == DateTime.MinValue)
                    _startDefaultDate = DateTime.Now;
                return _startDefaultDate;
            }
            set { _startDate = value; }
        }
    }
}
