class Line : IComparable<Line>
{
    public Line(string line)
    {
        int pos = line.IndexOf('.');
        Number = int.Parse(line.Substring(0, pos));
        Word = line.Substring(pos + 2);
    }

    public string Build() => $"{Number}. {Word}";

    public int CompareTo(Line other)
    {
            
            int result = Word.CompareTo(other.Word);
            if (result != 0)
                return result;
            return Number.CompareTo(other.Number);
 
    }

    public int Number { get; set; }
    public string Word { get; set; }

    

}
