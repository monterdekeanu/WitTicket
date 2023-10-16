using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitTicket.Model
{
    public class UserModel : Services.PropertyChecker
    {
        private int _accountId;
        private string _accountType;
        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        private DateOnly _dateOfBirth;
        private HashSet<int> _eventsParticipated;

        public HashSet<int> EventsParticipated { get => _eventsParticipated; set
            {
                _eventsParticipated = value;
                RaisePropertyChanged(nameof(EventsParticipated));
            }
        }
        public int AccountId
        {
            get => _accountId; set
            {
                _accountId = value;
                RaisePropertyChanged(nameof(AccountId));
            }
        }
        public string AccountType { get => _accountType; set
            { 
                _accountType = value;
                RaisePropertyChanged(nameof(AccountType));
            }
        }
        public string Email { get => _email; set {
                _email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }
        public string Password { get => _password; set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
        public string FirstName { get => _firstName; set {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            } 
        }
        public string LastName { get => _lastName; set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }
        public DateOnly DateOfBirth { get => _dateOfBirth; set {
                _dateOfBirth = value;
                RaisePropertyChanged(nameof(DateOfBirth));
            }
        }

        public UserModel()
        {

        }

        public UserModel(int accountId, string accountType, string email, string password, string firstName, string lastName, DateOnly dateOfBirth)
        {
            AccountId = accountId;
            AccountType = accountType;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

    }
}
