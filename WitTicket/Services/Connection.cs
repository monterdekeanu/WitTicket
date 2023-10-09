using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WitTicket.Model;

namespace WitTicket.Services
{
    class Connection
    {
        public static string pathEvents = Path.Combine(Environment.GetEnvironmentVariable("DATABASE_EVENTS", EnvironmentVariableTarget.Process), "Events.json");
        public static string pathUsers = Path.Combine(Environment.GetEnvironmentVariable("DATABASE_USERS",EnvironmentVariableTarget.Process), "Users.json");
        public ObservableCollection<UserModel> GetUsersList()
        {
            ObservableCollection<UserModel> userList = new ObservableCollection<UserModel>();
            string jsonString = JsonSerializer.Serialize(userList);
            if (!File.Exists(pathUsers))
            {
                File.WriteAllText(pathUsers, jsonString);
            }
            else
            {
                userList = JsonSerializer.Deserialize<ObservableCollection<UserModel>>(File.ReadAllText(pathUsers));
            }
            return userList;
        }

        public void SaveUsersList(ObservableCollection<UserModel> userList)
        {
            string jsonString = JsonSerializer.Serialize(userList);
            File.WriteAllText(pathUsers, jsonString);
        }


        public ObservableCollection<EventModel> GetEventsList()
        {
            ObservableCollection<EventModel> eventList = new ObservableCollection<EventModel>();
            string jsonString = JsonSerializer.Serialize(eventList);
            if (!File.Exists(pathEvents))
            {
                File.WriteAllText(pathEvents, jsonString);
            }
            else
            {
                eventList = JsonSerializer.Deserialize<ObservableCollection<EventModel>>(File.ReadAllText(pathEvents));
            }
            return eventList;
        }
        public EventModel GetEvent(int targetId)
        {
            ObservableCollection<EventModel> eventList = new ObservableCollection<EventModel>();
            string jsonString = JsonSerializer.Serialize(eventList);
            if (!File.Exists(pathEvents))
            {
                File.WriteAllText(pathEvents, jsonString);
            }
            else
            {
                eventList = JsonSerializer.Deserialize<ObservableCollection<EventModel>>(File.ReadAllText(pathEvents));
            }
            foreach(EventModel loopEvent in eventList)
            {
                if(targetId== loopEvent.EventId)
                {
                    return loopEvent;
                }
            }
            return null;
        }
        public void AddEvents(ObservableCollection<EventModel> eventList)
        {
            string jsonString = JsonSerializer.Serialize(eventList);
            File.WriteAllText(pathEvents, jsonString);
        }
    }
}
