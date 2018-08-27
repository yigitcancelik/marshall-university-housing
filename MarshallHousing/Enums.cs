using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarshallHousing
{
    public enum Position
    {
        Manager,
        Cleaner,
        Security
    }

    public enum RoomCondition
    {
        Ok,
        Dirty,
        Clean
    }

    public enum Semester
    {
        Fall,
        Spring,
        Summer
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        Check
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum StudentCategory
    {
        Freshman,
        Sophomore,
        Junior,
        Senior,
        Graduate
    }

    public enum AdvisorPosition
    {
        Professor,
        AssistantProfessor,
        AssociateProfessor,
        Instructor
    }

    public enum CurrentStatus
    {
        Waiting,
        Placed
    }
}
