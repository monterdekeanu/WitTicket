using WitTicket.Services;

namespace WitTicket.Model
{
    public class EventClassModel: PropertyChecker
    {
        int _classId;
        string _className;
        double _classPrice;
        int _classMaxQuantity;
        int _eventId;
        bool _isDeleted = false;

        public int ClassId { get => _classId; set {
                _classId = value;
                RaisePropertyChanged(nameof(ClassId));
            } }
        public string ClassName { get => _className; set {
                _className = value;
                RaisePropertyChanged(nameof(ClassName));
            } }
        public double ClassPrice { get => _classPrice; set { 
                _classPrice = value;
                RaisePropertyChanged(nameof(ClassPrice));
            } }
        public int ClassMaxQuantity { get => _classMaxQuantity; set { 
                _classMaxQuantity = value;
                RaisePropertyChanged(nameof(ClassMaxQuantity));
            } }
        public int EventId { get => _eventId; set {
                _eventId = value;
                RaisePropertyChanged(nameof(EventId));
                    } }
        public bool IsDeleted
        {
            get => _isDeleted; set
            {
                _isDeleted = value;
                RaisePropertyChanged(nameof(EventId));
            }
        }
        public EventClassModel() { }
        public EventClassModel(int classId, string className, double classPrice, int classMaxQuantity, int eventId)
        {
            ClassId = classId;
            ClassName = className;
            ClassPrice = classPrice;
            ClassMaxQuantity = classMaxQuantity;
            EventId = eventId;
        }
    }
}
