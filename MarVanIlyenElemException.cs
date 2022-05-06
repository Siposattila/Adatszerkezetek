using System.Runtime.Serialization;

namespace ZHGyak
{
    class MarVanIlyenElemException : Exception
    {
        public MarVanIlyenElemException()
        {
        }

        public MarVanIlyenElemException(string uzenet)
            : base(uzenet)
        {
        }
    }
}