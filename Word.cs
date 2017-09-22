namespace Crozzle_App
{
    class Word
    {
        public string Value = "";
        public string OrientationIdentifier = "";
        public int RorColNumber = 0;
        public int startsAt = 0;

        public Word(string orientation, int num, string value, int starts)
        {
            OrientationIdentifier = orientation;
            RorColNumber = num;
            Value = value;
            startsAt = starts;
        }

        public override string ToString()
        {
            return this.Value;
        }

       
    }
}
